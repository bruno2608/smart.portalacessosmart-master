// ┌───────────────────────────────────┐ \\
// │ jQuery ValueBinder Component 1.0.0 │ \\
// ├───────────────────────────────────┤ \\
// │         Michel Oliveira           │ \\
// │      Prime Team Tecnologia        │ \\
// ├───────────────────────────────────┤ \\
// │             Martins               │ \\
// └───────────────────────────────────┘ \\
function ComponenteValueBinder() {
    //
    this.modelo = {
        /** URL de POST de onde os dados serão buscados */
        ajaxPostURL: '',
        idsEnviarRequisicaoAjax: [],
        /** Array com os IDs dos elementos que serão enviados na requisição */
        get addidsEnviarRequisicaoAjax() {
            var itmPush = this.idsEnviarRequisicaoAjax;
            //
            var pushObj = {
                /** Nome (key) do JSON que será enviado */
                nomeJsonEnviar: '',
                /** Tipo do JSON que será enviado (Padrao tipoTexto) OPÇÕES: tipoTexto,
                                                                             tipoNumerico,
                                                                             tipoCheckBox,
                                                                             tipoParametro,
                                                                             tipoName,
                                                                             tipoFuncao */
                tipoJsonEnviar: '',
                /** ID do elemento em que será extraído o valor utilizando o método val() do elemento jQuery */
                idJsonEnviar: ''
            };
            //
            itmPush.push(pushObj);
            return pushObj;
        },
        idsSetarRequisicaoAjax: [],
        /** Itens que serão setados ao realizar a requisição ajax */
        get addidsSetarRequisicaoAjax() {
            var itmPush = this.idsSetarRequisicaoAjax;
            //
            var pushObj = {
                /** Nome do JSON a ser setado */
                nomeJsonSetar: '',
                /** Texto do JSON a ser setado */
                textoJsonSetar: '',
                /** Tipo do JSON a ser setado */
                tipoJsonSetar: '',
                /** ID do item a ser setado, padrão: tipoTexto, possibilidades:tipoTexto,
                                                                tipoNumerico,
                                                                tipoSelect2 */
                idJsonSetar: 'tipoTexto'
            };
            //
            itmPush.push(pushObj);
            return pushObj;
        }
    }

    this.inicializar = function () {
        var model = this.modelo;
        //
        with (new ComponenteAJAXFactory()) {
            with (modelo) {
                ajaxSuccessCallbackMethod = function (data) {
                    model.idsSetarRequisicaoAjax.forEach(function (itm, idx) {
                        //
                        if (itm.tipoJsonSetar == 'tipoNumerico' ||
                            itm.tipoJsonSetar == 'tipoTexto') {
                            var vl = Function("data", "return data." + itm.nomeJsonSetar + ";")(data);
                            $('#' + itm.idJsonSetar).val(vl);
                        } else if (itm.tipoJsonSetar == 'tipoHtml') {
                            var vl = Function("data", "return data." + itm.nomeJsonSetar + ";")(data);
                            $('#' + itm.idJsonSetar).html(vl);
                        } else if (itm.tipoJsonSetar == 'tipoSelect2Array') {
                            //
                            $(data).each(function (idx, itmdata) {
                                var vlNome = Function("data", "return data." + itm.nomeJsonSetar + ";")(itmdata);
                                var vlText = Function("data", "return data." + itm.textoJsonSetar + ";")(itmdata);
                                //
                                $('#' + itm.idJsonSetar).select2("trigger",
                                    "select",
                                    {
                                        data: {
                                            id: vlNome,
                                            text: vlText
                                        }
                                    });
                            });
                        }
                    });
                };
                ajaxURL = model.ajaxPostURL;
                async = true;
                idsEnviarRequisicaoAjax = model.idsEnviarRequisicaoAjax;
            }
            inicializar(null);
        }
    }
}