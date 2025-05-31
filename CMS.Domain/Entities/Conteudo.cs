    using CMS.Domain.Entities;
    using System.Collections.Generic;

    namespace CMS.Domain.Entities;

    public class Conteudo
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Titulo { get; private set; }
        public Template Template { get; private set; }
        public List<CampoPreenchido> CamposPreenchidos { get; private set; } = new();
        public string? Comentario { get; set; }
        
        public Guid CriadoPor { get; private set; }
        public string Status { get;  set; }
        
        public string NomeCriador { get; set; } = null!;
        
        protected Conteudo() { }

        public Conteudo(string titulo, Template template, List<CampoPreenchido> camposPreenchidos, Guid criadoPor,string nomeCriador)
        {
            Titulo = titulo;
            Template = template;
            CamposPreenchidos = camposPreenchidos;
            Status = "Rascunho";
            CriadoPor = criadoPor;
            NomeCriador = nomeCriador ?? throw new ArgumentNullException(nameof(nomeCriador));
        }
        
        public Conteudo Clone()
        {
            var camposClone = new List<CampoPreenchido>(CamposPreenchidos);
            return new Conteudo(Titulo + " - Cópia", Template, camposClone, CriadoPor, NomeCriador);
        }
        
        public void Submeter()
        {
            Status = "Submetido";
        }

        public void AlterarConteudo(List<CampoPreenchido> camposPreenchidos)
        {
            CamposPreenchidos = camposPreenchidos;
        }
        
        public void DevolverParaCorrecao(string comentario)
        {
            Status = "Rascunho";  
            Comentario = comentario; 
        }
        
        public void AlterarTitulo(string novoTitulo)
        {
            Titulo = novoTitulo;
        }
    }