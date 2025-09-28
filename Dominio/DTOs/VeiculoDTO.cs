

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.DTO;

public record VeiculoDTO
{



    public string Modelo { get; set; } = default!;

    public string Marca { get; set; } = default!;

    public int Ano { get; set; } = default!;

}
