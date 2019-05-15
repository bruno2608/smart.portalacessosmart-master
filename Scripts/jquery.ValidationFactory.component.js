// ┌───────────────────────────────────┐ \\
// │ jQuery ValidationFactory Component 1.0.0 │ \\
// ├───────────────────────────────────┤ \\
// │         Michel Oliveira           │ \\
// │      Prime Team Tecnologia        │ \\
// ├───────────────────────────────────┤ \\
// │             Martins               │ \\
// └───────────────────────────────────┘ \\
function ComponenteValidationFactory() {
    //
    this.modelo = {
        /** ID da form que será validada */
        idForm: '',
        regras: [],
        /** Regras da form que serão validadas */
        get addregras() {
            var itmPush = this.regras;
            //
            var pushObj = {
                /** Campo name="" a ser validado */
                nomeRegra: '',
                /** Se é obrigatório ou não */
                obrigatorio: false,
                /** Tamanho mínimo */
                tamanhoMinimo: 0,
                /** Tamanho máximo */
                tamanhoMaximo: 0,
                /** Tipo do JSON que será enviado (Padrao tipoTexto) OPÇÕES:tipoSelect,
                                                                            tipoTexto,
                                                                            tipoNumerico,
                                                                            tipoEmail,
                                                                            tipoURL,
                                                                            tipoCartaoDeCredito,
                                                                            tipoDigitos */
                tipoValor: 'tipoTexto',
                mensagem: {
                    /** Mensagem de required */
                    required: '',
                    /** Tamanho mínimo para mostrar a mensagem */
                    minlength: '',
                    /** Tamanho máximo */
                    maxlength: '',
                    /** Tamanho mínimo (campos dinâmicos) */
                    min: '',
                    /** Tamanho máximo (campos dinâmicos) */
                    max: ''
                }
            }
            //
            itmPush.push(pushObj);
            return pushObj;
        }
    }

    this.inicializar = function () {
        var modelo = this.modelo;
        //
        var msgs = {};
        var rles = {};
        //
        modelo.regras.forEach(function (regra, idx) {
            if (regra.mensagem.required !== '') {
                //
                var funcGenJson = Function(
                    "msgs",
                    "msgs." + regra.nomeRegra + " = { }; " +
                    "return msgs." + regra.nomeRegra + ";");
                var itmProp = funcGenJson(msgs);
                //
                if (regra.mensagem.required !== '')
                    itmProp.required = regra.mensagem.required;
                if (regra.mensagem.minlength !== '')
                    itmProp.minlength = regra.mensagem.minlength;
                if (regra.mensagem.maxlength !== '')
                    itmProp.maxlength = regra.mensagem.maxlength;
                if (regra.mensagem.min !== '')
                    itmProp.min = regra.mensagem.min;
                if (regra.mensagem.max !== '')
                    itmProp.max = regra.mensagem.max;
                //
            }
            //
            var funcGenJson = Function(
                "rles",
                "rles." + regra.nomeRegra + " = { }; " +
                "return rles." + regra.nomeRegra + ";");
            var itmProp = funcGenJson(rles);
            //
            itmProp.required = regra.obrigatorio;
            itmProp.minlength = regra.tamanhoMinimo;
            itmProp.maxlength = regra.tamanhoMaximo;
            //
            if (regra.tipoValor == 'tipoSelect') {
                itmProp.min = regra.tamanhoMinimo;
                itmProp.max = regra.tamanhoMaximo;
            }
            if (regra.tipoValor == 'tipoNumerico')
                itmProp.number = true;
            if (regra.tipoValor == 'tipoEmail')
                itmProp.email = true;
            if (regra.tipoValor == 'tipoURL')
                itmProp.url = true;
            if (regra.tipoValor == 'tipoCartaoDeCredito')
                itmProp.creditcard = true;
            if (regra.tipoValor == 'tipoDigitos')
                itmProp.digits = true;
            //
        });
        //
        $("#" + modelo.idForm).validate({
            errorElement: "span",
            errorClass: "help-block help-block-error",
            focusInvalid: true,
            ignore: "",
            messages: msgs,
            rules: rles,
            errorPlacement: function (e, r) {
                var i = $(r).parent(".input-group");
                i.size() > 0 ? i.after(e) : r.after(e);
            },
            highlight: function (e) {
                $(e).closest(".form-group").addClass("has-error");
            },
            unhighlight: function (e) {
                $(e).closest(".form-group").removeClass("has-error");
            },
            success: function (e) {
                $(e).closest(".form-group").removeClass("has-error");
            },
            submitHandler: function (e) {
            }
        });
    }
}