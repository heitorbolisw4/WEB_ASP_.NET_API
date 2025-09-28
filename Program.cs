using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using MinimalApi.DTO;
using Projeto_API_WEB_ASP_.NET.Dominio.Enuns;
using Projeto_API_WEB_ASP_.NET.Dominio.ModelViews;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region Builder

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key)) key = "12345";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false

    };

});
builder.Services.AddAuthentication();

builder.Services.AddScoped<IAdministradorServicos, AdministradorServicos>();
builder.Services.AddScoped<IVeiculoServicos, VeiculoServicos>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "Jwt",
        In = ParameterLocation.Header,
        Description = "Insira o token Jwt"

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {    new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"

                },
        
        
            },
            new string[] {}
        }    
        

    });
});


//builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
//.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//.AddJsonFile($"appsettings.auth.Development.json", optional: true, reloadOnChange: true)
//.AddEnvironmentVariables();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
    );
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Admin
string GerarTokenJwt(Administrador administrador)
{
    if (string.IsNullOrEmpty(key)) return string.Empty;


    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role, administrador.Perfil),
    };

    var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        
        );
    return new JwtSecurityTokenHandler().WriteToken(token);
    
}

app.MapPost("/administradores/login", ([FromBody]LoginDTO loginDTO, IAdministradorServicos AdministradorServico) =>
{
    var adm = AdministradorServico.Login(loginDTO);

    if (adm != null)
    {
        string token = GerarTokenJwt(adm);
        
        return Results.Ok(new AdministradorLogado
        {
            Email = adm.Email,
            Perfil = adm.Perfil,
            Token = token
        });
    }
    else
    {
        return Results.Unauthorized();
    }
}).AllowAnonymous().WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServicos AdministradorServico) =>
{
    var adms = new List<AdministradorModelView>();
    var administradores = AdministradorServico.Todos(pagina);

    foreach(var adm in administradores)
    {
        adms.Add(new AdministradorModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }

    return Results.Ok(AdministradorServico.Todos(pagina));

})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute {Roles= "Adm" })
    .WithTags("Administradores");

app.MapGet("/Administradores/{id}", ([FromRoute] int id, IAdministradorServicos AdministradorServico) =>
{
    var administrador = AdministradorServico.BuscaPorId(id);

    if (administrador == null) return Results.NotFound();


    return Results.Ok(new AdministradorModelView
    {
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    });


})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServicos AdministradorServico) =>
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };
    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("Digite o seu e-mail de Admin!");

    if (string.IsNullOrEmpty(administradorDTO.Password))
        validacao.Mensagens.Add("Digite a sua senha!");

    if (administradorDTO.Perfil == null)
        validacao.Mensagens.Add("Adicione o tipo de perfil!");
    
    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    


    var administrador = new Administrador
    {
        Email = administradorDTO.Email,
        Password = administradorDTO.Password,
        Perfil = administradorDTO.Perfil.ToString()
        ?? Perfil.Editor.ToString(),
    };
            
    AdministradorServico.Incluir(administrador);

    return Results.Created($"/administrador/{administrador.Id}", new AdministradorModelView
    {
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    });




})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Administradores");
#endregion

#region Veiculos
ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDTO.Modelo))
        validacao.Mensagens.Add("O nome não pode estar vazio!");

    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("Adicione a Marca do Veículo!");

    if (veiculoDTO.Ano <= 0)
        validacao.Mensagens.Add("Adicione o Ano do Veículo!");

    return validacao;
}
;
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServicos VeiculoServico) =>
{
    var validacao = validaDTO(veiculoDTO);
    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var veiculo = new Veiculo
    { 
        Marca = veiculoDTO.Marca,
        Modelo = veiculoDTO.Modelo,
        Ano = veiculoDTO.Ano
    };

    VeiculoServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);


})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
    .WithTags("Veiculos");
app.MapGet("/veiculos", ([FromQuery]int? pagina, IVeiculoServicos VeiculoServico) =>
{
    var veiculo = VeiculoServico.Todos(pagina);
    

       return Results.Ok(veiculo);


})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
    .WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoServicos VeiculoServico) =>
{
    var veiculo = VeiculoServico.BuscaPorId(id);

    if (veiculo == null) return Results.NotFound();


    return Results.Ok(veiculo);


})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
    .WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO,IVeiculoServicos VeiculoServico) =>
{
    var veiculo = VeiculoServico.BuscaPorId(id);
    if (veiculo == null) return Results.NotFound();


    var validacao = validaDTO(veiculoDTO);
    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);


    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Modelo = veiculoDTO.Modelo;
    veiculo.Ano = veiculoDTO.Ano;

    VeiculoServico.Atualizar(veiculo);

    return Results.Ok(veiculo);


})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoServicos VeiculoServico) =>
{
    var veiculo = VeiculoServico.BuscaPorId(id);

    if (veiculo == null) return Results.NotFound();

    VeiculoServico.Apagar(veiculo);

    return Results.NoContent();


})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
    .WithTags("Veiculos");
#endregion

#region App


app.MapControllers();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion