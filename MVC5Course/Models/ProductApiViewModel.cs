using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
    public class ProductApiViewModel
    {
        [Required]
        public int id{ get; set; }

        [StringLength(80, ErrorMessage = "欄位長度不得大於 80 個字元")]       
        public string name { get; set; }


    }
}