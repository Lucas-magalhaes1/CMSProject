using System;

namespace CMS.Domain.Entities
{
    public class Notificacao
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UsuarioId { get; private set; }
        public string Titulo { get; private set; } = null!;
        public string Mensagem { get; private set; } = null!;
        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
        public bool Lida { get; private set; } = false;

        protected Notificacao() { } 

        public Notificacao(Guid usuarioId, string titulo, string mensagem)
        {
            UsuarioId = usuarioId;
            Titulo = titulo;
            Mensagem = mensagem;
        }

        public void MarcarComoLida()
        {
            Lida = true;
        }
    }
}