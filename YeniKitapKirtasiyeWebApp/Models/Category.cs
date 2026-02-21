using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YeniKitapKirtasiyeWebApp.Models
{
    public class Category
    {
        //Eğer PropertyAdı ID ise Entity otomatik olarak Bu kolonu PrimaryKey yapar Identity Specification Uygular
        //[Key]
        public int ID { get; set; }

        [Display(Name="Kategori Adı")]
        [Required(ErrorMessage = "Kategori Adı zorunludur")]
        [StringLength(maximumLength:60, ErrorMessage ="En fazla 50 karakter olmalıdır")]
        public string Name { get; set; }

        [Display(Name = "Aktif mi")]
        public bool IsActive { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}