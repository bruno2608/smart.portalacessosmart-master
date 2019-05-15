using System.Collections.Generic;

namespace ControleDeAcessoTO
{
    public class ObterModulosPermitidos
    {
        public decimal CODMDUSIS { get; set; }
        private List<ObterControles> controles;
        public int FLGIDTDPVMVL { get; set; }
        [ModuloClasse.AtributoPropriedade(NaoPopular = true)]
        public List<ObterControles> CONTROLES
        {
            get
            {
                if (controles == null)
                    controles = new List<ObterControles>();
                return controles;
            }
            set
            {
                controles = value;
            }
        }
    }

    public class ObterControles
    {
        public int SISTEMA { get; set; }
        public long COD_MODULO { get; set; }
        public long PERMISSAO_MODULO { get; set; }
        public long PERFIL { get; set; }
        public long COD_CONTROLE { get; set; }
        public long COD_PERMISSAO_CONTROLE { get; set; }
    }

    public class ObterSistemasPermitidos
    {
        public decimal CODSISINF { get; set; }
        // <AtributoPropriedade(NaoPopular:=False)>
        public string DESIMGSISINF { get; set; }
        public string DESSISINF { get; set; }
        public string DESURLLNK { get; set; }
        public string DESGRPRDESISSMA { get; set; }
        public int FLGIDTDPVMVL { get; set; }
    }

    public class ObterInformacoesUsuario
    {
        public int CODFNC { get; set; }
        public string NOMFNC { get; set; }
        public string NOMUSRRCF { get; set; }
        public int FLGGERPLO { get; set; }
        public string DESENDCREETNFNC { get; set; }
    }

    public class ObterSistemasUsuario
    {
        public long CODGRPRDESISSMA { get; set; }
        public string ACAO { get; set; }
    }

    public class ObterPerfisPorGrupo
    {
        public int CODPFLACS { get; set; }
    }
}
