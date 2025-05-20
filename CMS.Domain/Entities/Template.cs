using System.Collections.Generic;
using CMS.Domain.Services;
using CMS.Domain.ValueObjects;

namespace CMS.Domain.Entities;

public class Template : IPrototype<Template>
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; }
    public List<CampoTemplate> Campos { get; private set; } = new();

    // Construtor protegido para EF Core
    protected Template() { }

    // Construtor público usado no UseCase
    public Template(string nome, List<CampoTemplate> campos)
    {
        Nome = nome;
        Campos = campos;
    }

    public Template Clone()
    {
        var camposClone = new List<CampoTemplate>();
        foreach (var campo in Campos)
        {
            camposClone.Add(new CampoTemplate(campo.Nome, campo.Tipo, campo.Obrigatorio));
        }
        return new Template(Nome, camposClone);
    }
}
