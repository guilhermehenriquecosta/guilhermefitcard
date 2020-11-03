$(document).ready(function(){
    document.getElementById("carregandoTodos").style.display = '';
    $('#modalCriar-agencia').mask('000-0');
    $('#modalCriar-conta').mask('00.000-0');
    $('#modalCriar-cnpj').mask('00.000.000/0000-00');
    $('#modalCriar-estado').mask('AA');
    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        spOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(SPMaskBehavior.apply({}, arguments), options);
            }
        };
    $('#modalCriar-telefone').mask(SPMaskBehavior, spOptions);

    $('#modalCriar-categoria').change(function () {
        var categoria = $('#modalCriar-categoria').find("option:selected").text();
        $('#modalCriar-categoria-nome').val(categoria);
        if (categoria.toString().toUpperCase() == "SUPERMERCADO") {
            document.getElementById('modalCriar-telefone').required = true;
        } else {
            document.getElementById('modalCriar-telefone').required = false;
        }
    })

    $('#modalEditar-categoria').change(function () {
        var categoria = $('#modalEditar-categoria').find("option:selected").text();
        $('#modalEditar-categoria-nome').val(categoria);
        if (categoria.toString().toUpperCase() == "SUPERMERCADO") {
            document.getElementById('modalEditar-telefone').required = true;
        } else {
            document.getElementById('modalEditar-telefone').required = false;
        }
    })

    let dropdownCriar = $('#modalCriar-status');
    let dropdownEditar = $('#modalEditar-status');
    const status = ['Ativo','Inativo'];
    for (let i = 0; i < status.length; i++) {
        dropdownCriar.append($('<option></option>').attr('value', i+1).text(status[i]));
        dropdownEditar.append($('<option></option>').attr('value', i+1).text(status[i]));
    }

    limparListagem();
    iniciarEstabelecimentos();
});

function validarEmail(email) {
    var regra = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (email.match(regra)) {
        return true;
    }
    return false;
}

function validarCNPJ(cnpj) {
    cnpj = cnpj.replace(/[^\d]+/g,'');
    if (cnpj == '') return false;
    if (cnpj.length != 14) return false;
    if (cnpj == "00000000000000" || 
        cnpj == "11111111111111" || 
        cnpj == "22222222222222" || 
        cnpj == "33333333333333" || 
        cnpj == "44444444444444" || 
        cnpj == "55555555555555" || 
        cnpj == "66666666666666" || 
        cnpj == "77777777777777" || 
        cnpj == "88888888888888" || 
        cnpj == "99999999999999") return false;
        
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0,tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0)) return false;
        
    tamanho = tamanho + 1;
    numeros = cnpj.substring(0,tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1)) return false;
            
    return true;
}

function notificar(titulo, mensagem, tipo) {
    $.notify({
        title: titulo,
        message: mensagem
    },{
        placement: {
            from: "bottom",
            align: "right"
        },
        offset: {
            x:10,
            y:15
        },
        spacing: 5,
        z_index: 2031,
        delay: 1500,
        type: tipo,
        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="title" style="font-weight: bold;">{1}</span><br> ' +
                    '<span data-notify="message">{2}</span>' +
                '</div>' 
    });
    return false; 
}

function limparListagem(){
    if ($("#tblEstabelecimentos tbody").length != 0){
        $("#tblEstabelecimentos tbody").remove();
    }
}

