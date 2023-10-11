using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class Transportist
    {
        public Transportist()
        {
            Orders = new HashSet<Order>();
        }

        public int IdTransportist { get; set; }
        public string? NameTransportist { get; set; }
        public string? Age { get; set; }
        public string? License { get; set; }
        public string? TypeLicense { get; set; }
        public string? Phone { get; set; }
        public string? UrlFoto { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
