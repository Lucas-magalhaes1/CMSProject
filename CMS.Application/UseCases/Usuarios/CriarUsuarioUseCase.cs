using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using CMS.Domain.Enums;
using BCrypt.Net;

namespace CMS.Application.UseCases.Usuarios;

public class CriarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ResponseDto<string>> ExecuteAsync(string nome, string email, string senhaPura, string papel)
    {
        if (string.IsNullOrWhiteSpace(nome)) return ResponseDto<string>.Falha("Nome é obrigatório");
        if (string.IsNullOrWhiteSpace(email)) return ResponseDto<string>.Falha("Email é obrigatório");
        if (string.IsNullOrWhiteSpace(senhaPura)) return ResponseDto<string>.Falha("Senha é obrigatória");
        if (string.IsNullOrWhiteSpace(papel)) return ResponseDto<string>.Falha("Papel é obrigatório");

        if (!Enum.TryParse(papel, true, out PapelUsuario papelEnum))
            return ResponseDto<string>.Falha("Papel inválido. Use: Admin, Editor ou Redator");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(senhaPura);

        var usuario = new Usuario(nome, email, senhaHash, papelEnum);
        await _usuarioRepository.CriarAsync(usuario);

        return ResponseDto<string>.Ok("Usuário criado com sucesso");
    }
}