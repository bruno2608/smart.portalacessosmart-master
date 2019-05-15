// ┌───────────────────────────────────┐ \\
// │ jQuery AJAXFactory Component 1.0.0 │ \\
// ├───────────────────────────────────┤ \\
// │         Michel Oliveira           │ \\
// │      Prime Team Tecnologia        │ \\
// ├───────────────────────────────────┤ \\
// │             Martins               │ \\
// └───────────────────────────────────┘ \\
function ComponenteAJAXFactory() {
    //
    this.modelo = {
        /** URL de POST de onde os dados serão buscados */
        ajaxURL: '',
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
        /** Método de CallBack que será chamado após a execução com sucesso da requisição */
        ajaxSuccessCallbackMethod: function () { },
        /** Se o método será executado assíncronamente ou não */
        async: false,
        enviarRequestVerificationToken: true
    }

    this.inicializar = function () {
        var modelo = this.modelo;
        //
        var obj;
        var arr = [];
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
        var ajaxOptions = {
            async: modelo.async,
            url: modelo.ajaxURL,
            type: "POST",
            data: $.extend(true, {}, arr),
            success: modelo.ajaxSuccessCallbackMethod,
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            }
        }
        if (modelo.enviarRequestVerificationToken) {
            ajaxOptions.headers = { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() };
        }
        $.ajax(ajaxOptions);
    }
}