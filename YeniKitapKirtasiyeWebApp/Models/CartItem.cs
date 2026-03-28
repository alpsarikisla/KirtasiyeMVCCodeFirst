using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YeniKitapKirtasiyeWebApp.Models
{
    public class CartItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }

    }
}