using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fitcard_estabelecimentos.domain.Domain
{
    public class Categoria
    {
        [Key]
        [Display(Name = "Código Interno")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O Nome é requerido")]
        [Display(Name = "Nome")]
        [StringLength(100, ErrorMessage = "O valor em Nome deve possuir no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Últ. Data de Edição")]
        public DateTime DataEdicao { get; set; }

        public bool Excluido { get; set; }

        public virtual List<Estabelecimento> Estabelecimentos { get; set; }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}