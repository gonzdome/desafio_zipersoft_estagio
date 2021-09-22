using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using desafio_zipersoft.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;

namespace desafio_zipersoft.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;
        private readonly IWebHostEnvironment _hostEnvironment; //propriedade injetada para acessar o caminho local da imagem

        public UsuariosController(Contexto context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,RG,IE,CPF,CNPJ,Endereco,Numero,Bairro,CEP,Cidade,Email,Site,Celular,Telefone,Observacao,ImageFile")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                //salvar imagem no wwwRoot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(usuario.ImageFile.FileName);
                string extention = Path.GetExtension(usuario.ImageFile.FileName);
                usuario.Foto = fileName = fileName + DateTime.Now.ToString("yymmssffff") + extention;
                string path = Path.Combine(wwwRootPath + "/Image", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await usuario.ImageFile.CopyToAsync(fileStream);
                }

                //inserir registro
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,RG,IE,CPF,CNPJ,Endereco,Numero,Bairro,CEP,Cidade,Email,Site,Celular,Telefone,Observacao,ImageFile")] Usuario usuario)
        {

            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {

                    //salvar imagem no wwwRoot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(usuario.ImageFile.FileName);
                    string extention = Path.GetExtension(usuario.ImageFile.FileName);
                    usuario.Foto = fileName = fileName + DateTime.Now.ToString("yymmssffff") + extention;
                    string path = Path.Combine(wwwRootPath + "/Image", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await usuario.ImageFile.CopyToAsync(fileStream);
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Send/5
        public async Task<IActionResult> Send(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Send/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int id, [Bind("Id,Nome,RG,IE,CPF,CNPJ,Endereco,Numero,Bairro,CEP,Cidade,Email,Site,Celular,Telefone,Observacao,ImageFile")] Usuario formData)
        {

            if (id != formData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {

                    _context.Update(formData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(formData.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //enviando email usando Gmail SMTP

            using(var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com");
                client.Authenticate("@gmail.com", "");

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"<p>{formData.Id}</p><p>{formData.Nome}</p><p>{formData.Endereco}</p><p>{formData.Numero}</p>" +
                    $"<p>{formData.Bairro}</p><p>{formData.CEP}</p><p>{formData.Cidade}</p>" +
                    $"<p>{formData.CPF}</p><p>{formData.CNPJ}</p><p>{formData.RG}</p><p>{formData.IE}</p>" +
                    $"<p>{formData.Email}</p><p>{formData.Site}</p><p>{formData.Telefone}</p>" +
                    $"<p>{formData.Celular}</p><p>{formData.Observacao}</p><p>{formData.Foto}</p>",
                TextBody = "{formData.Id} \r\n {formData.Nome}"
                };

                var message = new MimeMessage
                {
                    Body = bodyBuilder.ToMessageBody()
                };

                message.From.Add(new MailboxAddress("Informações do Cadastro", ""));
                message.To.Add(new MailboxAddress("Teste", ""));
                message.Subject = "Informações";
                client.Send(message);
                client.Disconnect(true);
            }
            TempData["Message"] = "Email enviado com sucesso!";
            return RedirectToAction("Send");
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            //deletar imagem de wwwRoot
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", usuario.Foto);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            //deletar o registro

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}
