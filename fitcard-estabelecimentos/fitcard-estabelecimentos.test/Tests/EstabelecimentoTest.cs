using fitcard_estabelecimentos.domain.Domain;
using fitcard_estabelecimentos.domain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace fitcard_estabelecimentos.test.Tests
{
    public class EstabelecimentoTest
    {
        //O estabelecimento deverá possuir as seguintes informações
        //    Razão Social;
        //    Nome Fantasia;
        //    CNPJ;
        //    E-mail;
        //    Endereço;
        //    Cidade;
        //    Estado;
        //    Telefone;
        //    Data de Cadastro;
        //    Categoria[Supermercado, Restaurante, Borracharia, Posto, Oficina];
        //    Status[Ativo, Inativo]
        //    Agência e Conta

        //Um estabelecimento deverá ter no mínimo: Razão Social e CNPJ
        //Caso o estabelecimento possua e-mail, o mesmo deverá ser válido;
        //Caso a categoria seja Supermercado, o telefone passa a ser obrigatório;

        [Fact]
        public void Estabelecimento_Objeto()
        {
            var idCat = Guid.NewGuid();
            var objEstabelecimentoTeste = new
            {
                IdCategoria = idCat,
                RazaoSocial = "Razão Social de Teste",
                NomeFantasia = "Nome Fantasia de Teste",
                CNPJ = "61426657000100",
                Email = "emaildeteste@email.com.br",
                Endereco = "Endereco de Teste",
                Cidade = "Cidade de Teste",
                Estado = "SP",
                Telefone = "11999998888",
                DataCadastro = DateTime.Now,
                Categoria = new Categoria
                {
                    Id = idCat,
                    Nome = "Supermercado",
                    DataCriacao = DateTime.Now,
                    DataEdicao = DateTime.Now,
                    Excluido = false
                },
                Status = Status.Ativo,
                Agencia = "1234",
                Conta = "123456",
                DataCriacao = DateTime.Now,
                DataEdicao = DateTime.Now,
                Excluido = false
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.NomeFantasia= objEstabelecimentoTeste.NomeFantasia;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;
            objEstabelecimento.Email = objEstabelecimentoTeste.Email;
            objEstabelecimento.Endereco = objEstabelecimentoTeste.Endereco;
            objEstabelecimento.Cidade = objEstabelecimentoTeste.Cidade;
            objEstabelecimento.Estado = objEstabelecimentoTeste.Estado;
            objEstabelecimento.Telefone = objEstabelecimentoTeste.Telefone;
            objEstabelecimento.DataCadastro = objEstabelecimentoTeste.DataCadastro;
            objEstabelecimento.Categoria = objEstabelecimentoTeste.Categoria;
            objEstabelecimento.Status = objEstabelecimentoTeste.Status;
            objEstabelecimento.Agencia = objEstabelecimentoTeste.Agencia;
            objEstabelecimento.Conta = objEstabelecimentoTeste.Conta;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.NomeFantasia, objEstabelecimento.NomeFantasia);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.Equal(objEstabelecimentoTeste.Email, objEstabelecimento.Email);
            Assert.Equal(objEstabelecimentoTeste.Endereco, objEstabelecimento.Endereco);
            Assert.Equal(objEstabelecimentoTeste.Telefone, objEstabelecimento.Telefone);
            Assert.Equal(objEstabelecimentoTeste.Cidade, objEstabelecimento.Cidade);
            Assert.Equal(objEstabelecimentoTeste.Estado, objEstabelecimento.Estado);
            Assert.Equal(objEstabelecimentoTeste.Telefone, objEstabelecimento.Telefone);
            Assert.Equal(objEstabelecimentoTeste.DataCadastro, objEstabelecimento.DataCadastro);
            Assert.Equal(objEstabelecimentoTeste.Categoria, objEstabelecimento.Categoria);
            Assert.Equal(objEstabelecimentoTeste.Status, objEstabelecimento.Status);
            Assert.Equal(objEstabelecimentoTeste.Agencia, objEstabelecimento.Agencia);
            Assert.Equal(objEstabelecimentoTeste.Conta, objEstabelecimento.Conta);
            Assert.True(isValid);

            //Esse teste deverá ser APROVADO
        }

        [Fact]
        public void Estabelecimento_Razao_e_Cnpj_Valido()
        {
            var objEstabelecimentoTeste = new
            {
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "61426657000100" //VALIDO
            };
            
            Estabelecimento objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ; //VALIDO

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.True(isValid);

            //Esse teste deverá ser APROVADO
        }

        [Fact]
        public void Estabelecimento_Razao_e_Cnpj_Invalido()
        {
            var objEstabelecimentoTeste = new
            {
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "12312312000100" //INVALIDO
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.True(isValid);

            //Esse teste deverá ser REPROVADO
        }

        [Fact]
        public void Estabelecimento_Razao_Nula()
        {
            var objEstabelecimentoTeste = new
            {
                RazaoSocial = String.Empty,
                CNPJ = "61426657000100"
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.True(isValid);

            //Esse teste deverá ser REPROVADO
        }

        [Fact]
        public void Estabelecimento_Cnpj_Nulo()
        {
            var objEstabelecimentoTeste = new
            {
                RazaoSocial = "Razão Social de Teste",
                CNPJ = String.Empty
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.True(isValid);
            
            //Esse teste deverá ser REPROVADO
        }

        [Fact]
        public void Estabelecimento_Email_Nao_Nulo_Valido()
        {
            var objEstabelecimentoTeste = new
            {
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "61426657000100",
                Email = "emaildeteste@email.com.br"
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;
            objEstabelecimento.Email = objEstabelecimentoTeste.Email;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.Equal(objEstabelecimentoTeste.Email, objEstabelecimento.Email);
            Assert.True(isValid);

            //Esse teste deverá ser APROVADO
        }

        [Fact]
        public void Estabelecimento_Email_Nulo_Valido()
        {
            var objEstabelecimentoTeste = new
            {
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "61426657000100",
                Email = String.Empty
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;
            objEstabelecimento.Email = objEstabelecimentoTeste.Email;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.Equal(objEstabelecimentoTeste.Email, objEstabelecimento.Email);
            Assert.True(isValid);

            //Esse teste deverá ser APROVADO
        }

        [Fact]
        public void Estabelecimento_Email_Nao_Nulo_Invalido()
        {
            var objEstabelecimentoTeste = new
            {
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "61426657000100",
                Email = "emaildeteste@"
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;
            objEstabelecimento.Email = objEstabelecimentoTeste.Email;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.Equal(objEstabelecimentoTeste.Email, objEstabelecimento.Email);
            Assert.True(isValid);

            //Esse teste deverá ser REPROVADO
        }

        [Fact]
        public void Estabelecimento_Telefone_Obrigatorio_Valido()
        {
            var idCat = Guid.NewGuid();
            var objEstabelecimentoTeste = new
            {
                IdCategoria = idCat,
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "61426657000100",
                Categoria = new Categoria
                {
                    Id = idCat,
                    Nome = "Supermercado",
                    DataCriacao = DateTime.Now,
                    DataEdicao = DateTime.Now,
                    Excluido = false
                },
                Telefone = "11999998888"
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;
            objEstabelecimento.Telefone = objEstabelecimentoTeste.Telefone;
            objEstabelecimento.Categoria = objEstabelecimentoTeste.Categoria;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.Equal(objEstabelecimentoTeste.Categoria, objEstabelecimento.Categoria);
            Assert.Equal(objEstabelecimentoTeste.Telefone, objEstabelecimento.Telefone);
            Assert.True(isValid);

            //Esse teste deverá ser APROVADO
        }

        [Fact]
        public void Estabelecimento_Telefone_Nao_Obrigatorio_Valido()
        {
            var idCat = Guid.NewGuid();
            var objEstabelecimentoTeste = new
            {
                IdCategoria = idCat,
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "61426657000100",
                Categoria = new Categoria
                {
                    Id = idCat,
                    Nome = "Oficina",
                    DataCriacao = DateTime.Now,
                    DataEdicao = DateTime.Now,
                    Excluido = false
                }
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;
            objEstabelecimento.Categoria = objEstabelecimentoTeste.Categoria;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.Equal(objEstabelecimentoTeste.Categoria, objEstabelecimento.Categoria);
            Assert.True(isValid);

            //Esse teste deverá ser APROVADO
        }

        [Fact]
        public void Estabelecimento_Telefone_Obrigatorio_Invalido()
        {
            var idCat = Guid.NewGuid();
            var objEstabelecimentoTeste = new
            {
                IdCategoria = idCat,
                RazaoSocial = "Razão Social de Teste",
                CNPJ = "61426657000100",
                Categoria = new Categoria
                {
                    Id = idCat,
                    Nome = "Supermercado",
                    DataCriacao = DateTime.Now,
                    DataEdicao = DateTime.Now,
                    Excluido = false
                }
            };

            var objEstabelecimento = new Estabelecimento();
            objEstabelecimento.RazaoSocial = objEstabelecimentoTeste.RazaoSocial;
            objEstabelecimento.CNPJ = objEstabelecimentoTeste.CNPJ;
            objEstabelecimento.Categoria = objEstabelecimentoTeste.Categoria;

            var context = new ValidationContext(objEstabelecimento, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(objEstabelecimento, context, results, true);

            Assert.NotNull(objEstabelecimento);
            Assert.Equal(objEstabelecimentoTeste.RazaoSocial, objEstabelecimento.RazaoSocial);
            Assert.Equal(objEstabelecimentoTeste.CNPJ, objEstabelecimento.CNPJ);
            Assert.Equal(objEstabelecimentoTeste.Categoria, objEstabelecimento.Categoria);
            Assert.True(isValid);

            //Esse teste deverá ser REPROVADO
        }
    }
}
