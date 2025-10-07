using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;
using RelojMarcador_ModuloUsuarios.Services;

namespace RelojMarcador_ModuloUsuarios.Pages
{
    public class MarcasModel : PageModel
    {
        private readonly IMarcaService _service;

        public MarcasModel(IMarcaService service)
        {
            _service = service;
            Areas = new List<SelectListItem>();
        }

        [BindProperty] public string Identificacion { get; set; } = "";
        [BindProperty] public string Contrasena { get; set; } = "";
        [BindProperty] public int IdAreaSeleccionada { get; set; }
        [BindProperty] public string TipoMarca { get; set; } = "";
        [BindProperty] public string Detalle { get; set; } = "";

        public List<SelectListItem> Areas { get; set; }
        public string Mensaje { get; set; } = "";

        public async Task<JsonResult> OnPostValidar(string identificacion, string contrasena)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(identificacion) || string.IsNullOrWhiteSpace(contrasena))
                    return new JsonResult(new { success = false, message = "Faltan credenciales." });

                var valido = await _service.ValidarFuncionario(identificacion, contrasena);
                if (!valido)
                    return new JsonResult(new { success = false, message = "Credenciales inválidas." });

                var areas = await _service.ObtenerAreasPorIdentificacion(identificacion);

                return new JsonResult(new
                {
                    success = true,
                    areas = areas.Select(a => new { id = a.Id_Area, nombre = a.Nombre_Area })
                });
            }
            catch (MySqlException ex)
            {
                return new JsonResult(new { success = false, message = $"Error de base de datos: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error inesperado: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Identificacion) || string.IsNullOrWhiteSpace(Contrasena))
                {
                    MostrarModal("warning", "Atención", "Debe ingresar usuario y contraseña.");
                    return Page();
                }

                var valido = await _service.ValidarFuncionario(Identificacion, Contrasena);
                if (!valido)
                {
                    MostrarModal("error", "Error", "Credenciales inválidas.");
                    return Page();
                }

                if (IdAreaSeleccionada <= 0)
                {
                    MostrarModal("warning", "Advertencia", "Debe seleccionar un área.");
                    return Page();
                }

                if (TipoMarca != "Entrada" && TipoMarca != "Salida")
                {
                    MostrarModal("warning", "Advertencia", "Debe seleccionar un tipo de marca.");
                    return Page();
                }

                // Registrar marca
                var idMarca = await _service.RegistrarMarca(Identificacion, IdAreaSeleccionada, Detalle, TipoMarca);
                var horaServidor = DateTime.Now.ToString("HH:mm:ss");

                MostrarModal("success", "Éxito", $"Marca registrada correctamente (ID={idMarca}) a las {horaServidor}.");
            }
            catch (MySqlException ex)
            {
                MostrarModal("error", "Error de Base de Datos", $"Ocurrió un problema al registrar la marca: {ex.Message}");
            }
            catch (Exception ex)
            {
                MostrarModal("error", "Error inesperado", $"Ocurrió un error inesperado: {ex.Message}");
            }

            return Page();
        }

        private void MostrarModal(string tipo, string titulo, string mensaje)
        {
            ViewData["ModalType"] = tipo;
            ViewData["ModalTitle"] = titulo;
            ViewData["ModalMessage"] = mensaje;
        }
    }
}


