using CMS.Domain.Entities;

namespace CMS.Domain.Chain.Handlers
{
    public class DevolverConteudoHandler : ConteudoHandlerBase
    {
        private readonly string _comentario;
        
        public DevolverConteudoHandler()
        {
            _comentario = string.Empty; 
        }
        
        public DevolverConteudoHandler(string comentario)
        {
            _comentario = comentario;
        }

        public override async Task<Conteudo> ManipularConteudo(Conteudo conteudo, string comentario)
        {
            if (conteudo.Status == "Submetido")
            {
                conteudo.DevolverParaCorrecao(comentario);  
                return conteudo;
            }
            else if (ProximoHandler != null)
            {
                return await ProximoHandler.ManipularConteudo(conteudo, comentario);
            }

            throw new InvalidOperationException("Conteúdo não pode ser devolvido.");
        }
    }
}