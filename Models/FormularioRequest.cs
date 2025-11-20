using System.ComponentModel.DataAnnotations;


namespace ControleFuncionarios.Models
{
    public class FormularioRequest
    {
        [Required(ErrorMessage = "A primeira pergunta é obrigatória.")]
        public string RespostaPerguntaUm { get; set; }

        [Required(ErrorMessage = "A segunda pergunta é obrigatória.")]
        public string RespostaPerguntaDois { get; set; }

        public string RespostaPerguntaTres { get; set; } 
        
        public string RespostaPerguntaQuatro { get; set; } 
        public string RespostaPerguntaQuinto { get; set; } 
    }
}
