using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppDemo.DAL.Interfaces;
using AppDemo.Models;
using AppDemo.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDemo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _usuario;
        public UsuariosController(IUsuarioRepository usuario)
        {
            _usuario = usuario;
        }

        // GET: Usuarios
        public ActionResult Index()
        {
            IEnumerable<Usuario> usuarioModel = _usuario.Query().AsNoTracking().ToArray();
            return View(usuarioModel);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            Usuario usuario = _usuario.Query().AsNoTracking().FirstOrDefault(q => q.IdUsuario == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("IdUsuario", "Nombre", "Telefono", "Correo", "User", "PasswordHash")] Usuario model)
        {
            if(ModelState.IsValid)
            {
                string generaSalt = Hash.GenerarSalString();
                model.PasswordSalt = generaSalt;
                model.PasswordHash = Hash.GenerarHash(model.PasswordHash, generaSalt);
                _usuario.Add(model);
                return RedirectToAction(nameof(HomeController.Index));
            }

            return View(model);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Usuario usuario = _usuario.Find(q => q.IdUsuario == id);
            usuario.PasswordHash = "";

            if (usuario == null)
                return NotFound();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("IdUsuario", "Nombre", "Telefono", "Correo", "User", "PasswordHash")] Usuario model)
        {
            if (id != model.IdUsuario)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    string generaSalt = Hash.GenerarSalString();
                    model.PasswordSalt = generaSalt;
                    model.PasswordHash = Hash.GenerarHash(model.PasswordHash, generaSalt);

                    _usuario.Update(model, id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_usuario.Query().Any(q => q.IdUsuario == id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(HomeController.Index));
            }
            return View(model);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Usuario user = _usuario.Query().FirstOrDefault(q => q.IdUsuario == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int? id)
        {
            Usuario user = _usuario.Find(q => q.IdUsuario == id);

            if (user == null)
                return NotFound();

            _usuario.Delete(user);

            return RedirectToAction(nameof(HomeController.Index));
        }
    }
}