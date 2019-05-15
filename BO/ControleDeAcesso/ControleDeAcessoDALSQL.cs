using System.Text;

public class ControleDeAcessoDALSQL
{
    public string ObterSistemasPermitidos()
    {
        return @"
            WITH CONSULTA AS (
                SELECT DISTINCT S.CODSISINF, S.DESSISINF, S.DESURLLNK, TRIM(UPPER(Z.DESGRPRDESISSMA)) AS DESGRPRDESISSMA,
                CASE WHEN 
                    (SELECT COUNT(*) FROM MRT.CADMDUSISSMA M WHERE S.CODSISINF = M.CODSISINF
                     AND FLGIDTDPVMVL = 1) 
                > 0 THEN 1
                    ELSE 0
                END AS FLGIDTDPVMVL
                FROM MRT.CADSISSISSMA S
                INNER JOIN MRT.RLCPFLSISSISSMA R ON R.CODSISINF = S.CODSISINF
                INNER JOIN MRT.RLCPFLUSRRDESISSMA X ON X.CODPFLACS = R.CODPFLACS
                INNER JOIN MRT.CADGRPRDESISSMA Z ON Z.CODGRPRDESISSMA = X.CODGRPRDESISSMA
                WHERE R.CODSISINF <> :CODSISINFPORTAL
                AND X.CODFNC = :CODFNC
                AND X.DATHRADST IS NULL
                AND R.DATHRADST IS NULL
                AND EXISTS (SELECT 1 FROM MRT.CADMDUSISSMA M WHERE S.CODSISINF = M.CODSISINF)
            )
            SELECT C.*, S.DESIMGSISINF
            FROM CONSULTA C
            JOIN MRT.CADSISSISSMA S ON S.CODSISINF = C.CODSISINF
            ORDER BY C.CODSISINF
                ";
    }

    public string ObterModulosPermitidos()
    {
        return @"
                SELECT M.CODMDUSIS, M.FLGIDTDPVMVL
                FROM MRT.CADSISSISSMA S
                INNER JOIN MRT.RLCPFLSISSISSMA R ON R.CODSISINF = S.CODSISINF
                INNER JOIN MRT.RLCPFLUSRRDESISSMA X ON X.CODPFLACS = R.CODPFLACS
                INNER JOIN MRT.CADPFLUSRSISSMA P ON P.CODPFLACS = X.CODPFLACS
                                                AND P.CODGRPRDESISSMA = X.CODGRPRDESISSMA
                INNER JOIN MRT.CADMDUSISSMA M ON S.CODSISINF = M.CODSISINF
                INNER JOIN MRT.RLCARZMDUSISSMA Q ON Q.CODMDUSIS = M.CODMDUSIS
                                                AND Q.CODPFLACS = P.CODPFLACS
                                                AND Q.CODARZACS = ( SELECT CODARZACS 
                                                                      FROM MRT.CADARZSISSMA
                                                                     WHERE UPPER(DESARZSIS) = 'VISUALIZAR'
                                                                   )
                                                AND Q.DATHRADST IS NULL
                WHERE S.CODSISINF = :CODSISINF
                AND X.CODFNC = :CODFNC
                ";
    }

    /// <summary>
    /// Obtem os controles permitidos de cada tela.
    /// </summary>
    /// <returns></returns>
    public string ObterControlesPermitidosOld()
    {
        return @"
            SELECT CON.CODCONMDUSIS
            FROM MRT.CADCONMDUSISSMA CON
            JOIN MRT.RLCARZCONMDUSISSMA RLCARZ
            ON RLCARZ.CODCONMDUSIS = CON.CODCONMDUSIS
            AND RLCARZ.CODMDUSIS = CON.CODMDUSIS
            JOIN MRT.RLCPFLUSRRDESISSMA RLCPFL
            ON RLCPFL.CODPFLACS = RLCARZ.CODPFLACS
            JOIN MRT.RLCARZMDUSISSMA RLCMDU
            ON RLCMDU.CODARZACS = RLCARZ.CODARZACS
            AND RLCMDU.CODPFLACS = RLCPFL.CODPFLACS
            AND RLCMDU.CODMDUSIS = CON.CODMDUSIS
            JOIN MRT.RLCPFLSISSISSMA RLCSIS
            ON RLCSIS.CODPFLACS = RLCPFL.CODPFLACS
            WHERE CON.CODMDUSIS = :CODMDUSIS
            AND RLCSIS.CODSISINF = :CODSISINF
            AND RLCPFL.CODFNC = :CODFNC
                ";
    }

