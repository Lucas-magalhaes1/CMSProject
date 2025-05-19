namespace CMS.Application.DTOs;

public class UsuarioResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Papel { get; set; } = null!;
}