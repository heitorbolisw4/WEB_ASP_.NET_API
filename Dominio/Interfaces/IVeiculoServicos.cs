using MinimalApi.Dominio.Entidades;
using MinimalApi.DTO;

namespace MinimalApi.Dominio.Interfaces;

public interface IVeiculoServicos
{
    List<Veiculo> Todos(int? pagina = 1, string? marca = null, string? modelo = null);

    Veiculo? BuscaPorId(int id);

    void Incluir(Veiculo veiculo);
    void Atualizar(Veiculo veiculo);

    void Apagar(Veiculo veiculo);


}
