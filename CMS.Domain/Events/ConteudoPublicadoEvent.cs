namespace CMS.Domain.Events
{
    public class ConteudoPublicadoEvent
    {
        public Guid ConteudoId { get; }
        public string Titulo { get; }
        public Guid CriadorId { get; }        
        public string CriadorNome { get; }
        public DateTime DataPublicacao { get; }

        public ConteudoPublicadoEvent(Guid conteudoId, string titulo, Guid criadorId, string criadorNome, DateTime dataPublicacao)
        {
            ConteudoId = conteudoId;
            Titulo = titulo;
            CriadorId = criadorId;
            CriadorNome = criadorNome;
            DataPublicacao = dataPublicacao;
        }
    }
}