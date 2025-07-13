using Enum;
using LogicaNegocio.EntidadesDominio;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioEnvio : IRepositorio<Envio>
    {
        // Métodos específicos para el repositorio de envíos
        void Add(Envio envio, int idUsuarioLogueado);
        void Update(Envio envio, int idUsuarioLogueado);
        void Remove(int id, int idUsuarioLogueado);
        Envio FindById(int id, TipoEnvio tipoEnvio);
        IEnumerable<Envio> FindAll(TipoEnvio tipoEnvio);
        IEnumerable<EnvioComun> ObtenerEnviosComunes();
        IEnumerable<EnvioUrgente> ObtenerEnviosUrgentes();
        bool Existe(int id);
        void FinalizarEnvio(Envio envio, int idUsuarioLogueado);
        void ActualizarEstado(Envio envio, int idUsuarioLogueado, string comentario);
        Envio ObtenerEnvioPorNumeroTracking(string numeroTracking);
        IEnumerable<Envio> ObtenerEnviosPorCliente(int idCliente);
        IEnumerable<Envio> ObtenerEnviosPorEmpleado(int idEmpleado);
        IEnumerable<Envio> ObtenerEnviosPorEstado(EstadoEnvio estado);
        IEnumerable<Envio> ObtenerEnviosPorFecha(DateTime fecha);
        IEnumerable<Envio> ObtenerEnviosPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Envio> ObtenerEnviosPorTipo(TipoEnvio tipoEnvio);
        IEnumerable<Envio> ObtenerEnviosPorPeso(decimal pesoMinimo, decimal pesoMaximo);
        IEnumerable<Envio> ObtenerEnviosPorDireccion(string direccion);
        IEnumerable<Envio> ObtenerEnviosPorPrioridad(string prioridad);
        IEnumerable<Envio> ObtenerEnviosPorTipoYEstado(TipoEnvio tipoEnvio, EstadoEnvio estado);
        IEnumerable<Envio> ObtenerEnviosPorTipoYFecha(TipoEnvio tipoEnvio, DateTime fecha);
        IEnumerable<Envio> ObtenerEnviosPorTipoYRangoFechas(TipoEnvio tipoEnvio, DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Envio> ObtenerEnviosPorEstadoYFecha(EstadoEnvio estado, DateTime fecha);
        IEnumerable<Envio> ObtenerEnviosPorEstadoYRangoFechas(EstadoEnvio estado, DateTime fechaInicio, DateTime fechaFin, int idCliente);
        IEnumerable<Envio> ObtenerEnviosPorClienteYEstado(int idCliente, EstadoEnvio estado);
        IEnumerable<Envio> ObtenerEnviosPorClienteYFecha(int idCliente, DateTime fecha);
        IEnumerable<Envio> ObtenerEnviosPorClienteYRangoFechas(int idCliente, DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Envio> ObtenerEnviosPorEmpleadoYEstado(int idEmpleado, EstadoEnvio estado);
        IEnumerable<Envio> ObtenerEnviosPorEmpleadoYFecha(int idEmpleado, DateTime fecha);
        IEnumerable<Envio> ObtenerEnviosPorEmpleadoYRangoFechas(int idEmpleado, DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Envio> ObtenerEnviosPorComentario(string comentario, int idCliente);
    }
}
