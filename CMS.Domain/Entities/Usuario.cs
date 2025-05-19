using CMS.Domain.Enums;

namespace CMS.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public PapelUsuario Papel { get; private set; }


    protected Usuario() {} 

    public Usuario(string nome, string email, string senhaHash, PapelUsuario papel)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Papel = papel;
    }

    public void AlterarNome(string novoNome)
    {
        Nome = novoNome;
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        SenhaHash = novaSenhaHash;
    }

    public void AlterarPapel(PapelUsuario novoPapel)
    {
        Papel = novoPapel;
    }
}