using CMS.Domain.Entities;
using System.Collections.Generic;

namespace CMS.Domain.Entities;

public class Conteudo
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Titulo { get; private set; }
    public Template Template { get; private set; }
    public List<CampoPreenchido> CamposPreenchidos { get; private set; } = new();
    
    public string Status { get;  set; }

    // Construtor para EF Core
    protected Conteudo() { }

    public Conteudo(string titulo, Template template, List<CampoPreenchido> camposPreenchidos)
    {
        Titulo = titulo;
        Template = template;
        CamposPreenchidos = camposPreenchidos;
        Status = "Rascunho";
    }
    
    // Método de clonagem
    public Conteudo Clone()
    {
        var camposClone = new List<CampoPreenchido>(CamposPreenchidos);
        return new Conteudo(Titulo + " - Cópia", Template, camposClone);
    }

    // Métodos para manipulação de conteúdo
    public void Submeter()
    {
        Status = "Submetido";
    }

    public void AlterarConteudo(List<CampoPreenchido> camposPreenchidos)
    {
        CamposPreenchidos = camposPreenchidos;
    }
}