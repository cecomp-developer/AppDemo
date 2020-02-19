using AppDemo.DAL.Interfaces;
using AppDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Controllers
{
    [Authorize(Roles = "administrador")]
    public class ArchivosController : Controller
    {
        private readonly IUsuarioCsvRepository _userRepository;

        public ArchivosController(IUsuarioCsvRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View(_userRepository.Query().AsNoTracking().ToArray());
        }

        public async Task<IActionResult> SubirArchivo(IFormFile archivo)
        {
            List<UsuarioCSV> listaUsuarioCsv = new List<UsuarioCSV>();

            try
            {
                if (!(archivo.Length > 0))
                    return NotFound();

                using (var flujoMemoria = new MemoryStream())
                {
                    await archivo.CopyToAsync(flujoMemoria);

                    using (MemoryStream flujoMemoriaInterno = new MemoryStream(flujoMemoria.ToArray()))
                    {
                        using (StreamReader reader = new StreamReader(flujoMemoriaInterno))
                        {
                            string[] headers = reader.ReadLine().Split(',');

                            while (!reader.EndOfStream)
                            {
                                string[] filas = reader.ReadLine().Split(',');

                                listaUsuarioCsv.Add(new UsuarioCSV()
                                {
                                    IdUsuario = !string.IsNullOrEmpty(filas[0].ToString()) ? int.Parse(filas[0].ToString()) : 0,
                                    HorasTrabajadas = !string.IsNullOrEmpty(filas[1].ToString()) ? int.Parse(filas[1].ToString()) : 0,
                                    SueldoPorHora = !string.IsNullOrEmpty(filas[2].ToString()) ? Convert.ToDecimal(filas[2].ToString()) : 0,
                                    FechaProximoPago = !string.IsNullOrEmpty(filas[3].ToString()) ? Convert.ToDateTime(filas[3].ToString()) : default(DateTime)
                                }) ;
                            }
                        }
                    } 
                }

                if (listaUsuarioCsv != null)
                {
                    foreach (UsuarioCSV user in listaUsuarioCsv)
                    {
                        if (user.IdUsuario != 0)
                            await _userRepository.AddAsync(user);
                    }
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty,
                            "Ya existe una misma clave id de usuario en la base de datos.");
            }

            return RedirectToAction(nameof(ArchivosController.Index));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            UsuarioCSV usuario = _userRepository.Query().AsNoTracking().FirstOrDefault(q => q.IdUsuario == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            UsuarioCSV usuario = _userRepository.Find(q => q.IdUsuario == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("IdUsuario", "HorasTrabajadas", "SueldoPorHora", "FechaProximoPago")] UsuarioCSV model)
        {
            if (id != model.IdUsuario)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _userRepository.Update(model, id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userRepository.Query().Any(q => q.IdUsuario == id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(HomeController.Index));
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            UsuarioCSV user = _userRepository.Query().FirstOrDefault(q => q.IdUsuario == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int? id)
        {
            UsuarioCSV user = _userRepository.Find(q => q.IdUsuario == id);

            if (user == null)
                return NotFound();

            _userRepository.Delete(user);

            return RedirectToAction(nameof(HomeController.Index));
        }
    }
}