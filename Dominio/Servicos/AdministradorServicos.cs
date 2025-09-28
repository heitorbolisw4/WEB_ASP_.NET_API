using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.DTO;
using System.Text.RegularExpressions;

namespace MinimalApi.Dominio.Servicos;

public class AdministradorServicos : IAdministradorServicos
{
    private readonly DbContexto _contexto;
    public AdministradorServicos(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administrador Excluir(Administrador administrador)
    {
        _contexto.Administradores.Remove(administrador);
        _contexto.SaveChanges();
        return administrador;
       
    }

    public Administrador Incluir(Administrador administrador)
    {
        _contexto.Administradores.Add(administrador);
        _contexto.SaveChanges();
        return administrador;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
        {
            return adm;
        }
    }

    public List<Administrador> Todos(int? pagina)
    {
        var query = _contexto.Administradores.AsQueryable();

        int itensPorPagina = 10;

        if (pagina != null)
        {
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

        }

        return query.ToList();
    }
    public Administrador? BuscaPorId(int id)
    {
        return _contexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
    }
}
