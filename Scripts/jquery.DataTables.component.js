// ┌───────────────────────────────────┐ \\
// │ jQuery DataTables Component 1.0.0 │ \\
// ├───────────────────────────────────┤ \\
// │         Michel Oliveira           │ \\
// │      Prime Team Tecnologia        │ \\
// ├───────────────────────────────────┤ \\
// │             Martins               │ \\
// └───────────────────────────────────┘ \\
function ComponenteDataTables() {
    //
    var funcaoVazia = function () { };
    var numKeys = function (o) {
        var res = 0;
        for (var k in o) {
            if (o.hasOwnProperty(k)) res++;
        }
        return res;
    }
    this.modelo = {
        /** ID HTML do container do controle (preferencialmente uma DIV) */
        idContainerControle: '',
        /** ID do controle que será gerado */
        idControle: '',
        /** Se o controle será carregado inicialmente */
        carregarInicialmente: false,
        /** Se haverá filtro por colunas */
        naoFiltrarPorColuna: false,
        /** Se haverá agrupamento por colunas */
        agruparPorColunas: false,
        /** URL de POST de onde os dados serão buscados */
        ajaxPostURL: '',
        /** Função que será chamada antes de carregar o componente */
        funcaoChamarAntes: function () { },
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
                tipoJsonEnviar: 'tipoTexto',
                /** ID do elemento em que será extraído o valor utilizando o método val() do elemento jQuery */
                idJsonEnviar: ''
            };
            //
            itmPush.push(pushObj);
            return pushObj;
        },
        /** Função a ser chamada no callback de Draw */
        fnDrawCallback: function () { },
        /** Se haverá quebra de linha ou não */
        noWrap: false,
        /** Se será modelado os atributos das colunas ou não */
        naoModelarAtributosColunas: false,
        botoesGrid: [],
        /** Adiciona botões no grid */
        get addbotoesGrid() {
            var itmPush = this.botoesGrid;
            //
            var pushObj = {
                /** Se o botão será oculto */
                botaoOculto: false,
                /** Classe do botão */
                classBotao: '',
                /** Ícone do botão (classe CSS que representa-o) */
                iconeBotao: '',
                /** Se haverá uma coluna de condição que mostrará ou não o botão no grid */
                colunaCondicaoMostrar: '',
                /** Se a direção (mostrar ou não mostrar) será inversamente aplicada ou não */
                direcaoColunaCondicaoMostrar: false,
                /** Função a ser chamada na montagem do botão */
                funcaoChamar: function () { },
                /** Função a ser chamada antes da montagem do botão */
                funcaoChamarAntes: function () { },
                /** Função a ser chamada depois da montagem do botão */
                funcaoChamarDepois: function () { },
                itensBotaoSetar: [],
                /** Ao acionar o botão, sertará os valores do GRID nos campos informados */
                get additensBotaoSetar() {
                    var itmPush = this.itensBotaoSetar;
                    //
                    var pushObj = {
                        /** ID do item a ser setado */
                        idItem: '',
                        /** ID da coluna que será setada no item */
                        idColuna: '',
                        /** Coluna texto em que será setado o elemento (Válido para o tipo Select) */
                        textoColuna: '',
                        /** ID do item a ser setado, padrão: tipoTexto, possibilidades:tipoTexto,
                                                                        tipoNumerico,
                                                                        tipoSelect2 */
                        tipoItemSetar: 'tipoTexto'
                    }
                    //
                    itmPush.push(pushObj);
                    return pushObj;
                },
                modal: {
                    /** Nome do modal em que o botão abrirá */
                    nomeModal: '',
                    itensModalSetar: [],
                    /** Itens do modal que serão setados ao clicar no botão */
                    get additensModalSetar() {
                        var itmPush = this.itensModalSetar;
                        //
                        var pushObj = {
                            /** ID do item a ser setado */
                            idItem: '',
                            /** ID da coluna que será setada no item */
                            idColuna: '',
                            /** Coluna texto em que será setado o elemento (Válido para o tipo Select) */
                            textoColuna: '',
                            /** Tipo do item a ser setado, valor padrão: tipoTexto, possíveis:  tipoTexto,
                                                                                                tipoNumerico,
                                                                                                tipoSelect2 */
                            tipoItemSetar: 'tipoTexto'
                        }
                        //
                        itmPush.push(pushObj);
                        return pushObj;
                    },
                    botoes: [],
                    get addbotoes() {
                        var itmPush = this.botoes;
                        //
                        var pushObj = {
                            /** ID do botão que estará dentro do modal */
                            idBotao: '',
                            /** Form que será validada ao clicar no botão, e se estiver validada, dispara o AJAX */
                            idFormValidar: '',
                            /** AJAX que será disparado ao clicar no botão */
                            ajax: function () { }
                        }
                        //
                        itmPush.push(pushObj);
                        return pushObj;
                    }
                },
                /** AJAX que será disparado ao clicar no botão */
                ajax: function () { },
                /** Nome da coluna que será gerado o cabeçalho */
                cabecalho: '',
                /** Estilo CSS que será setado ao cabeçalho */
                cabecalhoStyle: '',
                /** Classe CSS que será setado ao cabeçalho */
                cabecalhoClass: '',
                /** Coluna rodapé que será gerada */
                rodape: '',
                /** Estilo CSS que será setado no rodapé */
                rodapeStyle: '',
                /** Classe CSS que será setado no rodapé */
                rodapeClass: ''
            };
            //
            itmPush.push(pushObj);
            return pushObj;
        },
        Colunas: [],
        /** Adiciona colunas no grid */
        get addColunas() {
            var itmPush = this.Colunas;
            //
            var pushObj = {
                /** Nome da coluna */
                nomeColuna: '',
                /** Classe da coluna */
                classeColuna: '',
                /** Cabeçalho da coluna */
                cabecalho: '',
                /** Estilo do cabeçalho da coluna */
                cabecalhoStyle: '',
                /** Classe do cabeçalho da coluna */
                cabecalhoClass: '',
                /** Rodapé da coluna */
                rodape: '',
                /** Estilo do rodapé da coluna */
                rodapeStyle: '',
                /** Classe do rodapé da coluna */
                rodapeClass: '',
                /** Retorno do callback render a ser executado */
                retornoRender: function () { }
            };
            //
            itmPush.push(pushObj);
            return pushObj;
        },
        DefinicoesColunas: [],
        /** Adiciona as definições das colunas do grid */
        get addDefinicoesColunas() {
            var itmPush = this.DefinicoesColunas;
            //
            var pushObj = {
                /** Nome da classe a ser definida pela regra */
                nomeClasse: '',
                /** Colunas destino para a regra */
                colunasTargets: [],
                /** Retorno do callback render a ser regrado */
                retornoRender: function () { }
            };
            //
            itmPush.push(pushObj);
            return pushObj;
        },
        Botoes: [],
        /** Adiciona botões no grid */
        get addBotoes() {
            var itmPush = this.Botoes;
            //
            var pushObj = {
                /** Nome do botão que será inserido no grid */
                nomeBotao: '',
                /** Nome da classe que será inserida no botão */
                nomeClasse: '',
                /** Texto do botão */
                texto: '',
                /** Função para ser chamada ao clicar no botão */
                funcaoChamar: function () { },
                modal: {
                    /** Nome do modal para ser aberto ao clicar no botão */
                    nomeModal: '',
                    botoes: [],
                    /** Adiciona ação aos botões que estão dentro do modal */
                    get addbotoes() {
                        var itmPush = this.botoes;
                        //
                        var pushObj = {
                            /** ID do botão que estará dentro do modal */
                            idBotao: '',
                            /** ID da form a ser validada do clicar no botão */
                            idFormValidar: '',
                            /** Tipo do botão, valor padrão: tipoAjax, possibilidades:  tipoAjax,
                                                                                        tipoPesquisar,
                                                                                        tipoLimpar */
                            tipo: 'tipoAjax',
                            funcaoChamar: function () { },
                            ajax: function () { }
                        };
                        //
                        itmPush.push(pushObj);
                        return pushObj;
                    }
                }
            };
            //
            itmPush.push(pushObj);
            return pushObj;
        }
    }

    this.inicializar = function (configuracao) {
        var modelo = this.modelo;
        //
        var templateDataTables = '' +
            '<div class="portlet light ">\
                <div class="portlet-body">\
                    <table class="table table-striped table-bordered table-hover display compact " id="' +
            modelo.idControle +
            '">\
                        <thead>\
                            <tr>\
                            </tr>\
                        </thead>\
                    </table>\
                </div>\
            </div>';
        //
        $('#' + modelo.idContainerControle).append(templateDataTables);
        var addRodape = false;
        //
        modelo.Colunas.forEach(function (itm, idx) {
            if (itm.rodape !== '') {
                addRodape = true;
            }
        });
        //
        if (addRodape) {
            $('#' + modelo.idControle).append("" +
                "<tfoot>" +
                "<tr>" +
                "</tr>" +
                "</tfoot>");
        }
        //
        modelo.Colunas.forEach(function (itm, idx) {
            if (itm.botaoOculto) {
                return;
            }
            $('#' + modelo.idControle + ">thead>tr").append('<th><div style="' +
                itm.cabecalhoStyle +
                '" class="' +
                itm.cabecalhoClass +
                '">' +
                itm.cabecalho +
                '</div></th>');
            //
            if (addRodape) {
                $('#' + modelo.idControle + ">tfoot>tr").append('<th><div style="' +
                    itm.rodapeStyle +
                    '" class="' +
                    itm.rodapeClass +
                    '">' +
                    itm.rodape +
                    '</div></th>');
            }
        });

        modelo.botoesGrid.forEach(function (itm, idx) {
            $('#' + modelo.idControle + ">thead>tr").append('<th><div style="' +
                itm.cabecalhoStyle +
                '" class="' +
                itm.cabecalhoClass +
                '">' +
                itm.cabecalho +
                '</div></th>');
            //
            if (addRodape) {
                $('#' + modelo.idControle + ">tfoot>tr").append('<th><div style="' +
                    itm.rodapeStyle +
                    '" class="' +
                    itm.rodapeClass +
                    '">' +
                    itm.rodape +
                    '</div></th>');
            }
        });
        var colFix = 2;
        var table = $('#' + modelo.idControle);
        var oTable = null;
        //
        var colIdx = -1;
        var dataTablesFirstLoad = true;

        var dataTablesDrawn = true;
        var isGerenciandoAgrupamento = false;
        //
        var settingsDataTables = {
            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "ajax": {
                beforeSend: function (jqXhr) {
                    dataTablesDrawn = false;
                    if (dataTablesFirstLoad) {
                        jqXhr.abort();
                    }
                },
                "url": modelo.ajaxPostURL,
                "dataSrc": "",
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json",
                "data": parametrosGridAjaxData,
                "headers": { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
                "error": function (xhr, status, error) {
                    alert(xhr.responseText);
                }
            },
            "aoColumns": obterAoColumns(),
            "columnDefs": obterColumnDefs(),

            "language": {
                "sEmptyTable": "Nenhum registro encontrado",
                "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
                "sInfoFiltered": "(Filtrados de _MAX_ registros)",
                "sInfoPostFix": "",
                "sInfoThousands": ".",
                "sLengthMenu": "_MENU_ resultados por página",
                "sLoadingRecords": "Carregando...",
                "sProcessing": "Processando...",
                "sZeroRecords": "Nenhum registro encontrado",
                "sSearch": "",
                "oPaginate": {
                    "sNext": "Próximo",
                    "sPrevious": "Anterior",
                    "sFirst": "Primeiro",
                    "sLast": "Último"
                },
                "oAria": {
                    "sSortAscending": ": Ordenar colunas de forma ascendente",
                    "sSortDescending": ": Ordenar colunas de forma descendente"
                }
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: obterDataTableButtons(),

            // setup responsive extension: http://datatables.net/extensions/responsive/
            serverSide: false,
            responsive: false,
            scrollX: true,
            scrollY: '50vh',
            scrollCollapse: false,
            //"ordering": false, disable column ordering
            //"paging": false, disable pagination
            colReorder: {
                realtime: false
            },
            scroller: false,
            paging: true,
            keys: false,
            "order": [],
            //"destroy": true,
            "processing": false,
            "bAutoWidth": false,
            "bDeferRender": true,
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $('td', nRow).attr('nowrap', 'nowrap');
                return nRow;
            },
            "fnDrawCallback": obterDrawCallback,
            "lengthMenu": [
                [10, 25, 50, 100, 500],
                [10, 25, 50, 100, 500] // change per page values here
            ],
            // set the initial value
            "pageLength": 25,

            "dom": '<"fg-toolbar ui-toolbar ui-widget-header ui-helper-clearfix ui-corner-tl ui-corner-tr"Blr>' +
                't' +
                '<"fg-toolbar ui-toolbar ui-widget-header ui-helper-clearfix ui-corner-bl ui-corner-br"ip>',
        };
        //
        if (configuracao != null) {
            for (var key in configuracao) {
                Function("settings, value", "settings." + key + " = value;")(settingsDataTables, configuracao[key]);
            }
        }
        //
        modelo.funcaoChamarAntes();
        montarDataTables();

        function montarDataTables() {
            oTable = table.DataTable(settingsDataTables);
            table.dataTable().fnAdjustColumnSizing();

            oTable.on('page.dt',
                function() {
                    setTimeout(function () {
                            obterDrawCallback(null, null);
                        },
                        0);
                }).on('order.dt',
                function() {
                    setTimeout(function () {
                            obterDrawCallback(null, null);
                        },
                        0);
                }).on('length.dt',
                function(e, settings, len) {
                    setTimeout(function () {
                            obterDrawCallback(null, null);
                        },
                        0);
                });

            if (modelo.agruparPorColunas) {
                oTable.on('page.dt',
                    function () {
                        setTimeout(function () {
                            gerenciaAgrupamentoColunas();
                        },
                            0);
                    }).on('order.dt',
                    function () {
                        setTimeout(function () {
                            gerenciaAgrupamentoColunas();
                        },
                            0);
                    }).on('length.dt',
                    function (e, settings, len) {
                        setTimeout(function () {
                            gerenciaAgrupamentoColunas();
                        },
                            0);
                    });
            }

            modelo.Botoes.forEach(function (btn, idx) {
                //
                if (btn.modal.nomeModal !== '') {
                    $('.' + btn.nomeBotao).unbind();
                    $('.' + btn.nomeBotao).click(function () {
                        $('#' + btn.modal.nomeModal).modal();
                    });
                    btn.modal.botoes.forEach(function (btnModal, idx) {
                        if (btnModal.tipo !== 'tipoAjax') {
                            $('#' + btnModal.idBotao).unbind();
                            $('#' + btnModal.idBotao).click(function () {
                                if (btnModal.tipo == 'tipoPesquisar') {
                                    if (btnModal.idFormValidar !== '') {
                                        if (!$("#" + btnModal.idFormValidar).valid()) {
                                            return;
                                        }
                                    }

                                    btnModal.funcaoChamar();
                                    table.DataTable().ajax.reload();
                                    $('#' + btn.modal.nomeModal).modal('hide');
                                } else if (btnModal.tipo == 'tipoLimpar') {
                                    limparTodosOsCampos(btn.modal.nomeModal);
                                }
                            });
                        }
                    });
                }
            });
            modelo.botoesGrid.forEach(function (btn, idx) {
                //
                if (btn.modal.nomeModal !== '') {
                    btn.modal.botoes.forEach(function (btnGrd, idx) {
                        //
                        if (btnGrd.ajax !== funcaoVazia.toString()) {
                            $('#' + btnGrd.idBotao).unbind();
                            $('#' + btnGrd.idBotao).click(function () {
                                if (btnGrd.idFormValidar !== '') {
                                    if (!$("#" + btnGrd.idFormValidar).valid()) {
                                        return;
                                    }
                                }
                                btnGrd.ajax();
                                $('#' + btn.modal.nomeModal).modal('hide');
                            });
                        }
                    });
                }
            });
            modelo.Botoes.forEach(function (btn, idx) {
                //
                if (btn.modal.nomeModal !== '') {
                    btn.modal.botoes.forEach(function (btnGrd, idx) {
                        //
                        if (btnGrd.tipo === 'tipoAjax') {
                            $('#' + btnGrd.idBotao).unbind();
                            $('#' + btnGrd.idBotao).click(function () {
                                if (btnGrd.idFormValidar !== '') {
                                    if (!$("#" + btnGrd.idFormValidar).valid()) {
                                        return;
                                    }
                                }
                                btnGrd.ajax();
                                //@string.Concat(ajFactoryGenBtnModal[btnGrd].nomeMetodo, "();")
                                $('#' + btn.modal.nomeModal).modal('hide');
                            });
                        }
                    });
                } else {
                    if (btn.funcaoChamar.toString() !== funcaoVazia.toString()) {
                        $('.' + btn.nomeBotao).unbind();
                        $('.' + btn.nomeBotao).click(function () {
                            btn.funcaoChamar();
                        });
                    }
                }
            });
        }

        if (modelo.carregarInicialmente) {
            setTimeout(function () {
                oTable.ajax.reload();
            },
                0);
        }


        function gerenciaExportacao(strApi) {
            var dtt = oTable;
            if (!oTable.rows().data().length > 0) {
                showAlert('', 'Não há dados para exportar.', '');
                return;
            }
            var arrDataTables = dtt.ajax.json();
            var arr = [];
            var dttOrder = dtt.order();
            $('#' + modelo.idControle + ' > tbody[class!="grupo"] > tr:eq(0)').each(function () {
                $(this).find('td').each(function () {
                    var dttCol = dtt.column($(this));
                    var order = false;
                    var orderDirection = '';
                    if (dttOrder.length > 0 && dttOrder[0].length > 0) {
                        orderDirection = dttOrder[0][1];
                        if (orderDirection != undefined) {
                            order = (dttCol.index() === dttOrder[0][0]);
                        }
                    }
                    arr.push({
                        dataSrc: dttCol.dataSrc(),
                        visible: dttCol.visible().toString(),
                        order: (order ? orderDirection : ""),
                        header: $(dttCol.header()).text()
                    });
                });
                return;
            });
            strApi = diretorioAplicacao + strApi;
            $.ajax({
                async: true,
                url: strApi,
                type: "POST",
                headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
                data: JSON.stringify({ ajax: arrDataTables, columns: arr }),
                success: function (data) {
                    $(location).attr('href', data);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                }
            });
        }

        function parametrosGridAjaxData() {
            var obj;
            var arr = [];
            //foreach(var itm in modelo.idsEnviarRequisicaoAjax)
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
            return JSON.stringify($.extend(true, {}, arr));
        }

        function obterAoColumns() {
            var ret = [];
            if (modelo.naoModelarAtributosColunas) {
                return null;
            }
            else {
                modelo.Colunas.forEach(function (col, idx) {
                    var itmPush = {};
                    //
                    if (col.classeColuna !== '')
                        itmPush.sClass = col.classeColuna;
                    if (col.nomeColuna !== '')
                        itmPush.data = col.nomeColuna;
                    if (col.retornoRender.toString() !== funcaoVazia.toString())
                        itmPush.mRender = col.retornoRender;
                    //
                    ret.push(itmPush);
                });
                modelo.botoesGrid.forEach(function (btn, idx) {
                    if (btn.botaoOculto) {
                        return;
                    }
                    //
                    ret.push({
                        mRender: function (data, type, row) {
                            var ret = true;
                            if (btn.colunaCondicaoMostrar !== '') {
                                var nullOrEmp = Function("row", 'return isNullOrEmpty(row.' + btn.colunaCondicaoMostrar + ');')(row);
                                if ((!nullOrEmp && !btn.direcaoColunaCondicaoMostrar) || (nullOrEmp && btn.direcaoColunaCondicaoMostrar)) {
                                    ret = false;
                                }
                            }
                            if (ret) {
                                return '<div class="' + btn.classBotao + '"><i class="' + btn.iconeBotao + '"></i></div>';
                            }
                            else { return ''; }
                        },
                        "orderable": false,
                        "sClass": "text-center botoesGrid",
                        "sWidth": "1px"
                    });
                });
            }
            return ret;
        }

        function obterColumnDefs() {
            var ret = [];
            modelo.DefinicoesColunas.forEach(function (itm, idx) {
                var itmPush = {};
                //
                if (itm.colunasTargets.length > 0)
                    itmPush.targets = itm.colunasTargets;
                if (itm.nomeClasse !== '')
                    itmPush.className = itm.nomeClasse;
                if (itm.retornoRender.toString() !== funcaoVazia.toString())
                    itmPush.render = itm.retornoRender;
                //
                if (numKeys(itmPush) > 0)
                    ret.push(itmPush);
            });
            if (ret.length === 0) {
                return null;
            } else {
                return ret;
            };
        }


        function obterDrawCallback(settings, json) {
            if (!dataTablesDrawn || (settings == null && json == null)) {
                var thisGridApi = oTable;
                modelo.botoesGrid.forEach(function (btn, idx) {
                    //
                    if (btn.funcaoChamar.toString() === funcaoVazia.toString()) {
                        $('.' + btn.classBotao).unbind();
                        $('.' + btn.classBotao).click(function () {
                            btn.funcaoChamarAntes();
                            var data = thisGridApi.row($(this).parents('tr')).data();
                            btn.itensBotaoSetar.forEach(function (itm, idx) {
                                var idColuna = Function("data", "return data." + itm.idColuna + ";")(data);
                                if (itm.tipoItemSetar === 'tipoNumerico' || itm.tipoItemSetar === 'tipoTexto') {
                                    $('#' + itm.idItem).val(idColuna);
                                }
                                else {
                                    if (itm.tipoItemSetar == 'tipoSelect2') {
                                        var textoColuna = Function("data", "return data." + itm.textoColuna + ";")(data);
                                        $('#' + itm.idItem).select2("trigger",
                                            "select",
                                            {
                                                data: { id: idColuna, text: textoColuna }
                                            });
                                    }
                                }
                                btn.funcaoChamarDepois();
                            });
                        });
                        if (btn.ajax.toString() !== funcaoVazia.toString()) {
                            $('.' + btn.classBotao).unbind();
                            $('.' + btn.classBotao).click(function () {
                                btn.funcaoChamarAntes();
                                var data = thisGridApi.row($(this).parents('tr')).data();
                                btn.ajax(data);
                                btn.funcaoChamarDepois();
                            });
                        }
                        if (btn.modal.nomeModal !== '') {
                            $('.' + btn.classBotao).unbind();
                            $('.' + btn.classBotao).click(function () {
                                btn.funcaoChamarAntes();
                                var data = thisGridApi.row($(this).parents('tr')).data();
                                btn.modal.itensModalSetar.forEach(function (itm, idx) {
                                    var idColuna = Function("data", "return data." + itm.idColuna + ";")(data);
                                    //
                                    if (itm.tipoItemSetar == 'tipoNumerico' || itm.tipoItemSetar == 'tipoTexto') {
                                        $('#' + itm.idItem).val(idColuna);
                                    } else {
                                        if (itm.tipoItemSetar == 'tipoSelect2') {
                                            var textoColuna = Function("data", "return data." + itm.textoColuna + ";")(data);
                                            $('#' + itm.idItem).select2("trigger",
                                                "select",
                                                {
                                                    data: { id: idColuna, text: textoColuna }
                                                });
                                        }
                                    }
                                });
                                btn.funcaoChamarDepois();
                                $('#' + btn.modal.nomeModal).modal();
                            });
                        }
                    } else {
                        $('.' + btn.classBotao).unbind();
                        $('.' + btn.classBotao).click(function () {
                            btn.funcaoChamarAntes();
                            btn.funcaoChamar();
                            btn.funcaoChamarDepois();
                        });
                    }
                });

                if (!modelo.naoFiltrarPorColuna) {
                    thisGridApi.columns().every(function (idx) {
                        var that = this;
                        var theadBody = $(this.header()).closest('thead').next();
                        var theInput = theadBody.find('input:eq(' + idx + ')');
                        theInput.unbind();
                        theInput.on('keyup change',
                            function () {
                                if (that.search() !== this.value) {
                                    that.search(this.value).draw();
                                }
                            });
                    });
                }
                modelo.fnDrawCallback(this, settings, json);
                dataTablesFirstLoad = false;
                dataTablesDrawn = true;
                thisGridApi.columns.adjust();
            }
        }

        function obterDataTableButtons() {
            var msgAgrp =
                "O recurso de agrupamento de linhas somente está disponível na versão paginada.<br /><br /><br />Clique em Ferramentas > Paginação > Ativar para utilizar o recurso.";

            var buttonsOcultar = [];
            modelo.Colunas.forEach(function (itm, idx) {
                buttonsOcultar.push({
                    className: "",
                    text: itm.cabecalho,
                    action: function (e, dt, node, config) {
                        dt.column($(node).index()).every(function () {
                            this.visible(!this.visible(), false);
                        });
                        colIdx = -1;
                        dt.draw("page");
                    }
                });
            });

            var botoesGrid = [];
            modelo.Botoes.forEach(function (itm, idx) {
                botoesGrid.push({
                    className: itm.nomeBotao + ' ' + itm.nomeClasse,
                    text: itm.texto
                });
            });

            return botoesGrid.concat([
                {
                    extend: "collection",
                    autoClose: true,
                    text: '<i class="fa fa-cog"></i> Ferramentas',
                    buttons: [
                        {
                            extend: "collection",
                            text: '<i class="fa fa-toggle-on"></i> Paginação',
                            autoClose: true,
                            buttons: [
                                {
                                    className: "",
                                    text: '<i class="fa fa-check-square-o"></i> Ativar',
                                    action: function (e, dt, node, config) {
                                        if (settingsDataTables.scroller) {
                                            reloadDataTables(false)
                                        } else {
                                            showAlert("", "A paginação já se encontra ativada.", "")
                                        }
                                    }
                                }, {
                                    className: "",
                                    text: "<i class='fa fa-square-o'></i> Desativar",
                                    action: function (e, dt, node, config) {
                                        if (!settingsDataTables.scroller) {
                                            reloadDataTables(true)
                                        } else {
                                            showAlert("", "A paginação já se encontra desativada.", "")
                                        }
                                    }
                                }
                            ]
                        }, {
                            extend: "collection",
                            text: '<i class="fa fa-file"></i> Exportar',
                            autoClose: true,
                            buttons: [
                                {
                                    className: "",
                                    text: '<i class="fa fa-file-pdf-o"></i> PDF',
                                    action: function (e, dt, node, config) {
                                        gerenciaExportacao("api/Exportar/ExportarPDF")
                                    }
                                }, {
                                    className: "",
                                    text: "<i class='fa fa-file-excel-o'></i> Excel",
                                    action: function (e, dt, node, config) {
                                        gerenciaExportacao("api/Exportar/ExportarExcel")
                                    }
                                }, {
                                    className: "",
                                    text: "<i class='fa fa-file-excel-o'></i> Excel 2003",
                                    action: function (e, dt, node, config) {
                                        gerenciaExportacao("api/Exportar/ExportarExcel2003")
                                    }
                                }, {
                                    className: "",
                                    text: "<i class='fa fa-file-word-o'></i> Word",
                                    action: function (e, dt, node, config) {
                                        gerenciaExportacao("api/Exportar/ExportarWord")
                                    }
                                }, {
                                    className: "",
                                    text: "<i class='fa fa-file-word-o'></i> Word 2003",
                                    action: function (e, dt, node, config) {
                                        gerenciaExportacao("api/Exportar/ExportarWord2003")
                                    }
                                }, {
                                    className: "",
                                    text: "<i class='fa fa-picture-o'></i> Imagem",
                                    action: function (e, dt, node, config) {
                                        gerenciaExportacao("api/Exportar/ExportarImagem")
                                    }
                                }
                            ]
                        }, {
                            extend: "collection",
                            text: '<i class="fa fa-columns"></i> Ocultar',
                            autoClose: false,
                            buttons: buttonsOcultar
                        }, {
                            className: "",
                            text: "<i class='fa fa-undo'></i> Restaurar",
                            action: function (e, dt, node, config) {
                                dt.columns().every(function () {
                                    if (!this.visible()) {
                                        this.visible(true, false);
                                    }
                                });
                                oTable.colReorder.reset();
                                colIdx = -1;
                                dt.draw("page");
                            }
                        }
                    ]
                }
            ]);
        }

        function reloadDataTables(scroller) {
            var sTimeOut = false;
            if (oTable.rows().data().length > 0) {
                sTimeOut = true;
            }
            colIdx = -1;
            if (modelo.agruparPorColunas) {
                gerenciaAgrupamentoColunas();
            }
            dataTablesFirstLoad = true;
            dataTablesDrawn = true;
            settingsDataTables.scroller = scroller;
            oTable.colReorder.reset();
            oTable.clear();
            oTable.destroy();
            table.unbind();
            table.find("*").unbind();
            montarDataTables();
            if (sTimeOut) {
                setTimeout(function () {
                    oTable.ajax.reload();
                },
                    0);
            }
        }

        function gerenciaAgrupamentoColunas() {
            if (dataTablesDrawn && !isGerenciandoAgrupamento) {
                isGerenciandoAgrupamento = true;
                //
                var api = oTable;
                $('#' + modelo.idControle + ' .grupo').remove();
                $('#' + modelo.idControle + ' .grupotbody').remove();
                if (colIdx > -1) {
                    oTable.colReorder.reset();
                    var groupArr = [];
                    var col = api.column(colIdx, { page: 'current' });
                    var rows = api.rows({ page: 'current' });
                    var colIdxVisible = col.index('visible');
                    //
                    rows.every(function () {
                        var cell = $(this.node()).find('td:eq(' + colIdxVisible + ')');
                        var group = cell.text();
                        if (group === undefined || group === null || group === "") {
                            cell.text('-');
                        }
                    });
                    rows.every(function () {
                        var cell = $(this.node()).find('td:eq(' + colIdxVisible + ')');
                        var group = cell.text();
                        if (groupArr.indexOf(group) === -1) {
                            groupArr.push(group);
                            var groupHeader =
                                $(
                                    '<tbody class="grupo">' +
                                    '	<tr role="button">' +
                                    '		<td colspan="' +
                                    modelo.Colunas.length +
                                    '" class="ui-widget-header">' +
                                    '			<span class="caret"></span> ' +
                                    '			<strong>' +
                                    $(col.header()).text() +
                                    ': ' +
                                    group +
                                    '			</strong>' +
                                    '		</td>' +
                                    '	</tr>' +
                                    '</tbody>');
                            var allGroup =
                                $('#' +
                                        modelo.idControle +
                                        ' > tbody[class!="grupo"][class!="grupotbody"]:eq(0) > tr > td')
                                    .filter(function (index) {
                                        return $(this).text().trim() === group.trim();
                                    }).parent();
                            var allGroupParent = allGroup.parent();
                            var allGrpDetach = allGroup.detach();
                            if (allGroupParent.find('tr').size() === 0) {
                                allGrpDetach.appendTo(allGroupParent);
                                allGrpDetach.parent().before(groupHeader);
                            } else {
                                var newtBody = $('<tbody class="grupotbody">');
                                allGrpDetach.appendTo(newtBody);
                                newtBody.prependTo(table);
                                newtBody.before(groupHeader);
                            }
                        }
                    });
                    $('#' + modelo.idControle + ' .grupo').unbind();
                    $('#' + modelo.idControle + ' .grupo').click(function () {
                        var thisNext = $(this).next();
                        if (!thisNext.hasClass("grupo")) {
                            if (thisNext.is(":visible")) {
                                thisNext.fadeOut("fast");
                            } else {
                                thisNext.fadeIn("fast");
                            }
                        }
                    });
                }
                $('#' + modelo.idControle + ' > tbody:empty').remove();
                $('#' + modelo.idControle + ' > tbody').show();
                api.columns.adjust();
                //
                isGerenciandoAgrupamento = false;
            }
        }
    }
}