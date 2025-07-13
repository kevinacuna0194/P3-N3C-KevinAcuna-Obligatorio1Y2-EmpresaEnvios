using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarUsuarioPorEmailYPassword : IBuscarUsuarioPorEmailYPassword
    {
        public IRepositorioUsuario RepositorioUsuario { get; set; }

        public BuscarUsuarioPorEmailYPassword(IRepositorioUsuario repositorioUsuario)
        {
            RepositorioUsuario = repositorioUsuario;
        }

        public UsuarioDTO Buscar(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new DatosInvalidosException("El email y la contraseña no pueden estar vacíos.");
            }

            Usuario usuario = RepositorioUsuario.ObtenerUsuarioPorEmailYPassword(email, password);

            if (usuario is null)
            {
                throw new DatosInvalidosException("No se encontró un usuario con el email y la contraseña proporcionados.");
            }

            UsuarioDTO usuarioDTO = MapeadorUsuario.MapearUsuarioDTO(usuario);

            if (usuarioDTO is null)
            {
                throw new DatosInvalidosException("Error al mapear el usuario a DTO.");
            }

            return usuarioDTO;
        }
    }
}
