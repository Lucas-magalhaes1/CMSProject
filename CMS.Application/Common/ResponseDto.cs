namespace CMS.Application.DTOs;

public class ResponseDto<T>
{
    public bool Sucesso { get; set; }
    public string? Mensagem { get; set; }
    public T? Dados { get; set; }

    public static ResponseDto<T> Ok(T dados, string? mensagem = null) => new()
    {
        Sucesso = true,
        Mensagem = mensagem,
        Dados = dados
    };

    public static ResponseDto<T> Falha(string mensagem) => new()
    {
        Sucesso = false,
        Mensagem = mensagem
    };
}