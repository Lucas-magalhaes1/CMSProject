using CMS.Domain.Entities;

namespace CMS.Domain.Chain.Handlers;

public class SubmeterConteudoHandler : ConteudoHandlerBase
{
    public override async Task<Conteudo> ManipularConteudo(Conteudo conteudo, string? comentario = null)
    {
        // Se o status for diferente de "Rascunho", apenas passa adiante
        if (conteudo.Status != "Rascunho")
        {
            // Ao invés de lançar exceção, apenas deixe passar o conteúdo
            return conteudo;  // O próximo handler pode ser chamado se necessário
        }

        // Se está no status "Rascunho", a mudança de status é permitida, não é necessário mais nada aqui
        return conteudo;
    }
}