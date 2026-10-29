using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GymControl.Web.Models;
using GymControl.Web.Services;
using System.Collections.Generic;

namespace GymControl.Web.Pages
{
    public class IndexModel : PageModel
    {
        // Propiedad donde guardaremos los clientes para que la vista pueda leerlos
        public List<Cliente> listaClientes { get; set; } = new List<Cliente>();

        // El método OnGet se ejecuta automáticamente cuando el usuario entra a la Página
        public void OnGet()
        {
            // Instanciamos nuestro servicio y cargamos los datos
            var servicio = new ClienteService();
            listaClientes = servicio.ObtenerTodos();
        }

        // ?? ESTO ES LO QUE FALTABA: El método OnPost procesa el formulario de guardado
        public IActionResult OnPost(string nombre, string correo)
        {
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(correo))
            {
                var servicio = new ClienteService();
                servicio.Agregar(nombre, correo);
            }

            // Redirecciona a la misma página para limpiar el formulario y refrescar la tabla
            return RedirectToPage();
        }

        // Este método procesará las futuras renovaciones desde la tabla
        public IActionResult OnPostRenovar(int id)
        {
            var servicio = new ClienteService();
            servicio.RenovarMembresia(id);

            return RedirectToPage();
        }
    }
}
