var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapGet("/", () => "Hello World");
//app.UseHttpsRedirection();

app.MapPost("/login", (MinimaApi.DTO.LoginDTO LoginDTO) =>
{
    if (LoginDTO.Email == "adm@exemplo.com" && LoginDTO.Pasworld == "12345")
    {
        return Results.Ok("Login efetuado com sucesso!");
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.UseAuthorization();

app.MapControllers();

app.Run();
