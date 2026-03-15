using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YeniKitapKirtasiyeWebApp.Models
{
    public class Manager
    {
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength:50, ErrorMessage ="En fazla 50 karakter olmalıdır")]
        public string Name { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "En fazla 50 karakter olmalıdır")]
        public string Surname { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 150, ErrorMessage = "En fazla 150 karakter olmalıdır")]
        public string Mail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsActive { get; set; }
    }
}