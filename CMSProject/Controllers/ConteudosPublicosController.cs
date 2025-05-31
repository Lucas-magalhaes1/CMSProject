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
    [AllowAnonymous] // Permite acesso sem autenticação
    public async Task<IActionResult> Get()
    {
        var conteudos = await _listarConteudosAprovadosUseCase.ExecuteAsync();

        // Opcional: mapear para DTO, se desejar

        return Ok(conteudos);
    }
}