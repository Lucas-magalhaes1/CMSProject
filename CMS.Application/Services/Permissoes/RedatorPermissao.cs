using CMS.Application.Interfaces;

namespace CMS.Application.Services.Permissoes
{
    public class RedatorPermissao : IPermissaoUsuario
    {
        public bool PodeCriarConteudo() => true;
        public bool PodeAprovarConteudo() => false;
        public bool PodeEditarConteudo() => true;
        public bool PodeSubmeterConteudo() => true;
        public bool PodeClonarConteudo() => true;
        public bool PodeDevolverConteudo() => false;
        public bool PodeListarConteudos() =>  true;
        public bool PodeObterConteudoPorId() => true;
        public bool PodeRejeitarConteudo() => false;
        public bool PodeDeletarConteudo()  => true;
        


        public bool PodeClonarTemplate() => true;
        public bool PodeCriarTemplate() => true;
        public bool PodeListarTemplates() => true;
        public bool PodeObterTemplatePorId() => true;
        public bool PodeDeletarTemplate() => true;
       
        public bool PodeCriarUsuario() => false;
        public bool PodeListarUsuarios() => false;
        public bool PodeObterUsuarioPorId() => false;
        public bool PodeDeletarUsuario()  => false;
       
    }
}