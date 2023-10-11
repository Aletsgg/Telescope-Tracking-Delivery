using Microsoft.AspNetCore.Mvc.Rendering;

namespace Telescope_Traking_Delivery.Models.ViewModels
{
    public class OrderVM
    {

        public Order oOrder { get; set; }



        public List<SelectListItem> oListaClient { get; set; }
        public List<SelectListItem> oListaTypeOrder { get; set; }
        public List<SelectListItem> oListaTransport { get; set; }
        public List<SelectListItem> oListaTransportist { get; set; }
        public List<SelectListItem> oListaUsuarios{ get; set; }




        public string? SiteLoading { get; set; }
        public string? OrdersDelivery { get; set; }
        public string? DtAppointedForShipment { get; set; }
        public string? DtOfBoarding { get; set; }
        public string? OtOfBoarding { get; set; }
        public string? VehicleControl { get; set; }
        public string? Destination { get; set; }
        public string? DtDeliveryAppointment { get; set; }
        public string? Pieces { get; set; }
        public string? DtArrivalToUnload { get; set; }
        public string? OnTime { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? Obsevations { get; set; }
        public string? IdClient { get; set; }
        public string? IdTypeOrders { get; set; }
        public string? IdTransport { get; set; }
        public string? IdTransportist { get; set; }
        public string? Confirmation { get; set; }
        public string? EndDateTime { get; set; }
        public string? IdUsuario { get; set; }
        public string? FechaRegistro { get; set; }



    }
}
