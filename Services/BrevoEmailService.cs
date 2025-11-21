using ControleFuncionarios.Models;
using brevo_csharp.Api;
using brevo_csharp.Model;

namespace ControleFuncionarios.Services
{
    public class BrevoEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly TransactionalEmailsApi _apiInstance;

        public BrevoEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiInstance = new TransactionalEmailsApi();
        }

        public async Task<bool> EnviarEmailFormularioAsync(FormularioRequest dados)
        {

            var destinatariosConfig = _configuration["EmailSettings:DestinatariosNotificacao"];
            var remetenteEmail = _configuration["EmailSettings:Remetente"];
            
            if (string.IsNullOrEmpty(remetenteEmail) || string.IsNullOrEmpty(destinatariosConfig))
            {
                Console.WriteLine("Erro de Configuração: E-mail remetente ou destinatário(s) estão ausentes.");
                return false;
            }
            
            var listaDestinatarios = new List<SendSmtpEmailTo>();

            var arrayEmails = destinatariosConfig
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToArray();

            foreach (var email in arrayEmails)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    listaDestinatarios.Add(new SendSmtpEmailTo(email: email));
                }
            }
            
            if (listaDestinatarios.Count == 0)
            {
                 Console.WriteLine("Erro de Configuração: Nenhum destinatário válido foi encontrado.");
                 return false;
            }
            
            string htmlContent = $@"
                <h1>Registro de Atividades - {dados.RespostaPerguntaUm}</h1>
                <p><strong>Digite seu nome :</strong> {dados.RespostaPerguntaUm}</p>
                <p><strong>Como foi o dia de hoje? :</strong> {dados.RespostaPerguntaDois}</p>
                <p><strong>Houve alguma dificuldade? :</strong> {dados.RespostaPerguntaTres}</p>
                <p><strong>Como será o dia de amanhã? :</strong> {dados.RespostaPerguntaQuatro}</p>
                <p><strong>Link da Task:</strong> {dados.RespostaPerguntaQuinto}</p>
                <br>
                <small>Enviado via ASP.NET Core e Brevo.</small>";
            
            var dataAtual = DateTime.Now;
            var dataFormatada = dataAtual.ToString("dd/MM/yyyy");

            var sendSmtpEmail = new SendSmtpEmail(
                
                to: listaDestinatarios, 
                sender: new SendSmtpEmailSender(email: remetenteEmail),
                subject: $"Registro de Atividades de {dados.RespostaPerguntaUm} - {dataFormatada}",
                htmlContent: htmlContent
            );


            try
            {
                await _apiInstance.SendTransacEmailAsync(sendSmtpEmail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                return false;
            }
        }
    }
}