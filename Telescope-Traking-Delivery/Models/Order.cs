using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class Order
    {
        public Order()
        {
            Cancelacions = new HashSet<Cancelacion>();
            OrderDetails = new HashSet<OrderDetail>();
            OrderTrackings = new HashSet<OrderTracking>();
        }

        public int IdOrders { get; set; }
        public string? SiteLoading { get; set; }
        public string? OrdersDelivery { get; set; }
        public DateTime? DtAppointedForShipment { get; set; }
        public DateTime? DtOfBoarding { get; set; }
        public DateTime? DtOfFarrivalOfUnit { get; set; }
        public string? OtOfBoarding { get; set; }
        public DateTime? DtOfChargingStart { get; set; }
        public DateTime? DtLoadingEnd { get; set; }
        public string? OtOfChargingStart { get; set; }
        public DateTime? DtOfDepartureFromSite { get; set; }
        public string? VehicleControl { get; set; }
        public string? Destination { get; set; }
        public DateTime? DtDeliveryAppointment { get; set; }
        public int? Pieces { get; set; }
        public int? Boxes { get; set; }
        public DateTime? DtArrivalToUnload { get; set; }
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
        public int? IdClient { get; set; }
        public int? IdTypeOrders { get; set; }
        public int? IdTransport { get; set; }
        public int? IdTransportist { get; set; }
        public DateTime? Confirmation { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Client? IdClientNavigation { get; set; }
        public virtual Transport? IdTransportNavigation { get; set; }
        public virtual Transportist? IdTransportistNavigation { get; set; }
        public virtual TypeOrder? IdTypeOrdersNavigation { get; set; }
        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Cancelacion> Cancelacions { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderTracking> OrderTrackings { get; set; }
    }
}
