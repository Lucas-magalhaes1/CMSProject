using CMS.Domain.Enums;

namespace CMS.Application.DTOs;

public class CampoTemplateDto
{
    public string Nome { get; set; } = null!;
    public TipoCampo Tipo { get; set; }
    public bool Obrigatorio { get; set; }
}