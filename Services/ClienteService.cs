#region FASE 1: ANTES DE PROGRAMAR BOTONES DE ACCIÓN COMO GUARDAR CLIENTES
/*
using System;
using System.Collections.Generic;
using GymControl.Web.Models;

namespace GymControl.Web.Services
{
    public class ClienteService
    {
        //Lista en memoria estática para simular nuestra BD temporal
        private static List<Cliente> _clientes = new List<Cliente>
        {
            new Cliente{Id = 1, NombreCompleto = "Carlos Mendoza", Correo = "carlos@mail.com", FechaVencimiento = DateTime.Today.AddDays(15)},
            new Cliente{Id = 2, NombreCompleto = "Ana María Gómez", Correo = "ana@mail.com", FechaVencimiento = DateTime.Today.AddDays(2)},
            new Cliente{Id = 3, NombreCompleto = "Jorge Luis Ramos", Correo = "jorge@mail.com", FechaVencimiento = DateTime.Today.AddDays(-3)},
            new Cliente{Id = 4, NombreCompleto = "Lucía Fernández", Correo = "lucia@mail.com", FechaVencimiento = DateTime.Today.AddDays(25)},
        };

        //Método para obtener todos los miembros
        public List<Cliente> ObtenerTodos()
        {
            return _clientes;
        }
    }
}*/
#endregion FASE 1

#region FASE 2: PROGRAMACIÓN DE BOTONES

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GymControl.Web.Models;

namespace GymControl.Web.Services
{
    public class ClienteService
    {
        // Definimos la ruta donde se guardará el archivo de texto en el servidor
        private readonly string _rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clientes.json");

        // Método Senior: Lee los datos directamente desde el archivo JSON
        public List<Cliente> ObtenerTodos()
        {
            // Si el archivo aún no existe, creamos uno inicial con datos de prueba
            if (!File.Exists(_rutaArchivo))
            {
                var datosIniciales = GenerarDatosDePrueba();
                GuardarEnArchivo(datosIniciales);
                return datosIniciales;
            }

            try
            {
                // Leemos todo el texto del archivo JSON
                string jsonString = File.ReadAllText(_rutaArchivo);
                // Lo transformamos (deserializamos) de texto a una Lista de C#
                return JsonSerializer.Deserialize<List<Cliente>>(jsonString) ?? new List<Cliente>();
            }
            catch
            {
                // Enfoque preventivo: Si el archivo se corrompe, devolvemos una lista vacía para no romper la app
                return new List<Cliente>();
            }
        }

        // Método para agregar un nuevo cliente y persistirlo
        public void Agregar(string nombre, string correo)
        {
            var clientes = ObtenerTodos();

            int nuevoId = clientes.Count > 0 ? clientes[clientes.Count - 1].Id + 1 : 1;

            var nuevoCliente = new Cliente
            {
                Id = nuevoId,
                NombreCompleto = nombre,
                Correo = correo,
                FechaVencimiento = DateTime.Today.AddDays(30)
            };

            clientes.Add(nuevoCliente);

            // Guardamos la lista actualizada de vuelta en el archivo JSON
            GuardarEnArchivo(clientes);
        }

        // Método para renovar la membresía y persistir el cambio
        public void RenovarMembresia(int id)
        {
            var clientes = ObtenerTodos();
            var cliente = clientes.Find(c => c.Id == id);

            if (cliente != null)
            {
                cliente.FechaVencimiento = DateTime.Today.AddDays(30);
                // Guardamos los cambios de la renovación en el disco duro
                GuardarEnArchivo(clientes);
            }
        }

        // 🧠 Métodos de soporte privados para manejo de archivos e infraestructura
        private void GuardarEnArchivo(List<Cliente> clientes)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(clientes, opciones);
            File.WriteAllText(_rutaArchivo, jsonString);
        }

        private List<Cliente> GenerarDatosDePrueba()
        {
            return new List<Cliente>
            {
                new Cliente { Id = 1, NombreCompleto = "Carlos Mendoza", Correo = "carlos@mail.com", FechaVencimiento = DateTime.Today.AddDays(15) },
                new Cliente { Id = 2, NombreCompleto = "Ana María Gómez", Correo = "ana@mail.com", FechaVencimiento = DateTime.Today.AddDays(2) },
                new Cliente { Id = 3, NombreCompleto = "Jorge Luis Ramos", Correo = "jorge@mail.com", FechaVencimiento = DateTime.Today.AddDays(-3) },
                new Cliente { Id = 4, NombreCompleto = "Lucía Fernández", Correo = "lucia@mail.com", FechaVencimiento = DateTime.Today.AddDays(25) }
            };
        }
    }
}
#endregion