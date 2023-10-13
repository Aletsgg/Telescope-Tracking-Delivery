using Microsoft.AspNetCore.Mvc.Rendering;

namespace Telescope_Traking_Delivery.Models.viewModels
{
    public class OrderDetailVM
    {

        public OrderDetail? oOrderDetail { get; set; }
        public List<SelectListItem>? oListaClient { get; set; }
        public List<SelectListItem>? oListaOrder { get; set; }

        public string? IdOrderDetails { get; set; }
        public string? IdClient { get; set; }
        public string? IdOrders { get; set; }
        public string? DtPedido { get; set; }
        public string? Boxes { get; set; }
        public string? Pice { get; set; }
        public string? Fragile { get; set; }
        public string? Observation { get; set; }
        public string? DtDelivery { get; set; }
        public string? Status { get; set; }
        public string? TotalCost { get; set; }
        public string? EsActivo { get; set; }
        public string? FechaRegistro { get; set; }
    }
}
