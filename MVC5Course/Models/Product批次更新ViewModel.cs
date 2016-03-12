using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
    public class Product批次更新ViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        //{0}顯示Price
        [Range(1,99999, ErrorMessage = "正確範圍介於{1}~{2}之間")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        //{0}顯示Stock
        [Range(0,9999, ErrorMessage = "正確範圍介於{1}~{2}之間")]
        public Nullable<decimal> Stock { get; set; }



    }
}