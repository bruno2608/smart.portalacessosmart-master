using System.ComponentModel.DataAnnotations;

namespace PortalApiModel
{
    public class TransicionarSistema
    {
        [Required]
        public int CODSISINF { get; set; }
    }
    public class RetornoTransicionarSistema
    {
        public string PUBLICTOKEN { get; set; }
        public string PUBLICKEY { get; set; }
        public string DESURLLNK { get; set; }
    }
    public class ObterInformacoesUsuario
    {
        [Required]
        public string PUBLICTOKEN { get; set; }
        [Required]
        public string PUBLICKEY { get; set; }
        [Required]
        public int CODSISINF { get; set; }
    }
    public class RetornoObterInformacoesUsuario
    {
    }
}
