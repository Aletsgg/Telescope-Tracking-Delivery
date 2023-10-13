using Microsoft.AspNetCore.Mvc.Rendering;

namespace Telescope_Traking_Delivery.Models.ViewModels
{
    public class CancelacionVM 
    {
        public Cancelacion? oCancelacion { get; set; }


        public List<SelectListItem>? oListaClient { get; set; }

        public List<SelectListItem>? oListaOrder { get; set; }


        public string? IdCancelation { get; set; }
        public string? IdClient { get; set; }
        public string? IdOrders { get; set; }
        public string? Entrega { get; set; }
        public string? Factura { get; set; }
        public string? Pedido { get; set; }
        public string? Sku { get; set; }
        public string? Producto { get; set; }
        public string? Lote { get; set; }
        public string? Caducidad { get; set; }
        public string? Motivo { get; set; }
        public string? OriginalResto { get; set; }
        public string? FechaRegistro { get; set; }

    }
}
