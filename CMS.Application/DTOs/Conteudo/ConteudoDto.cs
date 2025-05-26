using CMS.Application.DTOs;

public class ConteudoDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = null!;
    public Guid TemplateId { get; set; }
    public string Status { get; set; } = "Rascunho";
    public List<CampoConteudoDto> CamposPreenchidos { get; set; } = new();
    public string? NomeCriador { get; set; }
    public string? Comentario { get; set; }
}