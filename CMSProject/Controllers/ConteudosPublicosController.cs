using CMS.Application.UseCases.Conteudos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/public/conteudos")]
public class ConteudosPublicosController : ControllerBase
{
    private readonly ListarConteudosAprovadosUseCase _listarConteudosAprovadosUseCase;

    public ConteudosPublicosController(ListarConteudosAprovadosUseCase listarConteudosAprovadosUseCase)
    {
        _listarConteudosAprovadosUseCase = listarConteudosAprovadosUseCase;
    }

    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> Get()
    {
        var conteudos = await _listarConteudosAprovadosUseCase.ExecuteAsync();
        

        return Ok(conteudos);
    }
}