function adicionarListagem(item){
    if ($("#tblEstabelecimentos tbody").length == 0){
        $("#tblEstabelecimentos").append("<tbody></tbody>");
    }
    var categoria = "";
    if (item.categoria != null) categoria = item.categoria.nome;
    $("#tblEstabelecimentos tbody").append(
        "<tr>" +
            "<td style=\"width:auto;\">" + item.razaoSocial + "</td>" +
            "<td style=\"width:auto;\">" + item.cnpj.substring(0, 2)+"."+item.cnpj.substring(2, 5)+"."+item.cnpj.substring(5, 8)+"/"+item.cnpj.substring(8, 12)+"-"+item.cnpj.substring(12, 14) + "</td>" +
            "<td style=\"width:auto;\">" + categoria + "</td>" +
            "<td style=\"width: 1% !important; white-space: nowrap !important;\">" +
            "<a href=\"#modalEditar\" onclick=\"opcaoItem('" + item.id + "',1)\" data-toggle=\"modal\" data-target=\"#modalEditar\">" +
            "<i class=\"fas fa-lg fa-edit\"></i>" +
            "</a>" +
            " | " +
            "<a href=\"#modalDetalhar\" onclick=\"opcaoItem('" + item.id + "',2)\" data-toggle=\"modal\" data-target=\"#modalDetalhar\">" +
            "<i class=\"fas fa-lg fa-eye\"></i>" +
            "</a>" +
            " | " +
            "<a href=\"#modalExcluir\" onclick=\"opcaoItem('" + item.id + "',3)\" data-toggle=\"modal\" data-target=\"#modalExcluir\">" +
            "<i class=\"fas fa-lg fa-trash-alt\"></i>" +
            "</a>" +
            "</td>" +
        "</tr>"
    );
}

