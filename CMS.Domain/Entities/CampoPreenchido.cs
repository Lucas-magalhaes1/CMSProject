namespace CMS.Domain.Entities;

public class CampoPreenchido
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public string Valor { get; private set; }  
    
    protected CampoPreenchido() { }

    public CampoPreenchido(string nome, string valor)  
    {
        Nome = nome;
        Valor = valor;  
    }

    public string GetValor()
    {
        return Valor; 
    }
    
    public void AtualizarValor(string novoValor)
    {
        Valor = novoValor;
    }
}