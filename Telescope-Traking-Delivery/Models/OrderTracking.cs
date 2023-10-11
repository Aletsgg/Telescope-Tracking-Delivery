using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class OrderTracking
    {
        public int IdOrderTracking { get; set; }
        public int? IdOrders { get; set; }
        public DateTime? DtUpdate { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public string? Observation { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Order? IdOrdersNavigation { get; set; }
    }
}
