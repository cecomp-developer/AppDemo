using AppDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDemo.DAL.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Usuario Autenticar(string username, string pasword);
    }
}
