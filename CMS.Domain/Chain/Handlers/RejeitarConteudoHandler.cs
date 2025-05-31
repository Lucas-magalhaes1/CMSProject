using CMS.Domain.Entities;

namespace CMS.Domain.Chain.Handlers
{
    public class RejeitarConteudoHandler : ConteudoHandlerBase
    {
        public override async Task<Conteudo> ManipularConteudo(Conteudo conteudo, string? comentario = null)
        {
            if (conteudo.Status == "Submetido")
            {
                conteudo.Status = "Rejeitado";
                
                if (!string.IsNullOrEmpty(comentario))
                {
                    conteudo.Comentario = comentario;
                }

                return conteudo;
            }
            else if (ProximoHandler != null)
            {
                return await ProximoHandler.ManipularConteudo(conteudo, comentario);
            }

            throw new InvalidOperationException("Conteúdo não pode ser rejeitado.");
        }
    }
}