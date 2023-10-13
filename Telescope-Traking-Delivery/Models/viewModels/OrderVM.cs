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





        public string? IdOrders { get; set; }
        public string? SiteLoading { get; set; }
        public string? OrdersDelivery { get; set; }
        public string? DtAppointedForShipment { get; set; }
        public string? DtOfBoarding { get; set; }
        public string? DtOfFarrivalOfUnit { get; set; }
        public string? OtOfBoarding { get; set; }
        public string? DtOfChargingStart { get; set; }
        public string? DtLoadingEnd { get; set; }
        public string? OtOfChargingStart { get; set; }
        public string? DtOfDepartureFromSite { get; set; }
        public string? VehicleControl { get; set; }
        public string? Destination { get; set; }
        public string? DtDeliveryAppointment { get; set; }
        public string? Pieces { get; set; }
        public string? Boxes { get; set; }
        public string? DtArrivalToUnload { get; set; }
        public string? OnTime { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? Obsevations { get; set; }
        public string? ShippingObservations { get; set; }
        public string? IncidentOfArrivalCargoTr { get; set; }
        public string? IncidentOfArrivalClientTr { get; set; }
        public string? IncidentInShipmentWh { get; set; }
        public string? ExtraRouteIndicator { get; set; }
        public string? AccidentInRoute { get; set; }
        public string? RoadTimeIndicators { get; set; }
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
