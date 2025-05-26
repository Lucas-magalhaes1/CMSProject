public class AdminPermissao : IPermissaoUsuario
{
 
    public bool PodeCriarConteudo() => true;
    public bool PodeAprovarConteudo() => true;
    public bool PodeEditarConteudo() => true;
    public bool PodeSubmeterConteudo() => true;
    public bool PodeClonarConteudo() => true;
    public bool PodeDevolverConteudo() => true;
    public bool PodeListarConteudos() => true;
    public bool PodeObterConteudoPorId() => true;
    public bool PodeRejeitarConteudo() => true;
    

    public bool PodeClonarTemplate() => true;
    public bool PodeCriarTemplate() => true;
    public bool PodeListarTemplates() => true;
    public bool PodeObterTemplatePorId() => true;
    public bool PodeDeletarTemplate()  => true;
    

    public bool PodeCriarUsuario() => true;
    public bool PodeListarUsuarios() => true;
    public bool PodeObterUsuarioPorId() => true;
    public bool PodeDeletarUsuario()  => true;
    public bool PodeDeletarConteudo()  => true;
}