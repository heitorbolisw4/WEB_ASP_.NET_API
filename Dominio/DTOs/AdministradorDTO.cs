

using Projeto_API_WEB_ASP_.NET.Dominio.Enuns;

namespace MinimalApi.DTO;

public class AdministradorDTO
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public Perfil? Perfil { get; set; } = default!;

}
