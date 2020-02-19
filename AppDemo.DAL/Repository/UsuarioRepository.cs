using AppDemo.DAL.Interfaces;
using AppDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AppDemo.Security;

namespace AppDemo.DAL.Repository
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly UsuariosDbContext _context;
        public UsuarioRepository(UsuariosDbContext context)
            : base(context)
        {
            _context = context; 
        }

        public Usuario Autenticar(string username, string password)
        {
            Usuario user = this._context.Set<Usuario>().FirstOrDefault(q => q.User == username);

            if (user != null)
            {
                if (Hash.VerificarHashPassword(password, user.PasswordSalt, user.PasswordHash))
                {
                    return user;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
