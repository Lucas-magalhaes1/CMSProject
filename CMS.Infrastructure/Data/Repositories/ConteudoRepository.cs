using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Data.Repositories
{
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
            // Retorna todos os conteúdos, incluindo Template e campos preenchidos
            return await _context.Conteudos
                .Include(c => c.Template)  // Inclui o Template junto com o Conteudo
                .Include(c => c.CamposPreenchidos)  // Inclui os campos preenchidos
                .ToListAsync();
        }

        public async Task<List<Conteudo>> ListarPorCriadorAsync(Guid usuarioId)
        {
            // Filtra para retornar apenas os conteúdos do usuário logado
            return await _context.Conteudos
                .Where(c => c.CriadoPor == usuarioId) // Filtra por criador
                .Include(c => c.Template)  // Inclui o Template junto com o Conteudo
                .Include(c => c.CamposPreenchidos)  // Inclui os campos preenchidos
                .ToListAsync();
        }

        public async Task<Conteudo?> ObterPorIdAsync(Guid id)
        {
            // Retorna um conteúdo específico por ID, incluindo Template e campos preenchidos
            return await _context.Conteudos
                .Include(c => c.Template)  // Inclui o Template ao buscar o Conteudo
                .Include(c => c.CamposPreenchidos)  // Inclui os campos preenchidos
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AtualizarAsync(Conteudo conteudo)
        {
            _context.Conteudos.Update(conteudo); 
            await _context.SaveChangesAsync();    
        }

        public async Task DeletarAsync(Conteudo conteudo)
        {
            _context.Conteudos.Remove(conteudo);
            await _context.SaveChangesAsync();
        }
    }
}
