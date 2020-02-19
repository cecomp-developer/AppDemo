using AppDemo.DAL.Interfaces;
using AppDemo.Models;

namespace AppDemo.DAL.Repository
{
    public class UsuarioCsvRepository : GenericRepository<UsuarioCSV>, IUsuarioCsvRepository
    {
        public UsuarioCsvRepository(UsuariosDbContext context)
            : base(context)
        {

        }
    }
}
