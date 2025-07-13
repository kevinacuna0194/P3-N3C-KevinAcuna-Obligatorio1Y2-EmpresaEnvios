using CasosUso.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarSeguimientosPorCliente
    {
        IEnumerable<SeguimientoDTO> Buscar(int clienteId);
    }
}
