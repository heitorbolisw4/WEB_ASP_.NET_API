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

app.MapPost("/login", (LoginADM LoginADM) =>
{
    if (LoginADM.Email == "adm@exemplo.com" && LoginADM.Pasworld == "12345")
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
public class LoginADM
{
    public string Email { get; set; } = default!;
    public string Pasworld { get; set; } = default!;

}
