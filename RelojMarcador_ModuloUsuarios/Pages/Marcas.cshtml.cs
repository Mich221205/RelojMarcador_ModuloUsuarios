using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // AJAX para validar y devolver áreas
        public async Task<JsonResult> OnPostValidar(string identificacion, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(identificacion) || string.IsNullOrWhiteSpace(contrasena))
                return new JsonResult(new { success = false, message = "Faltan credenciales" });

            var valido = await _service.ValidarFuncionario(identificacion, contrasena);
            if (!valido)
                return new JsonResult(new { success = false, message = "Credenciales inválidas" });

            var areas = await _service.ObtenerAreasPorIdentificacion(identificacion);

            return new JsonResult(new
            {
                success = true,
                areas = areas.Select(a => new { id = a.Id_Area, nombre = a.Nombre_Area })
            });
        }

        // POST para registrar marca
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Identificacion) || string.IsNullOrWhiteSpace(Contrasena))
                {
                    ViewData["ModalType"] = "warning";
                    ViewData["ModalTitle"] = "Atención";
                    ViewData["ModalMessage"] = "Debe ingresar usuario y contraseña.";
                    return Page();
                }

                var valido = await _service.ValidarFuncionario(Identificacion, Contrasena);
                if (!valido)
                {
                    ViewData["ModalType"] = "error";
                    ViewData["ModalTitle"] = "Error";
                    ViewData["ModalMessage"] = "Credenciales inválidas.";
                    return Page();
                }

                if (IdAreaSeleccionada <= 0)
                {
                    ViewData["ModalType"] = "warning";
                    ViewData["ModalTitle"] = "Advertencia";
                    ViewData["ModalMessage"] = "Debe seleccionar un área.";
                    return Page();
                }

                if (TipoMarca != "Entrada" && TipoMarca != "Salida")
                {
                    ViewData["ModalType"] = "warning";
                    ViewData["ModalTitle"] = "Advertencia";
                    ViewData["ModalMessage"] = "Debe seleccionar un tipo de marca.";
                    return Page();
                }

                var idMarca = await _service.RegistrarMarca(Identificacion, IdAreaSeleccionada, Detalle, TipoMarca);
                var horaServidor = DateTime.Now.ToString("HH:mm:ss");

                ViewData["ModalType"] = "success";
                ViewData["ModalTitle"] = "Éxito";
                ViewData["ModalMessage"] = $"Marca registrada correctamente (ID={idMarca}) a las {horaServidor}.";
            }
            catch (Exception ex)
            {
                ViewData["ModalType"] = "error";
                ViewData["ModalTitle"] = "Error inesperado";
                ViewData["ModalMessage"] = ex.Message;
            }

            return Page();
        }
    }
}


