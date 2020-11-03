
using fitcard_estabelecimentos.domain.Domain.Enums;
using fitcard_estabelecimentos.domain.Domain.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace fitcard_estabelecimentos.domain.Domain
{
    public class Estabelecimento
    {
        [Key]
        [Display(Name="Código Interno")]
        public Guid Id { get; set; }

        public Guid IdCategoria { get; set; }

        [Required(ErrorMessage = "A Razão Social é requerida")]
        [Display(Name = "Razão Social")]
        [StringLength(255, ErrorMessage = "O valor em Razão Social deve possuir até 255 caracteres")]
        public string RazaoSocial { get; set; }
        
        [Display(Name = "Nome Fantasia")]
        [StringLength(255, ErrorMessage = "O valor em Nome Fantasia deve possuir até 255 caracteres")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "O CNPJ é requerido")]
        [Display(Name = "CNPJ")]
        [StringLength(14, ErrorMessage = "O valor em CNPJ deve possuir 14 dígitos")] 
        [CustomValidationCNPJ(ErrorMessage = "O valor em CNPJ deve ser um valor válido")]
        public string CNPJ { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(100, ErrorMessage = "O valor em E-mail deve possuir até 100 caracteres")]
        [CustomValidationEmail(ErrorMessage = "Caso haja algum valor em E-mail, deve ser um valor válido")]
        public string Email { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(255, ErrorMessage = "O valor em Endereço deve possuir até 255 caracteres")]
        public string Endereco { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(50, ErrorMessage = "O valor em Cidade deve possuir até 50 caracteres")]
        public string Cidade { get; set; }

        [Display(Name = "Estado")]
        [StringLength(2, ErrorMessage = "O valor em Estado deve possuir 2 caracteres")]
        public string Estado { get; set; }

        [CustomRequiredIf("Categoria", "Supermercado", "Caso a Categoria seja Supermercado, o Telefone é requerido")]
        [Display(Name = "Telefone")]
        [StringLength(11, ErrorMessage = "O valor em Telefone deve possuir até 11 caracteres")]
        public string Telefone { get; set; }

        [Display(Name = "Data de Cadastro")]
        [DataType(DataType.DateTime, ErrorMessage = "O valor em Data de Cadastro deve ser válido")]
        public DateTime? DataCadastro { get; set; }

        [Display(Name = "Status")]
        public Status? Status { get; set; }

        [Display(Name = "Agência")]
        [StringLength(4, ErrorMessage = "O valor em Agência deve possuir 4 dígitos")]
        public string Agencia { get; set; }
        
        [Display(Name = "Conta")]
        [StringLength(6, ErrorMessage = "O valor em Conta deve possuir 6 dígitos")]
        
        public string Conta { get; set; }
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Últ. Data de Edição")]
        public DateTime DataEdicao { get; set; }

        public bool Excluido { get; set; }

        public virtual Categoria Categoria { get; set; }
    }
}