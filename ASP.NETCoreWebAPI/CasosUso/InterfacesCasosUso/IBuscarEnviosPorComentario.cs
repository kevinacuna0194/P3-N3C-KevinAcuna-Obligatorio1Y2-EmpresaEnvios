using CasosUso.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarEnviosPorComentario
    {
        IEnumerable<EnvioDTO> Buscar(String comentario, int idCliente);
    }
}
