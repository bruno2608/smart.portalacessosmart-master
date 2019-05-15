// ┌───────────────────────────────────┐ \\
// │ jQuery Select2 Component 1.0.0 │ \\
// ├───────────────────────────────────┤ \\
// │         Michel Oliveira           │ \\
// │      Prime Team Tecnologia        │ \\
// ├───────────────────────────────────┤ \\
// │             Martins               │ \\
// └───────────────────────────────────┘ \\
function ComponenteSelect2() {
    //
    this.modelo = {
        /** ID HTML do container do controle (preferencialmente uma DIV) */
        idContainerControle: '',
        /** ID do controle que será gerado */
        idControle: '',
        /** Número mínimo de caracteres a serem digitados */
        caracteresMinimos: 0,
        /** Texto inicial do controle */
        textoInicial: '',
        /** URL de POST de onde os dados serão buscados */
        ajaxPostApiURL: '',
        /** ID do campo que será pesquisado pela API */
        ajaxIdCampoPesquisaAPI: '',
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
        /** O elemento JSON que será o texto do campo select */
        jsonTexto: '',
        /** O elemento JSON que será o valor do campo select */
        jsonValor: '',
        /** Se é múltiplo ou não */
        multiplo: false,
        /** Se validará a form */
        validarForm: false
    }

    this.inicializar = function (configuracao) {
        var modelo = this.modelo;
        //
        var templateSelect2 = '' +
            '<select    id="' + modelo.idControle + '"' +
            '           name="' + modelo.idControle + '"' +
            '           ' + (modelo.multiplo ? "multiple='multiple'" : "") + '>' +
            '</select>'
        //
        $('#' + modelo.idContainerControle).append(templateSelect2);
        //
        var settings = {
            ajax: {
                url: modelo.ajaxPostApiURL,
                multiple: modelo.multiplo,
                dataType: "json",
                contentType: "application/json",
                type: "POST",
                delay: 500,
                headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
                data: parametrosAjaxData,
                processResults: function (data) {
                    return {
                        results: $.map(data,
                            function (item) {
                                var fncRetJson = Function("item",
                                    "return {" +
                                    "    text: item." +
                                    modelo.jsonTexto +
                                    "," +
                                    "    id: item." +
                                    modelo.jsonValor +
                                    "" +
                                    "}");
                                return fncRetJson(item);
                            })
                    };
                },
                cache: false
            },
            language: "pt-BR",
            width: '100%',
            allowClear: true,
            placeholder: modelo.textoInicial,
            minimumInputLength: modelo.caracteresMinimos
        };
        //
        if (configuracao != null) {
            for (var key in configuracao) {
                Function("settings, value", "settings." + key + " = value;")(settings, configuracao[key]);
            }
        }
        //
        $("#" + modelo.idControle).select2(settings);
        //
        if (modelo.validarForm) {
            $("#" + modelo.idControle).on("select2:select",
                function () {
                    $("#" + modelo.idControle).valid();
                });
        }

        function parametrosAjaxData(params) {
            var obj;
            var arr = [];
            //
            arr[modelo.ajaxIdCampoPesquisaAPI] = (isNullOrEmpty(params.term) ? "" : params.term);
            modelo.idsEnviarRequisicaoAjax.forEach(function (itm, idx) {
                //
                if (itm.tipoJsonEnviar == 'tipoNumerico' ||
                    itm.tipoJsonEnviar == 'tipoTexto') {
                    arr[itm.nomeJsonEnviar] =
                        isNullOrEmpty((obj = $('#' + itm.idJsonEnviar).val())) ? (itm.tipoJsonEnviar == 'tipoNumerico' ? "-1" : "") : obj;
                } else if (itm.tipoJsonEnviar == 'tipoCheckBox') {
                    arr[itm.nomeJsonEnviar] = (($('#' + itm.idJsonEnviar).is(':checked')) ? "1" : "0");
                } else if (itm.tipoJsonEnviar == 'tipoName') {
                    arr[itm.nomeJsonEnviar] = ($("[name='" + itm.idJsonEnviar + "']").val());
                } else if (itm.tipoJsonEnviar == 'tipoFuncao') {
                    arr[itm.nomeJsonEnviar] = itm.idJsonEnviar();
                }
            });
            //
            return JSON.stringify($.extend(true, {}, arr));
        }
    }
}