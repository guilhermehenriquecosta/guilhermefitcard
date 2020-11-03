$(document).ready(function(){
    document.getElementById("carregandoTodas").style.display = '';
    
    limparListagem();
    iniciarCategorias();
});

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
    if ($("#tblCategorias tbody").length != 0){
        $("#tblCategorias tbody").remove();
    }
}

function adicionarListagem(item){
    if ($("#tblCategorias tbody").length == 0){
        $("#tblCategorias").append("<tbody></tbody>");
    }
    $("#tblCategorias tbody").append(
        "<tr>" +
            "<td style=\"width:auto;\">" + item.nome + "</td>" +
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

async function buscarCategorias() {
    var myHeaders = new Headers();
    var myInit = { method: 'GET',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default' };
    const URL = 'http://18.229.127.212:8009/api/v1/categoria';
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function buscarCategoria(id) {
    var myHeaders = new Headers();
    var myInit = { method: 'GET',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default' };

    const URL = 'http://18.229.127.212:8009/api/v1/categoria/'+id;
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function criarCategoria(item) {
    var myHeaders = new Headers();
    myHeaders.append("Accept", "application/json");
    myHeaders.append("Content-Type", "application/json");
    var myInit = { method: 'POST',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default',
                body: JSON.stringify(item)};

    const URL = 'http://18.229.127.212:8009/api/v1/categoria/';
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function editarCategoria(item) {
    var myHeaders = new Headers();
    myHeaders.append("Accept", "application/json");
    myHeaders.append("Content-Type", "application/json");
    var myInit = { method: 'PUT',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default',
                body: JSON.stringify(item)};

    const URL = 'http://18.229.127.212:8009/api/v1/categoria/'+item.id;
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function excluirCategoria(id) {
    var myHeaders = new Headers();
    var myInit = { method: 'DELETE',
                headers: myHeaders,
                mode: 'cors',
                cache: 'default' };

    const URL = 'http://18.229.127.212:8009/api/v1/categoria/'+id;
    const resposta = await fetch(URL, myInit);
    return resposta;
}

async function iniciarCategorias() {
    const resposta = await buscarCategorias();
    document.getElementById("carregandoTodas").style.display = 'none';
    if (resposta.status == 204) {
        notificar("Dados não encontrados","Ainda não existem registros cadastrados.", "info");
    } else if (resposta.status != 200) { 
        notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
    }
    if(resposta.status == 200) {
        const categorias = await resposta.json();
        if (categorias.length > 0) {
            categorias.forEach(item => {
                adicionarListagem(item);
            });
        }
    }
    return true;
}

async function opcaoItem(id, opcao) {
    if (opcao === 1) limparModalEditar();
    if (opcao === 2) limparModalDetalhar();
    if (opcao === 3) limparModalExcluir();
    let resposta = await buscarCategoria(id);
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
    const categoria = await resposta.json();
    if (opcao === 1) {
        janelamodal = "modalEditar";
        preencherModalEditar(categoria);
    } else if (opcao === 2) {
        janelamodal = "modalDetalhar";
        preencherModalDetalhar(categoria);
    } else if (opcao === 3) {
        janelamodal = "modalExcluir";
        preencherModalExcluir(categoria);
    }
    $('#'+janelamodal).modal({show:true});
}

async function submitCriar() {
    document.getElementById("carregandoCriarSalvar").style.display = '';
    var form = document.getElementById("formCriar");
    if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
    } else {
        Categoria = new Object();
        Categoria.nome = $("#modalCriar-nome").val();
        let resposta = await criarCategoria(Categoria);
        let categoria = await resposta.json();
        if (resposta.status == 201) {
            notificar("Registro cadastrado","Os dados foram inseridos na base com sucesso.", "success");
        } else if (resposta.status == 400) {
            notificar("Erro","A operação requisitada é inválida.", "danger");
        } else {
            notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
        }
        limparListagem();
        iniciarCategorias();
    }
    form.classList.add('was-validated');
    document.getElementById("carregandoCriarSalvar").style.display = 'none';
}

async function submitEditar() {
    document.getElementById("carregandoEditarSalvar").style.display = '';
    var form = document.getElementById("formEditar");
    if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
    } else {
        Categoria = new Object();
        Categoria.id = $("#modalEditar-id").val();
        Categoria.nome = $("#modalEditar-nome").val();
        let resposta = await editarCategoria(Categoria);
        let categoria = null;
        if (resposta.status == 202) {
            notificar("Registro editado","Os dados foram atualizados na base com sucesso.", "success");
        } else if (resposta.status == 400) {
            let categoria = await resposta.json();
            notificar("Erro","A operação requisitada é inválida.", "danger");
        } else if (resposta.status == 404) {
            notificar("Dados não encontrados","O item buscado não foi encontrado na base.", "info");
        } else {
            notificar("Erro interno","Ocorreu um erro no servidor, tente novamente mais tarde.", "danger");
        }
        limparListagem();
        iniciarCategorias();
    }
    form.classList.add('was-validated');
    document.getElementById("carregandoEditarSalvar").style.display = 'none';
}

async function submitExcluir() {
    document.getElementById("carregandoExcluirSalvar").style.display = '';
    var id = document.getElementById("modalExcluir-id").innerHTML;
    let resposta = await excluirCategoria(id);
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
    iniciarCategorias();
    document.getElementById("carregandoExcluirSalvar").style.display = 'none';
}

function limparModalCriar() {
    $('#modalCriar-nome').val("");
}

function limparModalEditar() {
    $('#modalEditar-id').val("");
    $('#modalEditar-nome').val("");
}

function limparModalDetalhar() {
    document.getElementById("modalDetalhar-nome").innerHTML = "";
    document.getElementById("modalDetalhar-dataCriacao").innerHTML = "";
    document.getElementById("modalDetalhar-dataEdicao").innerHTML = "";
}

function limparModalExcluir() {
    document.getElementById("modalExcluir-id").innerHTML = "";
    document.getElementById("modalExcluir-nome").innerHTML = "";
    document.getElementById("modalExcluir-dataCriacao").innerHTML = "";
    document.getElementById("modalExcluir-dataEdicao").innerHTML = "";
}

function preencherModalEditar(item) {
    $('#modalEditar-id').val(item.id);
    $('#modalEditar-nome').val(item.nome);
}

function preencherModalDetalhar(item) {
    var dataCriacao = new Date(item.dataCriacao),
        dataEdicao = new Date(item.dataEdicao);
    document.getElementById("modalDetalhar-nome").innerHTML = item.nome;
    document.getElementById("modalDetalhar-dataCriacao").innerHTML = dataCriacao.toLocaleDateString() + " " + dataCriacao.toLocaleTimeString();
    document.getElementById("modalDetalhar-dataEdicao").innerHTML = dataEdicao.toLocaleDateString() + " " + dataEdicao.toLocaleTimeString();
}

function preencherModalExcluir(item) {
    var dataCriacao = new Date(item.dataCriacao),
        dataEdicao = new Date(item.dataEdicao);
    document.getElementById("modalExcluir-id").innerHTML = item.id;
    document.getElementById("modalExcluir-nome").innerHTML = item.nome;
    document.getElementById("modalExcluir-dataCriacao").innerHTML = dataCriacao.toLocaleDateString() + " " + dataCriacao.toLocaleTimeString();
    document.getElementById("modalExcluir-dataEdicao").innerHTML = dataEdicao.toLocaleDateString() + " " + dataEdicao.toLocaleTimeString();
}

$('#modalCriar').on('show.bs.modal', function(e){
    limparModalCriar();
    document.getElementById('formCriar').classList.remove('was-validated');
    document.getElementById('formCriar').classList.add('needs-validation');
    document.getElementById('formCriar').noValidate = true;
    document.getElementById("carregandoCriarSalvar").style.display = 'none';
});

$('#modalCriar').on('keydown', function ( e ) {
    var key = e.which || e.keyCode;
    if (key == 13) {
        submitCriar();
    }
});

$('#modalEditar').on('show.bs.modal', function(e){
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