    /// <summary>
    /// Obtem os Controles Permitidos para
    /// cada modulo, perfil.
    /// </summary>
    /// <returns></returns>
    /// <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public string ObterControlesPermitidos()
    {
        return @"
            SELECT C.CODSISINF SISTEMA,
            B.CODMDUSIS COD_MODULO,
            B.CODARZACS PERMISSAO_MODULO,
            B.CODPFLACS PERFIL,
            NVL(E.CODCONMDUSIS, 0) COD_CONTROLE,
            NVL(E.CODARZACS, 0) COD_PERMISSAO_CONTROLE
            FROM MRT.RLCPFLUSRRDESISSMA A
            INNER JOIN MRT.RLCARZMDUSISSMA B
            ON A.CODPFLACS = B.CODPFLACS
            AND B.DATHRADST IS NULL
            INNER JOIN MRT.CADMDUSISSMA C ON B.CODMDUSIS = C.CODMDUSIS
            INNER JOIN MRT.CADMDUSISSMA D ON B.CODMDUSIS = C.CODMDUSIS
            LEFT JOIN MRT.RLCARZCONMDUSISSMA E ON E.CODPFLACS = A.CODPFLACS
            AND E.CODMDUSIS = B.CODMDUSIS
            AND E.DATHRADSTRGT IS NULL
            WHERE C.CODSISINF = :CODSISINF
            AND A.CODFNC = :CODFNC
            --AND A.CODPFLACS = :CODPFLACS
            AND A.DATHRADST IS NULL
            AND B.CODMDUSIS = :CODMDUSIS
            GROUP BY C.CODSISINF,
            B.CODMDUSIS,
            B.CODARZACS,
            B.CODPFLACS,
            E.CODCONMDUSIS,
            E.CODARZACS ORDER BY C.CODSISINF,
            B.CODMDUSIS,
            B.CODARZACS,
            B.CODPFLACS,
            E.CODCONMDUSIS
                ";
    }

