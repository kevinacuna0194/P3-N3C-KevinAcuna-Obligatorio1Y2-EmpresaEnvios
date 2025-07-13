namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorio<T>
    {
        void Add(T entidad);
        void Update(T entidad);
        void Remove(int id);
        T FindById(int id);
        List<T> FindAll();
    }
}
