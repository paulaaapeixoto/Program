using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Teste.Models;

namespace Teste.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["usuarioLogadoID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult Login(Usuario u)
        {
            if (ModelState.IsValid)
            {

                using (CadastroEntities1 dc = new CadastroEntities1())
                {
                    var v = dc.Usuarios.Where(a => a.NomeUsuario.Equals(u.NomeUsuario) && a.Senha.Equals(u.Senha)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["usuarioLogado"] = v.Id.ToString();
                        return RedirectToAction("ListarLivros");
                    }
                }
            }
            return View(u);

        }

        public ActionResult ListarLivros()
        {
            if (Session["usuarioLogado"] != null)
            {
                List<Livro> livros = new List<Livro>();

                using (CadastroEntities1 dc = new CadastroEntities1())
                {
                    livros = dc.Livroes.ToList();
                }
                return View(livros);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Pesquisar(string tituloLivro)
        {
            List<Livro> livros = new List<Livro>();

            using (CadastroEntities1 dc = new CadastroEntities1())
            {
                livros = dc.Livroes.Where(a => a.Titulo.EndsWith(tituloLivro)).ToList();
            }

            return View(livros);
        }

        public ActionResult Detalhes(Livro livro)
        {
            if (Session["usuarioLogado"] != null)
            {
                Livro livroAux = new Livro();
                using (CadastroEntities1 dc = new CadastroEntities1())
                {
                    livroAux = dc.Livroes.Where(a => a.Id.Equals(livro.Id)).FirstOrDefault();
                }
                return View(livroAux);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Alugar(Livro livro)
        {
            Livro livroSeraAlugado = new Livro();
            using (CadastroEntities1 dc = new CadastroEntities1())
            {

                livroSeraAlugado = dc.Livroes.Where(a => a.Id.Equals(livro.Id)).FirstOrDefault();
                if (!livroSeraAlugado.Alugado)
                {
                    livroSeraAlugado.Alugado = true;
                    dc.Livroes.AddOrUpdate(livroSeraAlugado);
                    dc.SaveChanges();
                }
            }

            return RedirectToAction("ListarLivros");
        }
        public ActionResult Liberar(Livro livro)
        {
            Livro livroSeraAlugado = new Livro();
            using (CadastroEntities1 dc = new CadastroEntities1())
            {
                livroSeraAlugado = dc.Livroes.Where(a => a.Id.Equals(livro.Id)).FirstOrDefault();
                livroSeraAlugado.Alugado = false;
                dc.Livroes.AddOrUpdate(livroSeraAlugado);
                dc.SaveChanges();
            }

            return RedirectToAction("ListarLivros");
        }

    }
}