    public string ObterInformacoesUsuario(int CODFNC)
    {
        StringBuilder strBld = new StringBuilder(@"
            SELECT A.CODFNC, A.NOMFNC, UPPER(TRIM(B.NOMUSRRCF)) AS NOMUSRRCF,
            TRIM(A.DESENDCREETNFNC) DESENDCREETNFNC,
            CASE WHEN COUNT(PLO.CODPLO) > 0 THEN 1 ELSE 0 END FLGGERPLO
            FROM MRT.T0100361 A
            INNER JOIN MRT.T0104596 B ON (B.CODFNC = A.CODFNC)
            LEFT JOIN MRT.T0148372 PLO
                ON A.CODFNC = PLO.CODFNCGERPLO                  
            WHERE A.DATDEMFNC IS NULL
                ");

        if (CODFNC > 0)
        {
            strBld.AppendLine(" AND B.CODFNC = :CODFNC ");
        }
        else
        {
            strBld.AppendLine(" AND UPPER(TRIM(B.NOMUSRRCF)) = UPPER(TRIM(:NOMUSRRCF)) ");
        }

        strBld.AppendLine("GROUP BY");
        strBld.AppendLine("A.CODFNC, A.NOMFNC, UPPER(TRIM(B.NOMUSRRCF)) ,");
        strBld.AppendLine("TRIM(A.DESENDCREETNFNC)");
        strBld.AppendLine("ORDER BY A.NOMFNC ASC");

        return strBld.ToString();
    }

    /// <summary>
    /// Obtem os Sistemas do Usuário.
    /// </summary>
    /// <returns></returns>
    /// <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public string ObterSistemasUsuario()
    {
        return @"
            SELECT A.CODGRPRDESISSMA, 
                CASE WHEN B.CODGRPRDESISSMA IS NULL THEN 'INSERIR'
                    ELSE 'NADA' END ACAO
            FROM MRT.CADGRPRDESISSMA A
                LEFT OUTER JOIN MRT.RLCPFLUSRRDESISSMA B
                ON A.CODGRPRDESISSMA = B.CODGRPRDESISSMA
                AND B.DATHRADST IS NULL
            AND B.CODFNC = :CODFNC
                WHERE UPPER(TRIM(DESGRPRDESISSMA)) IN ({0})
                AND B.CODGRPRDESISSMA IS NULL
            UNION
            SELECT A.CODGRPRDESISSMA, 
                'DESATIVAR' ACAO
            FROM MRT.CADGRPRDESISSMA A
                LEFT JOIN MRT.RLCPFLUSRRDESISSMA B
                ON A.CODGRPRDESISSMA = B.CODGRPRDESISSMA
                AND B.DATHRADST IS NOT NULL
                AND B.CODFNC = :CODFNC
            WHERE UPPER(TRIM(DESGRPRDESISSMA)) NOT IN ({0})           
            ORDER BY ACAO, 1
                ";
    }

    /// <summary>
    /// Atualiza os a Relação de Sistemas do Usuário.
    /// </summary>
    /// <returns></returns>
    /// <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public string AtualizarRelacaoSistemasUsuario()
    {
        return @"
            UPDATE MRT.RLCPFLUSRRDESISSMA
            SET DATHRADST = SYSDATE
            WHERE CODFNC = :CODFNC
            AND CODGRPRDESISSMA IN ({0})
            AND DATHRADST IS NULL   
                ";
    }

    /// <summary>
    /// Obtem os Perfis do Grupo de Rede.
    /// </summary>
    /// <returns></returns>
    /// <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public string ObterPerfisPorGrupo()
    {
        return @"
            SELECT CODPFLACS 
            FROM MRT.CADPFLUSRSISSMA
            WHERE CODGRPRDESISSMA = :CODGRPRDESISSMA
                ";
    }

    /// <summary>
    /// Insere uma nova Relação de Sistemas do Usuário.
    /// </summary>
    /// <returns></returns>
    /// <remarks>Leon Denis Paiva e Silva [PrimeTeam]</remarks>
    public string InserirRelacaoSistemaUsuario()
    {
        return @"
              INSERT INTO MRT.RLCPFLUSRRDESISSMA
            (
                CODFNC,
                CODPFLACS,
                CODGRPRDESISSMA,
                CODFNCCAD,
                DATHRACAD,
                CODFNCDST,
                DATHRADST
            ) VALUES (
                :CODFNC,
                :CODPFLACS,
                :CODGRPRDESISSMA,
                NULL,
                SYSDATE,
                NULL,
                NULL
            )
                ";
    }

    /// <summary>
    ///' Atualiza Relação de Sistemas do Usuário já inserido em algum perfil posteriormente.
    /// Para que esse após ser inserido no grupo de rede de algum sistema via SIM/HF
    ///' possa acessar o sistema.
    /// </summary>
    /// <returns></returns>
    /// <remarks>Elton Costa</remarks>
    public string AtualizarRelacaoSistemaUsuario()
    {   
        return @"
            UPDATE MRT.RLCPFLUSRRDESISSMA
               SET CODGRPRDESISSMA = :CODGRPRDESISSMA,
                   DATHRADST       =  NULL
             WHERE CODFNC          = :CODFNC
               AND CODPFLACS       = :CODPFLACS
               AND DATHRADST       =  NULL
               AND (CODGRPRDESISSMA = :CODGRPRDESISSMA1
                OR  CODGRPRDESISSMA = 0)
        ";
    }
}