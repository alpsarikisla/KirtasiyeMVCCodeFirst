using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YeniKitapKirtasiyeWebApp.Models.ViewModels
{
    public class OdeViewModel
    {
        [Required(ErrorMessage ="*")]
        public string cardname { get; set; }

        [Required(ErrorMessage = "*")]
        public string cardnumber { get; set; }

        [Required(ErrorMessage = "*")]
        public string expiry { get; set; }

        [Required(ErrorMessage = "*")]
        public string cvv { get; set; }
    }
}