using CMS.Domain.Entities;

namespace CMS.Application.Interfaces;

public interface ITemplateRepository
{
    Task<Template> CriarAsync(Template template);
    Task<List<Template>> ListarAsync();
    Task<Template?> ObterPorIdAsync(Guid id);
    Task DeletarAsync(Template template);
}