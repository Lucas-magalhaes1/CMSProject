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
    bool PodeDeletarConteudo();  

    bool PodeClonarTemplate();
    bool PodeCriarTemplate();
    bool PodeListarTemplates();
    bool PodeObterTemplatePorId();
    
    bool PodeDeletarTemplate();   
    bool PodeCriarUsuario();
    bool PodeListarUsuarios();
    bool PodeObterUsuarioPorId();
    bool PodeDeletarUsuario();  
}