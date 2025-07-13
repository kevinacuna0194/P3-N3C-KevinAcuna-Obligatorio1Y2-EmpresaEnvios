using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarEnvioPorNumeroTracking
    {
        EnvioDTO Buscar(string numeroTracking);
    }
}
