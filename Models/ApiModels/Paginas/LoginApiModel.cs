using System.ComponentModel.DataAnnotations;


namespace LoginApiModel
{
    public class EfetuarLogin
    {
        [Required]
        public string USUARIO { get; set; }
        [Required]
        public string SENHA { get; set; }
    }

    public class RetornoEfetuarLogin
    {
        public int CODIGO { get; set; }
        public string MENSAGEM { get; set; }
    }
}
