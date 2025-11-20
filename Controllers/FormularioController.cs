
using Microsoft.AspNetCore.Mvc;
using ControleFuncionarios.Models;
using ControleFuncionarios.Services;

namespace ControleFuncionarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularioController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public FormularioController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("enviar-email")]
        public async Task<IActionResult> EnviarEmail([FromBody] FormularioRequest request)
        {
            // Validação básica (definida no FormularioRequest com [Required])
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna 400 Bad Request
            }

            bool sucesso = await _emailService.EnviarEmailFormularioAsync(request);

            if (sucesso)
            {
                // Retorna 200 OK para o React
                return Ok(new { Mensagem = "E-mail enviado com sucesso!" });
            }
            else
            {
                // Retorna 500 Internal Server Error (Erro no envio via Brevo)
                return StatusCode(500, new { Mensagem = "Falha ao enviar o e-mail. Tente novamente." });
            }
        }
    }
}