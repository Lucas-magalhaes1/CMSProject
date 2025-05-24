using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Data.Repositories;

public class ConteudoRepository : IConteudoRepository
{
    private readonly AppDbContext _context;

    public ConteudoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Conteudo> CriarAsync(Conteudo conteudo)
    {
        _context.Conteudos.Add(conteudo);
        await _context.SaveChangesAsync();
        return conteudo;
    }

    public async Task<List<Conteudo>> ListarAsync()
    {
        return await _context.Conteudos
            .Include(c => c.Template)  // Inclui o Template junto com o Conteudo
            .ToListAsync();
    }
    public async Task<Conteudo?> ObterPorIdAsync(Guid id)
    {
        return await _context.Conteudos
            .Include(c => c.Template)  // Inclui o Template ao buscar o Conteudo
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AtualizarAsync(Conteudo conteudo)
    {
        _context.Conteudos.Update(conteudo); 
        await _context.SaveChangesAsync();    
    }
}