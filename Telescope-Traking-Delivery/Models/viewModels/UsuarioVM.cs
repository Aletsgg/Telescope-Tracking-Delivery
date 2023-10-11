using Microsoft.AspNetCore.Mvc.Rendering;

namespace Telescope_Traking_Delivery.Models.ViewModels
{
    public class UsuarioVM
    {
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? IdRol { get; set; }
        public string? UrlFoto { get; set; }
        public string? NombreFoto { get; set; }
        public string? Clave { get; set; }
        public string? EsActivo { get; set; }
        public string? FechaRegistro { get; set; }


        public Usuario oUsuario { get; set; }
        public List<SelectListItem> oListaRol { get; set; }
    }


}
