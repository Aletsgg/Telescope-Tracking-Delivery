using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Telescope_Traking_Delivery.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Telescope_Traking_Delivery.Models.ViewModels;

namespace Telescope_Traking_Delivery.Controllers
{

    [Authorize]
    public class ClientController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;

        public ClientController(DB_TRACKER2Context context)
        {
            _DBcontext = context;
        }
        //----------------------(GLOBAL CLAIMS)-----------------------------

        // Helper method to get user information from Claims
        private void GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario)
        {
            ClaimsPrincipal ClaimsName = HttpContext.User;
            nombreUsuario = "";
            EmailUser = "";
            UrlFoto = "";
            ROluser = "";
            IdUsuario = "";

            if (ClaimsName.Identity.IsAuthenticated)
            {
                nombreUsuario = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                EmailUser = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Email)
                    .Select(c => c.Value).SingleOrDefault();
                UrlFoto = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.UserData)
                    .Select(c => c.Value).SingleOrDefault();
                ROluser = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).SingleOrDefault();
                IdUsuario = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
                    .Select(c => c.Value).SingleOrDefault();
            }
        }

        //----------------------( INDEX )----------------------
        [Authorize(Roles = "1,2,3")]
        public IActionResult Index()
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;

            //esta propiedad permite convertir algun registro de la lista en "" si este es null
            List<Client> lista = _DBcontext.Clients.ToList();
            lista.ForEach(c =>
            {                
                c.CodeClient = c.CodeClient ?? "";
                c.ClientName = c.ClientName ?? "";
                c.ContactAliases = c.ContactAliases ?? "";
                c.Rfc = c.Rfc ?? "";
                c.AddressClient = c.AddressClient ?? "";
                c.TypeCredit = c.TypeCredit ?? "";
                c.TypeClient = c.TypeClient ?? "";                
                // Continúa para todas las propiedades que quieras convertir
            });
            return View(lista);
        }

        //----------------------( CREAR Y ACTUALIZAR )----------------------
        [Authorize(Roles = "1,2,3")]
        [HttpGet]
        public IActionResult Client_Detalle(int IdClient)
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;

            Client client = new()
            {
            };

            if (IdClient != 0)
            {
                client = _DBcontext.Clients.Find(IdClient);
            }

            return View(client);
        }

        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public IActionResult Client_Detalle(Client client)
        {
            if (client.IdClient == 0)
            {
                _DBcontext.Clients.Add(client);
            }
            else
            {
                _DBcontext.Clients.Update(client);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Index", "Client");

        }


        //----------------------( ELIMINAR )----------------------
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult Eliminar(int IdClient)
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------

            Client client = _DBcontext.Clients.Where(e => e.IdClient == IdClient).FirstOrDefault();

            return View(client);

        }

        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Eliminar(Client client)
        {

            _DBcontext.Clients.Remove(client);
            _DBcontext.SaveChanges();

            return RedirectToAction("Index", "Client");

        }

        //----------------------( CARGAR DATOS EXCEL )----------------------

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult MostrarDatos()
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------

            return View();
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        // Action method to display data from an Excel file
        public IActionResult MostrarDatos([FromForm] IFormFile ArchivoExcel)
        {
            // Open a stream to read the Excel file
            Stream stream = ArchivoExcel.OpenReadStream();

            // Create a workbook object to hold the Excel data
            IWorkbook MiExcel = null;

            // Check the file extension to determine the type of workbook to create
            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                // Create an XSSFWorkbook for .xlsx files
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                // Create an HSSFWorkbook for .xls files
                MiExcel = new HSSFWorkbook(stream);
            }

            // Get the first sheet from the workbook
            ISheet HojaExcel = MiExcel.GetSheetAt(0);

            // Get the number of rows in the sheet
            int cantidadFilas = HojaExcel.LastRowNum;

            // Create a list to hold the data from the sheet
            List<ClientVM> lista = new List<ClientVM>();

            // Loop through each row in the sheet, starting from row 1 (skipping the header row)
            for (int i = 1; i <= cantidadFilas; i++)
            {
                // Get the current row
                IRow fila = HojaExcel.GetRow(i);

                // Add a new ClientVM object to the list with data from the current row
                lista.Add(new ClientVM
                {
                    CodeClient = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    ClientName = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    ContactAliases = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Rfc = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    AddressClient = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    TypeCredit = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    TypeClient = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    FechaRegistro = fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                });
            }

            // Return an OK status code with the list of data as the response body
            return StatusCode(StatusCodes.Status200OK, lista);
        }


        [Authorize(Roles = "1")]
        [HttpPost]
        // Action method to send data from an Excel file to the database
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel)
        {
            // Open a stream to read the Excel file
            Stream stream = ArchivoExcel.OpenReadStream();

            // Create a workbook object to hold the Excel data
            IWorkbook MiExcel = null;

            // Check the file extension to determine the type of workbook to create
            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                // Create an XSSFWorkbook for .xlsx files
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                // Create an HSSFWorkbook for .xls files
                MiExcel = new HSSFWorkbook(stream);
            }

            // Get the first sheet from the workbook
            ISheet HojaExcel = MiExcel.GetSheetAt(0);

            // Get the number of rows in the sheet
            int cantidadFilas = HojaExcel.LastRowNum;

            // Create a list to hold the data from the sheet
            List<Client> lista = new List<Client>();

            // Loop through each row in the sheet, starting from row 1 (skipping the header row)
            for (int i = 1; i <= cantidadFilas; i++)
            {
                // Get the current row
                IRow fila = HojaExcel.GetRow(i);

                // Add a new Client object to the list with data from the current row
                lista.Add(new Client
                {
                    CodeClient = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    ClientName = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    ContactAliases = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Rfc = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    AddressClient = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    TypeCredit = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    TypeClient = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = bool.Parse(fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    FechaRegistro = DateTime.Parse(fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                });
            }

            // Insert the list of data into the database using BulkInsert
            _DBcontext.BulkInsert(lista);

            // Return an OK status code with a success message as the response body
            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}