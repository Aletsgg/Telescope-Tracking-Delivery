
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Telescope_Traking_Delivery.Models.ViewModels;

namespace Telescope_Traking_Delivery.Controllers
{
    public class StorageController : Controller
    {
        private readonly string cadenaSQL;
        public StorageController(IConfiguration configuration) {
            cadenaSQL = configuration.GetConnectionString("SQLConexion");

        }
        
        public IActionResult Index()
        {
            var oListaUsuario = new List<UsuarioVM>();

            using (var con = new SqlConnection(cadenaSQL))
            {
                con.Open();
                var cmd = new SqlCommand("Listar ", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oListaUsuario.Add(new UsuarioVM
                        {
                            Nombre = dr["nombre"].ToString(),
                            Correo = dr["correo"].ToString(),
                            Telefono = dr["telefono"].ToString(),
                            UrlFoto = dr["urlFoto"].ToString(),
                            NombreFoto = dr["nombreFoto"].ToString(),
                            Clave = dr["clave"].ToString(),


                        }) ;

                        
                    }
                }

            }
            return View(oListaUsuario);
        }

        [HttpGet]
        public IActionResult ActualizarStorage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarStorage(UsuarioVM oUsuario, IFormFile imagen)
        {
            Stream Imagen = imagen.OpenReadStream();
            string UrlFoto = await SubirSotage(Imagen, imagen.FileName);

            using(var con = new SqlConnection(cadenaSQL))
            {
                con.Open();
                var cmd = new SqlCommand("Guardar ", con);
                cmd.Parameters.AddWithValue("nombre", oUsuario.Nombre);
                cmd.Parameters.AddWithValue("correo", oUsuario.Clave);
                cmd.Parameters.AddWithValue("telefono", oUsuario.Telefono);
                cmd.Parameters.AddWithValue("urlFoto", oUsuario.UrlFoto);
                cmd.Parameters.AddWithValue("nombreFoto", oUsuario.NombreFoto);
                cmd.Parameters.AddWithValue("clave", oUsuario.Clave);
                cmd.ExecuteNonQuery();
            }


            return RedirectToAction("Index");
        }

        public async Task<string> SubirSotage(Stream archivo, string nombre)
        {

            string email = " garciag.alejandro1@outlook.com";
            string clave = "Ch6YJ#1i)ejE";
            string ruta = "trackerpedidos-d8a48.appspot.com";
            string api_key = "AIzaSyCdtxXk9EiIIGnJVm8KYSwYrvueY1QDkUc";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }
                )
                .Child("Fotos_Perfil")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);
            var downloadURL = await task;

            return downloadURL;
        }
    
    }
}
