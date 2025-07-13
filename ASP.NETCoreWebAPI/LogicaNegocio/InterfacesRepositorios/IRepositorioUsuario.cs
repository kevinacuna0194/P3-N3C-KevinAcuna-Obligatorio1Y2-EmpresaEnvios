using LogicaNegocio.EntidadesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        // Métodos específicos para el repositorio de usuarios
        void Add(Usuario usuario, int idUsuarioLogueado);
        void Update(Usuario usuario, int idUsuarioLogueado);
        void Remove(int id, int idUsuarioLogueado);
        Usuario ObtenerUsuarioPorNombre(string nombre);
        Usuario ObtenerUsuarioPorEmail(string email);
        Usuario ObtenerUsuarioPorDocumentoIdentidad(int documentoIdentidad);
        List<Usuario> ObtenerUsuariosPorRol(Rol rol);
        Usuario ObtenerUsuarioPorEmailYPassword(string email, string password);
        bool Existe(int id);
    }
}
