
namespace CMS.Application.Interfaces;

public interface IAuthService
{
    string GerarToken(Guid usuarioId, string papel);
}