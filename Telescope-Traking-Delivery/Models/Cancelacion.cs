using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class Cancelacion
    {
        public int IdCancelation { get; set; }
        public int? IdClient { get; set; }
        public int? IdOrders { get; set; }
        public string? Entrega { get; set; }
        public string? Factura { get; set; }
        public string? Pedido { get; set; }
        public string? Sku { get; set; }
        public string? Producto { get; set; }
        public string? Lote { get; set; }
        public string? Caducidad { get; set; }
        public string? Motivo { get; set; }
        public string? OriginalResto { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Client? IdClientNavigation { get; set; }
        public virtual Order? IdOrdersNavigation { get; set; }
    }
}
