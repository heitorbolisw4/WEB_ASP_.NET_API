using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;

    namespace MinimalApi.DTO;

public class DbContexto : DbContext
{

    private readonly IConfiguration _ConfigurationAppSettings;
    public DbContexto(IConfiguration ConfigurationAppSettings)
    {
        _ConfigurationAppSettings = ConfigurationAppSettings;
    }

    public DbSet<Administrador> Administradores { get; set; } = default!;
    public DbSet<Veiculo> Veiculos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>().HasData(
            
            new Administrador
            {
                Id = 1,
                Email = "adm@exemplo.com",
                Password = "12345",
                Perfil = "Adm"

            }
            

        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        if (!optionBuilder.IsConfigured)
        {
            var stringConection = _ConfigurationAppSettings.GetConnectionString("MySql")?.ToString();
            if (!string.IsNullOrEmpty(stringConection))
            {
                optionBuilder.UseMySql(stringConection, ServerVersion.AutoDetect(stringConection));

            }
        }
    }
}
