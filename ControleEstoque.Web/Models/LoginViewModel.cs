using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o USUÁRIO")]
        [Display(Name = "Usuário:")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Informe a SENHA")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public string Senha { get; set; }
        [Display(Name = "Lembrar Me")]
        public bool LembrarMe { get; set; }
    }
}