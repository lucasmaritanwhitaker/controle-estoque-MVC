using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class CadGrupoProdutoController : Controller
    {
        private const int quantLinhaPorPagina = 5;


        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { quantLinhaPorPagina, 10, 15, 20 }, quantLinhaPorPagina);
            ViewBag.QuantMaxLinhasPorPag = quantLinhaPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = GrupoProdutoModel.RecuperarLista(ViewBag.PaginaAtual, quantLinhaPorPagina);
            var quant = GrupoProdutoModel.RecuperarQuantidade();

            var difQuantiPaginas = (quant % ViewBag.QuantMaxLinhasPorPag) > 0 ? 1 : 0;
            ViewBag.QuantidadePaginas = (quant / ViewBag.QuantMaxLinhasPorPag) + difQuantiPaginas;


            return View(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult GrupoProdutoPagina(int pagina, int tamPag)
        {
            var lista = GrupoProdutoModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarGrupoProduto(int id)
        {
            return Json(GrupoProdutoModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirGrupoProduto(int id)
        {
            return Json(GrupoProdutoModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarGrupoProduto(GrupoProdutoModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

    }
}