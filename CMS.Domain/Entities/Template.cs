using CMS.Domain.ValueObjects;

public class Template
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public List<CampoTemplate> Campos { get; private set; } = new();

    public Guid CriadoPor { get; private set; }
    public string NomeCriador { get; private set; } = null!;

    // Construtor para EF Core
    protected Template() { }

    // Construtor principal, agora com criadoPor e nomeCriador
    public Template(string nome, List<CampoTemplate> campos, Guid criadoPor, string nomeCriador)
    {
        Nome = nome;
        Campos = campos;
        CriadoPor = criadoPor;
        NomeCriador = nomeCriador ?? throw new ArgumentNullException(nameof(nomeCriador));
    }

    // Clone atualizado para manter CriadoPor e NomeCriador
    public Template Clone()
    {
        var camposClone = new List<CampoTemplate>();
        foreach (var campo in Campos)
        {
            camposClone.Add(new CampoTemplate(campo.Nome, campo.Tipo, campo.Obrigatorio));
        }

        return new Template(Nome, camposClone, CriadoPor, NomeCriador);
    }
}