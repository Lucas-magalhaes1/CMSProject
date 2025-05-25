public interface IPermissaoUsuario
{
    bool PodeCriarConteudo();
    bool PodeAprovarConteudo();
    bool PodeEditarConteudo();
    bool PodeSubmeterConteudo();
    bool PodeClonarConteudo();
    bool PodeDevolverConteudo();
    bool PodeListarConteudos();
    bool PodeObterConteudoPorId();
    bool PodeRejeitarConteudo();

    bool PodeClonarTemplate();
    bool PodeCriarTemplate();
    bool PodeListarTemplates();
    bool PodeObterTemplatePorId();
    
    bool PodeCriarUsuario();
    bool PodeListarUsuarios();
    bool PodeObterUsuarioPorId();
}