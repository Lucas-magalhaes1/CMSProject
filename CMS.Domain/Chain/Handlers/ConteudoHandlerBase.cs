using CMS.Domain.Entities;

namespace CMS.Domain.Chain.Handlers
{
    public abstract class ConteudoHandlerBase
    {
        protected ConteudoHandlerBase? ProximoHandler;

        public void DefinirProximoHandler(ConteudoHandlerBase handler)
        {
            ProximoHandler = handler;
        }

        
        public abstract Task<Conteudo> ManipularConteudo(Conteudo conteudo, string? comentario = null);
    }
}