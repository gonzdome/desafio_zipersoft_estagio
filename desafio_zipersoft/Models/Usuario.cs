using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace desafio_zipersoft.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Display(Name="Id")]
        [Column("Id")]
        public int Id { get; set; }
        
        [Display(Name="Nome")]
        [Column("Nome")]
        public string Nome { get; set; } 
        
        [Display(Name="RG")]
        [Column("RG")]
        public string RG { get; set; } 
        
        [Display(Name="IE")]
        [Column("IE")]
        public string IE { get; set; } 

        [Display(Name="CPF")]
        [Column("CPF")]
        public string CPF { get; set; } 

        [Display(Name="CNPJ")]
        [Column("CNPJ")]
        public string CNPJ { get; set; } 
        
        [Display(Name="Endereco")]
        [Column("Endereco")]
        public string Endereco { get; set; } 
        
        [Display(Name="Numero")]
        [Column("Numero")]
        public string Numero { get; set; } 

        [Display(Name="Bairro")]
        [Column("Bairro")]
        public string Bairro { get; set; } 

        [Display(Name="CEP")]
        [Column("CEP")]
        public string CEP { get; set; } 

        [Display(Name="Cidade")]
        [Column("Cidade")]
        public string Cidade { get; set; } 
        
        [Display(Name="Email")]
        [Column("Email")]
        public string Email { get; set; } 
        
        [Display(Name="Site")]
        [Column("Site")]
        public string Site { get; set; }
        
        [Display(Name="Celular")]
        [Column("Celular")]
        public string Celular { get; set; }
        
        [Display(Name="Telefone")]
        [Column("Telefone")]
        public string Telefone { get; set; }   
        
        [Display(Name="Observacao")]
        [Column("Observacao")]
        public string Observacao { get; set; }
        
        [Display(Name="Foto")]
        [Column("Foto")]
        public string Foto { get; set; }


        [NotMapped]
        [DisplayName("Sua Foto")]
        public IFormFile ImageFile { get; set; }
    }
}
