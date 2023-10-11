using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class Client
    {
        public Client()
        {
            Cancelacions = new HashSet<Cancelacion>();
            OrderDetails = new HashSet<OrderDetail>();
            Orders = new HashSet<Order>();
        }

        public int IdClient { get; set; }
        public string? CodeClient { get; set; }
        public string? ClientName { get; set; }
        public string? ContactAliases { get; set; }
        public string? Rfc { get; set; }
        public string? AddressClient { get; set; }
        public string? TypeCredit { get; set; }
        public string? TypeClient { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual ICollection<Cancelacion> Cancelacions { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