async function buscarEstabelecimentos() {
    var myHeaders = new Headers();
    var myInit = { method: 'GET',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default' };
    const URL = 'http://18.229.127.212:8009/api/v1/estabelecimento';
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function buscarEstabelecimento(id) {
    var myHeaders = new Headers();
    var myInit = { method: 'GET',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default' };

    const URL = 'http://18.229.127.212:8009/api/v1/estabelecimento/'+id;
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function criarEstabelecimento(item) {
    var myHeaders = new Headers();
    myHeaders.append("Accept", "application/json");
    myHeaders.append("Content-Type", "application/json");
    var myInit = { method: 'POST',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default',
                body: JSON.stringify(item)};

    const URL = 'http://18.229.127.212:8009/api/v1/estabelecimento/';
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function editarEstabelecimento(item) {
    var myHeaders = new Headers();
    myHeaders.append("Accept", "application/json");
    myHeaders.append("Content-Type", "application/json");
    var myInit = { method: 'PUT',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default',
                body: JSON.stringify(item)};

    const URL = 'http://18.229.127.212:8009/api/v1/estabelecimento/'+item.id;
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function excluirEstabelecimento(id) {
    var myHeaders = new Headers();
    var myInit = { method: 'DELETE',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default' };

    const URL = 'http://18.229.127.212:8009/api/v1/estabelecimento/'+id;
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function buscarCategorias() {
    var myHeaders = new Headers();
    var myInit = { method: 'GET',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default' };

    const URL = 'http://18.229.127.212:8009/api/v1/categoria/';
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function iniciarEstabelecimentos() {
    const resposta = await buscarEstabelecimentos();
    document.getElementById("carregandoTodos").style.display = 'none';
    if (resposta.status == 204) {
        notificar("Dados não encontrados","Ainda não existem registros cadastrados.", "info");
    } else if (resposta.status != 200) { 
        notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
    }
    if(resposta.status == 200) {
        const estabelecimentos = await resposta.json();
        if (estabelecimentos.length > 0) {
            estabelecimentos.forEach(item => {
                adicionarListagem(item);
            });
        }
    }
    return true;
}

async function opcaoItem(id, opcao) {
    if (opcao === 1) {
        limparModalEditar();
        let dropdownEditar = $('#modalEditar-categoria');
        dropdownEditar.empty();
        dropdownEditar.append('<option value="00000000-0000-0000-0000-000000000000" selected="true">Escolha uma categoria...</option>');
        dropdownEditar.prop('selectedIndex', 0);

        let resposta = await buscarCategorias();
        if (resposta.status == 200) {
            const categorias = await resposta.json();
            if (categorias.length > 0) {
                categorias.forEach(item => {
                    dropdownEditar.append($('<option></option>').attr('value', item.id).text(item.nome));
                });
            }
        }
    } 
    if (opcao === 2) limparModalDetalhar();
    if (opcao === 3) limparModalExcluir();

    let resposta = await buscarEstabelecimento(id);
    document.getElementById("carregandoEditar").style.display = 'none';
    document.getElementById("carregandoDetalhar").style.display = 'none';
    document.getElementById("carregandoExcluir").style.display = 'none';
    if (resposta.status == 400) {
        notificar("Erro","A operação requisitada é inválida.", "danger");
    } else if (resposta.status == 404) {
        notificar("Dados não encontrados","O item buscado não foi encontrado na base.", "info");
    } else if (resposta.status != 200) {
        notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
    }
    const estabelecimento = await resposta.json();
    if (opcao === 1) {
        janelamodal = "modalEditar";
        preencherModalEditar(estabelecimento);
    } else if (opcao === 2) {
        janelamodal = "modalDetalhar";
        preencherModalDetalhar(estabelecimento);
    } else if (opcao === 3) {
        janelamodal = "modalExcluir";
        preencherModalExcluir(estabelecimento);
    }
    $('#'+janelamodal).modal({show:true});
}

async function submitCriar() {
    document.getElementById("carregandoCriarSalvar").style.display = '';
    document.getElementById('modalCriar-cnpj').setCustomValidity("");
    document.getElementById('modalCriar-email').setCustomValidity("");
    var form = document.getElementById("formCriar");
    if ($("#modalCriar-cnpj").val() != "" && !validarCNPJ($("#modalCriar-cnpj").val())) {
        document.getElementById('modalCriar-cnpj').setCustomValidity("O CNPJ informado é inválido.");
        notificar("CNPJ","O CNPJ informado é inválido.", "warning");
        event.preventDefault();
        event.stopPropagation();
    }
    if ($("#modalCriar-email").val() != "" && !validarEmail($("#modalCriar-email").val())) {
        document.getElementById('modalCriar-email').setCustomValidity("O endereço de E-mail informado é inválido.");
        notificar("E-mail","O endereço de E-mail informado é inválido.", "warning");
        event.preventDefault();
        event.stopPropagation();
    }
    if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
    } else {
        Estabelecimento = new Object();
        if ($("#modalCriar-categoria").val() != "00000000-0000-0000-0000-000000000000") {
            Categoria = new Object();
            if ($("#modalCriar-categoria").val() != "") Categoria.id = $("#modalCriar-categoria").val();
            if ($("#modalCriar-categoria-nome").val() != "") Categoria.nome = $("#modalCriar-categoria-nome").val();
            Estabelecimento.categoria = Categoria;
        }
        if ($("#modalCriar-categoria").val() != "") Estabelecimento.idCategoria = $("#modalCriar-categoria").val();
        if ($("#modalCriar-razaoSocial").val() != "") Estabelecimento.razaoSocial = $("#modalCriar-razaoSocial").val();
        if ($("#modalCriar-nomeFantasia").val() != "") Estabelecimento.nomeFantasia = $("#modalCriar-nomeFantasia").val();
        if ($("#modalCriar-cnpj").val() != "") Estabelecimento.cnpj = $("#modalCriar-cnpj").val().replace(/[^\d]+/g,'');
        if ($("#modalCriar-email").val() != "") Estabelecimento.email = $("#modalCriar-email").val();
        if ($("#modalCriar-telefone").val() != "") Estabelecimento.telefone = $("#modalCriar-telefone").val().replace(/[^\d]+/g,'');
        if ($("#modalCriar-endereco").val() != "") Estabelecimento.endereco = $("#modalCriar-endereco").val();
        if ($("#modalCriar-cidade").val() != "") Estabelecimento.cidade = $("#modalCriar-cidade").val();
        if ($("#modalCriar-estado").val() != "") Estabelecimento.estado = $("#modalCriar-estado").val();
        if ($("#modalCriar-dataCadastro").val() != "") Estabelecimento.dataCadastro = $("#modalCriar-dataCadastro").val();
        if ($("#modalCriar-status").val() != "") Estabelecimento.status = $("#modalCriar-status").val();
        if ($("#modalCriar-agencia").val() != "") Estabelecimento.agencia = $("#modalCriar-agencia").val().replace(/[^\d]+/g,'');
        if ($("#modalCriar-conta").val() != "") Estabelecimento.conta = $("#modalCriar-conta").val().replace(/[^\d]+/g,'');
        
        let resposta = await criarEstabelecimento(Estabelecimento);
        let estabelecimento = await resposta.json();
        if (resposta.status == 201) {
            notificar("Registro cadastrado","Os dados foram inseridos na base com sucesso.", "success");
        } else if (resposta.status == 400) {
            notificar("Erro","A operação requisitada é inválida.", "danger");
        } else {
            notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
        }
        limparListagem();
        iniciarEstabelecimentos();
    }
    form.classList.add('was-validated');
    document.getElementById("carregandoCriarSalvar").style.display = 'none';
}

async function submitEditar() {
    document.getElementById("carregandoEditarSalvar").style.display = '';
    document.getElementById('modalEditar-cnpj').setCustomValidity("");
    document.getElementById('modalEditar-email').setCustomValidity("");
    var form = document.getElementById("formEditar");
    if ($("#modalEditar-cnpj").val() != "" && !validarCNPJ($("#modalEditar-cnpj").val())) {
        document.getElementById('modalEditar-cnpj').setCustomValidity("O CNPJ informado é inválido.");
        notificar("CNPJ","O CNPJ informado é inválido.", "warning");
        event.preventDefault();
        event.stopPropagation();
    } 
    if ($("#modalEditar-email").val() != "" && !validarEmail($("#modalEditar-email").val())) {
        document.getElementById('modalEditar-email').setCustomValidity("O endereço de E-mail informado é inválido.");
        notificar("E-mail","O endereço de E-mail informado é inválido.", "warning");
        event.preventDefault();
        event.stopPropagation();
    }
    if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
    } else {
        Estabelecimento = new Object();
        if ($("#modalEditar-categoria").val() != "00000000-0000-0000-0000-000000000000") {
            Categoria = new Object();
            if ($("#modalEditar-categoria").val() != "") Categoria.id = $("#modalEditar-categoria").val();
            if ($("#modalEditar-categoria-nome").val() != "") Categoria.nome = $("#modalEditar-categoria-nome").val();
            Estabelecimento.categoria = Categoria;
        }
        if ($("#modalEditar-id").val() != "") Estabelecimento.id = $("#modalEditar-id").val();
        if ($("#modalEditar-categoria").val() != "") Estabelecimento.idCategoria = $("#modalEditar-categoria").val();
        if ($("#modalEditar-razaoSocial").val() != "") Estabelecimento.razaoSocial = $("#modalEditar-razaoSocial").val();
        if ($("#modalEditar-nomeFantasia").val() != "") Estabelecimento.nomeFantasia = $("#modalEditar-nomeFantasia").val();
        if ($("#modalEditar-cnpj").val() != "") Estabelecimento.cnpj = $("#modalEditar-cnpj").val().replace(/[^\d]+/g,'');
        if ($("#modalEditar-email").val() != "") Estabelecimento.email = $("#modalEditar-email").val();
        if ($("#modalEditar-telefone").val() != "") Estabelecimento.telefone = $("#modalEditar-telefone").val().replace(/[^\d]+/g,'');
        if ($("#modalEditar-endereco").val() != "") Estabelecimento.endereco = $("#modalEditar-endereco").val();
        if ($("#modalEditar-cidade").val() != "") Estabelecimento.cidade = $("#modalEditar-cidade").val();
        if ($("#modalEditar-estado").val() != "") Estabelecimento.estado = $("#modalEditar-estado").val();
        if ($("#modalEditar-dataCadastro").val() != "") Estabelecimento.dataCadastro = $("#modalEditar-dataCadastro").val();
        if ($("#modalEditar-status").val() != "") Estabelecimento.status = $("#modalEditar-status").val();
        if ($("#modalEditar-agencia").val() != "") Estabelecimento.agencia = $("#modalEditar-agencia").val().replace(/[^\d]+/g,'');
        if ($("#modalEditar-conta").val() != "") Estabelecimento.conta = $("#modalEditar-conta").val().replace(/[^\d]+/g,'');

        let resposta = await editarEstabelecimento(Estabelecimento);
        let estabelecimento = null;
        if (resposta.status == 202) {
            notificar("Registro editado","Os dados foram atualizados na base com sucesso.", "success");
        } else if (resposta.status == 400) {
            let estabelecimento = await resposta.json();
            notificar("Erro","A operação requisitada é inválida.", "danger");
        } else if (resposta.status == 404) {
            notificar("Dados não encontrados","O item buscado não foi encontrado na base.", "info");
        } else {
            notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
        }
        limparListagem();
        iniciarEstabelecimentos();
    }
    form.classList.add('was-validated');
    document.getElementById("carregandoEditarSalvar").style.display = 'none';
}

async function submitExcluir() {
    document.getElementById("carregandoExcluirSalvar").style.display = '';
    var id = document.getElementById("modalExcluir-id").innerHTML;
    let resposta = await excluirEstabelecimento(id);
    if (resposta.status == 200) {
        notificar("Registro excluído","Os dados foram removidos da base com sucesso.", "success");
    } else if (resposta.status == 400) {
        notificar("Erro","A operação requisitada é inválida.", "danger");
    } else if (resposta.status == 404) {
        notificar("Dados não encontrados","O item buscado não foi encontrado na base.", "info");
    } else {
        notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
    }
    limparListagem();
    iniciarEstabelecimentos();
    document.getElementById("carregandoExcluirSalvar").style.display = 'none';
}

function limparModalCriar() {
    $('#modalCriar-id').val("");
    $('#modalCriar-razaoSocial').val("");
    $('#modalCriar-nomeFantasia').val("");
    $('#modalCriar-cnpj').val("");
    $('#modalCriar-email').val("");
    $('#modalCriar-telefone').val("");
    $('#modalCriar-endereco').val("");
    $('#modalCriar-cidade').val("");
    $('#modalCriar-estado').val("");
    $('#modalCriar-dataCadastro').val("");
    $('#modalCriar-categoria-nome').val("");
    $('#modalCriar-agencia').val("");
    $('#modalCriar-conta').val("");
    $('#modalCriar-categoria').prop('selectedIndex', 0);
    $('#modalCriar-status').prop('selectedIndex', 0);
    document.getElementById('modalCriar-cnpj').setCustomValidity("");
    document.getElementById('modalCriar-email').setCustomValidity("");
}

function limparModalEditar() {
    $('#modalEditar-id').val("");
    $('#modalEditar-razaoSocial').val("");
    $('#modalEditar-nomeFantasia').val("");
    $('#modalEditar-cnpj').val("");
    $('#modalEditar-email').val("");
    $('#modalEditar-telefone').val("");
    $('#modalEditar-endereco').val("");
    $('#modalEditar-cidade').val("");
    $('#modalEditar-estado').val("");
    $('#modalEditar-dataCadastro').val("");
    $('#modalEditar-categoria-nome').val("");
    $('#modalEditar-agencia').val("");
    $('#modalEditar-conta').val("");
    $('#modalEditar-categoria').prop('selectedIndex', 0);
    $('#modalEditar-status').prop('selectedIndex', 0);
    document.getElementById('modalEditar-cnpj').setCustomValidity("");
    document.getElementById('modalEditar-email').setCustomValidity("");
}

function limparModalDetalhar() {
    document.getElementById("modalDetalhar-razaoSocial").innerHTML = "";
    document.getElementById("modalDetalhar-nomeFantasia").innerHTML = "";
    document.getElementById("modalDetalhar-cnpj").innerHTML = "";
    document.getElementById("modalDetalhar-email").innerHTML = "";
    document.getElementById("modalDetalhar-telefone").innerHTML = "";
    document.getElementById("modalDetalhar-endereco").innerHTML = "";
    document.getElementById("modalDetalhar-cidade").innerHTML = "";
    document.getElementById("modalDetalhar-estado").innerHTML = "";
    document.getElementById("modalDetalhar-dataCadastro").innerHTML = "";
    document.getElementById("modalDetalhar-categoria").innerHTML = "";
    document.getElementById("modalDetalhar-status").innerHTML = "";
    document.getElementById("modalDetalhar-agencia").innerHTML = "";
    document.getElementById("modalDetalhar-conta").innerHTML = "";
    document.getElementById("modalDetalhar-dataCriacao").innerHTML = "";
    document.getElementById("modalDetalhar-dataEdicao").innerHTML = "";
}

function limparModalExcluir() {
    document.getElementById("modalExcluir-id").innerHTML = "";
    document.getElementById("modalExcluir-razaoSocial").innerHTML = "";
    document.getElementById("modalExcluir-nomeFantasia").innerHTML = "";
    document.getElementById("modalExcluir-cnpj").innerHTML = "";
    document.getElementById("modalExcluir-email").innerHTML = "";
    document.getElementById("modalExcluir-telefone").innerHTML = "";
    document.getElementById("modalExcluir-endereco").innerHTML = "";
    document.getElementById("modalExcluir-cidade").innerHTML = "";
    document.getElementById("modalExcluir-estado").innerHTML = "";
    document.getElementById("modalExcluir-dataCadastro").innerHTML = "";
    document.getElementById("modalExcluir-categoria").innerHTML = "";
    document.getElementById("modalExcluir-status").innerHTML = "";
    document.getElementById("modalExcluir-agencia").innerHTML = "";
    document.getElementById("modalExcluir-conta").innerHTML = "";
    document.getElementById("modalExcluir-dataCriacao").innerHTML = "";
    document.getElementById("modalExcluir-dataEdicao").innerHTML = "";
}

function preencherModalEditar(item) {
    $('#modalEditar-id').val(item.id);
    $('#modalEditar-razaoSocial').val(item.razaoSocial);
    $('#modalEditar-nomeFantasia').val(item.nomeFantasia);
    $('#modalEditar-cnpj').unmask();
    $('#modalEditar-cnpj').val(item.cnpj).mask('00.000.000/0000-00');
    $('#modalEditar-email').val(item.email);
    $('#modalEditar-telefone').unmask();
    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        spOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(SPMaskBehavior.apply({}, arguments), options);
            }
        };
    $('#modalEditar-telefone').val(item.telefone).mask(SPMaskBehavior, spOptions);
    $('#modalEditar-endereco').val(item.endereco);
    $('#modalEditar-cidade').val(item.cidade);
    $('#modalEditar-estado').unmask();
    $('#modalEditar-estado').val(item.estado).mask('AA');
    $('#modalEditar-dataCadastro').val(item.dataCadastro);
    if (item.categoria != null) $('#modalEditar-categoria-nome').val(item.categoria.nome);
    $('#modalEditar-agencia').unmask();
    $('#modalEditar-agencia').val(item.agencia).mask('000-0');
    $('#modalEditar-conta').unmask();
    $('#modalEditar-conta').val(item.conta).mask('00.000-0');

    $('#modalEditar-categoria > option').each(function () {
        if ($(this).text() === $('#modalEditar-categoria-nome').val())
            $('#modalEditar-categoria').prop('selectedIndex', $(this).index());
    });
    $('#modalEditar-status').prop('selectedIndex', item.status);
}

function preencherModalDetalhar(item) {
    var dataCadastro = new Date(item.dataCadastro),
        dataCriacao = new Date(item.dataCriacao),
        dataEdicao = new Date(item.dataEdicao),
        categoria = "",
        telefone = "",
        agencia = "",
        conta = "";
    const status = ['Ativo','Inativo'];
    if (item.categoria != null) categoria = item.categoria.nome;
    if (item.telefone != null) {
        if (item.telefone.length == 11) {
            telefone = "("+item.telefone.substring(0,2)+") "+item.telefone.substring(2,7)+"-"+item.telefone.substring(7,11);
        } else if (item.telefone.length == 10) {    
            telefone = "("+item.telefone.substring(0,2)+") "+item.telefone.substring(2,6)+"-"+item.telefone.substring(6,10);
        }
    }
    if (item.agencia != null) agencia = item.agencia.substring(0,3)+"-"+item.agencia.substring(3,4);
    if (item.conta != null) conta = item.conta.substring(0,2)+"."+item.conta.substring(2,5)+"-"+item.conta.substring(5,6);
    
    document.getElementById("modalDetalhar-razaoSocial").innerHTML = item.razaoSocial;
    document.getElementById("modalDetalhar-nomeFantasia").innerHTML = item.nomeFantasia;
    document.getElementById("modalDetalhar-cnpj").innerHTML = item.cnpj.substring(0, 2)+"."+item.cnpj.substring(2, 5)+"."+item.cnpj.substring(5, 8)+"/"+item.cnpj.substring(8, 12)+"-"+item.cnpj.substring(12, 14);
    document.getElementById("modalDetalhar-email").innerHTML = item.email;
    document.getElementById("modalDetalhar-telefone").innerHTML = telefone;
    document.getElementById("modalDetalhar-endereco").innerHTML = item.endereco;
    document.getElementById("modalDetalhar-cidade").innerHTML = item.cidade;
    document.getElementById("modalDetalhar-estado").innerHTML = item.estado;
    document.getElementById("modalDetalhar-dataCadastro").innerHTML = item.dataCadastro != null ? dataCadastro.toLocaleDateString() : "";
    document.getElementById("modalDetalhar-categoria").innerHTML = categoria;
    document.getElementById("modalDetalhar-status").innerHTML = (item.status == null || item.status == "0") ? "" : status[item.status-1];
    document.getElementById("modalDetalhar-agencia").innerHTML = agencia;
    document.getElementById("modalDetalhar-conta").innerHTML = conta;
    document.getElementById("modalDetalhar-dataCriacao").innerHTML = dataCriacao.toLocaleDateString() + " " + dataCriacao.toLocaleTimeString();
    document.getElementById("modalDetalhar-dataEdicao").innerHTML = dataEdicao.toLocaleDateString() + " " + dataEdicao.toLocaleTimeString();
}

function preencherModalExcluir(item) {
    var dataCadastro = new Date(item.dataCadastro),
        dataCriacao = new Date(item.dataCriacao),
        dataEdicao = new Date(item.dataEdicao),
        categoria = "",
        telefone = ""
        agencia = "",
        conta = "";
    const status = ['Ativo','Inativo'];
    if (item.categoria != null) categoria = item.categoria.nome;
    if (item.telefone != null) {
        if (item.telefone.length == 11) {
            telefone = "("+item.telefone.substring(0,2)+") "+item.telefone.substring(2,7)+"-"+item.telefone.substring(7,11);
        } else if (item.telefone.length == 10) {    
            telefone = "("+item.telefone.substring(0,2)+") "+item.telefone.substring(2,6)+"-"+item.telefone.substring(6,10);
        }
    }
    if (item.agencia != null) agencia = item.agencia.substring(0,3)+"-"+item.agencia.substring(3,4);
    if (item.conta != null) conta = item.conta.substring(0,2)+"."+item.conta.substring(2,5)+"-"+item.conta.substring(5,6);

    document.getElementById("modalExcluir-id").innerHTML = item.id;
    document.getElementById("modalExcluir-razaoSocial").innerHTML = item.razaoSocial;
    document.getElementById("modalExcluir-nomeFantasia").innerHTML = item.nomeFantasia;
    document.getElementById("modalExcluir-cnpj").innerHTML = item.cnpj.substring(0, 2)+"."+item.cnpj.substring(2, 5)+"."+item.cnpj.substring(5, 8)+"/"+item.cnpj.substring(8, 12)+"-"+item.cnpj.substring(12, 14);
    document.getElementById("modalExcluir-email").innerHTML = item.email;
    document.getElementById("modalExcluir-telefone").innerHTML = telefone;
    document.getElementById("modalExcluir-endereco").innerHTML = item.endereco;
    document.getElementById("modalExcluir-cidade").innerHTML = item.cidade;
    document.getElementById("modalExcluir-estado").innerHTML = item.estado;
    document.getElementById("modalExcluir-dataCadastro").innerHTML = item.dataCadastro != null ? dataCadastro.toLocaleDateString() : "";
    document.getElementById("modalExcluir-categoria").innerHTML = categoria;
    document.getElementById("modalExcluir-status").innerHTML = (item.status == null || item.status == "0") ? "" : status[item.status-1];
    document.getElementById("modalExcluir-agencia").innerHTML = agencia;
    document.getElementById("modalExcluir-conta").innerHTML = conta;
    document.getElementById("modalExcluir-dataCriacao").innerHTML = dataCriacao.toLocaleDateString() + " " + dataCriacao.toLocaleTimeString();
    document.getElementById("modalExcluir-dataEdicao").innerHTML = dataEdicao.toLocaleDateString() + " " + dataEdicao.toLocaleTimeString();
}

$('#modalCriar').on('show.bs.modal', async function(e){
    limparModalCriar();
    document.getElementById("carregandoCriar").style.display = '';
    document.getElementById('formCriar').classList.remove('was-validated');
    document.getElementById('formCriar').classList.add('needs-validation');
    document.getElementById('formCriar').noValidate = true;

    let dropdown = $('#modalCriar-categoria');
    dropdown.empty();
    dropdown.append('<option value="00000000-0000-0000-0000-000000000000" selected="true">Escolha uma categoria...</option>');
    dropdown.prop('selectedIndex', 0);

    let resposta = await buscarCategorias();
    if (resposta.status == 200) {
        const categorias = await resposta.json();
        if (categorias.length > 0) {
            categorias.forEach(item => {
                dropdown.append($('<option></option>').attr('value', item.id).text(item.nome));
            });
        }
    }
    document.getElementById("carregandoCriar").style.display = 'none';
    document.getElementById("carregandoCriarSalvar").style.display = 'none';
});

$('#modalCriar').on('keydown', function ( e ) {
    var key = e.which || e.keyCode;
    if (key == 13) {
        submitCriar();
    }
});

$('#modalEditar').on('show.bs.modal', async function(e){
    document.getElementById('formEditar').classList.remove('was-validated');
    document.getElementById('formEditar').classList.add('needs-validation');
    document.getElementById('formEditar').noValidate = true;
    document.getElementById("carregandoEditar").style.display = '';
    document.getElementById("carregandoEditarSalvar").style.display = 'none';
});

$('#modalEditar').on('keydown', function ( e ) {
    var key = e.which || e.keyCode;
    if (key == 13) {
        submitEditar();
    }
});

$('#modalDetalhar').on('show.bs.modal', function(e){
    document.getElementById("carregandoDetalhar").style.display = '';
});

$('#modalExcluir').on('show.bs.modal', function(e){
    document.getElementById("carregandoExcluir").style.display = '';
    document.getElementById("carregandoExcluirSalvar").style.display = 'none';
});