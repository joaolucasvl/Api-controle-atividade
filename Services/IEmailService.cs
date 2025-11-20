
using ControleFuncionarios.Models;

namespace ControleFuncionarios.Services
{
    public interface IEmailService
    {
        Task<bool> EnviarEmailFormularioAsync(FormularioRequest dados);
    }
}