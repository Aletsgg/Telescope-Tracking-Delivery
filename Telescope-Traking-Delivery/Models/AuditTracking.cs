using System;
using System.Collections.Generic;

namespace Telescope_Traking_Delivery.Models
{
    public partial class AuditTracking
    {
        public int IdAuditTracking { get; set; }
        public int? IdUsuario { get; set; }
        public string? TableName { get; set; }
        public int? IdRegister { get; set; }
        public DateTime? DtChange { get; set; }
        public string? Action { get; set; }
        public string? Observation { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
