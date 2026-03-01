using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YeniKitapKirtasiyeWebApp.Areas.ManagerPanel.Data.ViewModels
{
    public class ManagerLoginViewModel
    {
        [Required(ErrorMessage ="Mail adresi zorunludur")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Şifre adresi zorunludur")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}