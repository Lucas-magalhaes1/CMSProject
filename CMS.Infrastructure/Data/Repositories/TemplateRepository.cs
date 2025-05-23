using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Data.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly AppDbContext _context;

    public TemplateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Template> CriarAsync(Template template)
    {
        _context.Templates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task<List<Template>> ListarAsync()
    {
        return await _context.Templates.ToListAsync();
    }

    public async Task<Template?> ObterPorIdAsync(Guid id)
    {
        return await _context.Templates.FindAsync(id);
    }
}