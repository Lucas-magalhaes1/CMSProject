namespace CMS.Application.DTOs;

public class UsuarioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Papel { get; set; }
}