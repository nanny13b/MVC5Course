using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models
{
    public class MemberViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage ="請輸入有效日期")]
        public DateTime Birthday { get; set; }
    }
}