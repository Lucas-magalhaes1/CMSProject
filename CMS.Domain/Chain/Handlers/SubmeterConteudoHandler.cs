using CMS.Domain.Entities;

namespace CMS.Domain.Chain.Handlers;

public class SubmeterConteudoHandler : ConteudoHandlerBase
{
    public override async Task<Conteudo> ManipularConteudo(Conteudo conteudo, string? comentario = null)
    {
        if (conteudo.Status != "Rascunho")
        {
            
            return conteudo;  
        }
        
        return conteudo;
    }
}