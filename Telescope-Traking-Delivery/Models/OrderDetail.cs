using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class OrderDetail
    {
        public int IdOrderDetails { get; set; }
        public int? IdClient { get; set; }
        public int? IdOrders { get; set; }
        public DateTime? DtPedido { get; set; }
        public int? Boxes { get; set; }
        public int? Pice { get; set; }
        public string? Fragile { get; set; }
        public string? Observation { get; set; }
        public DateTime? DtDelivery { get; set; }
        public string? Status { get; set; }
        public string? TotalCost { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Client? IdClientNavigation { get; set; }
        public virtual Order? IdOrdersNavigation { get; set; }
    }
}
