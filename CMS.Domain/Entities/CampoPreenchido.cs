namespace CMS.Domain.Entities;

public class CampoPreenchido
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public string Valor { get; private set; }  // Valor agora é apenas string

    // Construtor para EF Core
    protected CampoPreenchido() { }

    public CampoPreenchido(string nome, string valor)  // Valor é agora do tipo string
    {
        Nome = nome;
        Valor = valor;  // Armazenando valor diretamente como string
    }

    public string GetValor()
    {
        return Valor;  // Apenas retorna o valor como string
    }
    
    // Método para atualizar o valor
    public void AtualizarValor(string novoValor)
    {
        Valor = novoValor;
    }
}