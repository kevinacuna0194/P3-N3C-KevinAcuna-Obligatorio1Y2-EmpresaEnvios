using CasosUso.DTOs;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.Mapeadores
{
    public class MapeadorRol
    {
        public static Rol MapearRol(RolDTO rolDTO)
        {
            if (rolDTO is null)
            {
                throw new Exception("El Rol no puede ser nulo");
            }

            return new Rol()
            {
                Id = rolDTO.Id,
                Nombre = new NombreRol { Nombre = rolDTO.Nombre }
            };
        }

        public static RolDTO MapearRolDTO(Rol rol)
        {
            if (rol is null)
            {
                throw new Exception("El Rol no puede ser nulo");
            }

            return new RolDTO()
            {
                Id = rol.Id,
                Nombre = rol.Nombre.Nombre
            };
        }

        public static List<RolDTO> MapearListaRolDTO(List<Rol> roles)
        {
            if (roles is null)
            {
                throw new Exception("La lista de Roles no puede ser nula");
            }

            List<RolDTO> listaRolDTO = new List<RolDTO>();

            foreach (var rol in roles)
            {
                listaRolDTO.Add(MapearRolDTO(rol));
            }

            return listaRolDTO;
        }
    }
}
