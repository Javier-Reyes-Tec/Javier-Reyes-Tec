using System;
using System.Text.Json.Serialization; // 👈 1. IMPORTANTE: Agregamos esta librería

namespace GymControl.Web.Models
{
    //Definimos los estados posibles usando un Enumerador
    public enum EstadoMembresia
    {
        Activa,
        ProximoAVencer,
        Vencida
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }

        [JsonIgnore] // 👈 2. El toque de Arquitectura: Evita que interfiera con el archivo JSON
        //Propiedad Estado, calculada: No se guarda en BD, se procesa en tiempo real
        public EstadoMembresia Estado
        {
            get
            {
                DateTime dtHoy = DateTime.Today;
                if (FechaVencimiento < dtHoy)
                {
                    return EstadoMembresia.Vencida;
                }

                int iDiasRestantes = (FechaVencimiento - dtHoy).Days;
                if (iDiasRestantes <= 3)
                {
                    return EstadoMembresia.ProximoAVencer;
                }

                return EstadoMembresia.Activa;
            }
        }
    }
}
