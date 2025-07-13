using CasosUso.DTOs;
using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarEnviosPorEstadoYRangoFechas
    {
        IEnumerable<EnvioDTO> Buscar(EstadoEnvio estado, DateTime fechaInicio, DateTime fechaFin, int idCliente);
    }
}
