using System.Collections.Generic;

namespace CMS.Application.DTOs;

public class TemplateDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public List<CampoTemplateDto> Campos { get; set; } = new();
}