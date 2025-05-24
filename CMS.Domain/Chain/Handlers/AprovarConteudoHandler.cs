using CMS.Domain.Entities;

namespace CMS.Domain.Chain.Handlers
{
    public class AprovarConteudoHandler : ConteudoHandlerBase
    {
        public override async Task<Conteudo> ManipularConteudo(Conteudo conteudo,  string? comentario = null)
        {
            if (conteudo.Status == "Submetido")
            {
                conteudo.Status = "Aprovado";
                return conteudo;
            }
            else if (ProximoHandler != null)
            {
                return await ProximoHandler.ManipularConteudo(conteudo);
            }

            throw new InvalidOperationException("Conteúdo não pode ser aprovado.");
        }
    }
}