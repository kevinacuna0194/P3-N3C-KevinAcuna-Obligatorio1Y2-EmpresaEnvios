using Enum;
using LogicaNegocio.EntidadesDominio;
using System.ComponentModel.DataAnnotations;

namespace CasosUso.DTOs
{
    public class EnvioDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa un número de tracking válido.")]
        public string NumeroTracking { get; set; }
        public int EmpleadoId { get; set; }
        public string? NombreEmpleado { get; set; }
        public int ClienteId { get; set; }
        public string? NombreCliente { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa un peso válido mayor a 0.")]
        [Range(0.01, 999999.99)]
        public decimal Peso { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public EstadoEnvio Estado { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Selecciona una agencia.")]
        public int? AgenciaId { get; set; }
        public string? NombreAgencia { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa la dirección postal.")]
        public string? DireccionPostal { get; set; }
        public bool? EntregaEficiente { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa el correo electrónico del cliente.")]
        [EmailAddress(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> El formato del correo no es válido.")]
        public string? EmailCliente { get; set; }
        public string? Comentario { get; set; }
        public IEnumerable<SeguimientoDTO>? Seguimientos { get; set; }
        public TipoEnvio Tipo { get; set; }

        public decimal ConvertirPesoAGramos(decimal pesoEnKilogramos)
        {
            return Peso = pesoEnKilogramos * 1000m;
        }

        public decimal ConvertirPesoAKilogramos()
        {
            return Peso = Peso / 1000m;
        }
    }
}