using CMS.Application.Interfaces;
using CMS.Application.Services.Permissoes;
using CMS.Domain.Services;
using CMS.Domain.Enums;

namespace CMS.Application.Services
{
    public static class PermissaoFactory
    {
        public static IPermissaoUsuario CriarPermissao(PapelUsuario papel)
        {
            return papel switch
            {
                PapelUsuario.Admin => new AdminPermissao(),
                PapelUsuario.Editor => new EditorPermissao(),
                PapelUsuario.Redator => new RedatorPermissao(),
                _ => throw new ArgumentException("Papel desconhecido")
            };
        }
    }
}