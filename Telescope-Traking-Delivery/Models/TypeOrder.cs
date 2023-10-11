using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class TypeOrder
    {
        public TypeOrder()
        {
            Orders = new HashSet<Order>();
        }

        public int IdTypeOrders { get; set; }
        public string? Clasification { get; set; }
        public string? DescriptionOrder { get; set; }
        public string? LocalForaneo { get; set; }
        public string? NormalOrFast { get; set; }
        public string? Promotion { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
