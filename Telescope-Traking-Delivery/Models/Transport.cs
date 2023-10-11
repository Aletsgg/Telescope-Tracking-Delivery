using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class Transport
    {
        public Transport()
        {
            Orders = new HashSet<Order>();
        }

        public int IdTransport { get; set; }
        public string? TransportName { get; set; }
        public string? UnitType { get; set; }
        public string? Plate { get; set; }
        public string? Capacity { get; set; }
        public string? Details { get; set; }
        public bool? Refrigerated { get; set; }
        public bool? EsActivo { get; set; }
        public string? UrlImagen { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
