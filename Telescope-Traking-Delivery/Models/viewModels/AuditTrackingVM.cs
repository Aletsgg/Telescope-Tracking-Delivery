using Microsoft.AspNetCore.Mvc.Rendering;

namespace Telescope_Traking_Delivery.Models.viewModels
{
    public class AuditTrackingVM
    {
        public AuditTracking? oAuditTracking { get; set; }
        public List<SelectListItem>? oListaUsuario { get; set; }


        public string? IdAuditTracking { get; set; }
        public string? IdUsuario { get; set; }
        public string? TableName { get; set; }
        public string? IdRegister { get; set; }
        public string? DtChange { get; set; }
        public string? Action { get; set; }
        public string? Observation { get; set; }
        public string? FechaRegistro { get; set; }
    }
}
