using MinimalApi.Dominio.Entidades;
using MinimalApi.DTO;

namespace MinimalApi.Dominio.Interfaces;

public interface IAdministradorServicos
{
    Administrador? Login(LoginDTO loginDTO);
    Administrador Incluir(Administrador administrador);
    Administrador Excluir(Administrador administrador);
    Administrador? BuscaPorId(int id);

    List<Administrador> Todos(int? pagina);

}
