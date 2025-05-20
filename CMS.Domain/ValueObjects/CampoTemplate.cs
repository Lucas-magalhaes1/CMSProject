using CMS.Domain.Enums;

namespace CMS.Domain.ValueObjects;

public class CampoTemplate
{
    public string Nome { get; private set; }
    public TipoCampo Tipo { get; private set; }
    public bool Obrigatorio { get; private set; }
    
    protected CampoTemplate() { }
    
    public CampoTemplate(string nome, TipoCampo tipo, bool obrigatorio)
    {
        Nome = nome;
        Tipo = tipo;
        Obrigatorio = obrigatorio;
    }
}