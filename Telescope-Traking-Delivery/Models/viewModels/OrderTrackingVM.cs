using Microsoft.AspNetCore.Mvc.Rendering;

namespace Telescope_Traking_Delivery.Models.viewModels
{
    public class OrderTrackingVM
    {

        public OrderTracking? oOrderTracking { get; set; }
        public List<SelectListItem>? oListaOrder { get; set; }


        public string? IdOrderTracking { get; set; }
        public string? IdOrders { get; set; }
        public string? DtUpdate { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public string? Observation { get; set; }
        public string? FechaRegistro { get; set; }
    }
}
