﻿using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    [Authorize(Roles = "Gerente,Administrativo,Operador")]
    public class CadProdutoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = ProdutoModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = ProdutoModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;
            ViewBag.UnidadesMedida = UnidadeMedidaModel.RecuperarLista(1, 9999);
            ViewBag.Grupos = GrupoProdutoModel.RecuperarLista(1, 9999);
            ViewBag.Marcas = MarcaProdutoModel.RecuperarLista(1, 9999);
            ViewBag.Fornecedores = FornecedorModel.RecuperarLista();
            ViewBag.LocaisArmazenamento = LocalArmazenamentoModel.RecuperarLista(1, 9999);

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ProdutoPagina(int pagina, int tamPag, string ordem)
        {
            var lista = ProdutoModel.RecuperarLista(pagina, tamPag, ordem: ordem);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarProduto(int id)
        {
            return Json(ProdutoModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Administrativo")]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirProduto(int id)
        {
            return Json(ProdutoModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarProduto()
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            var nomeArquivoImagem = "";
            HttpPostedFileBase arquivo = null;
            if (Request.Files.Count > 0)
            {
                arquivo = Request.Files[0];
                nomeArquivoImagem = Path.GetFileName(arquivo.FileName);
            }

            var model = new ProdutoModel()
            {
                Id = Int32.Parse(Request.Form["Id"]),
                Codigo = Request.Form["Codigo"],
                Nome = Request.Form["Nome"],
                PrecoCusto = Decimal.Parse(Request.Form["PrecoCusto"]),
                PrecoVenda = Decimal.Parse(Request.Form["PrecoVenda"]),
                QuantEstoque = Int32.Parse(Request.Form["QuantEstoque"]),
                IdUnidadeMedida = Int32.Parse(Request.Form["IdUnidadeMedida"]),
                IdGrupo = Int32.Parse(Request.Form["IdGrupo"]),
                IdMarca = Int32.Parse(Request.Form["IdMarca"]),
                IdFornecedor = Int32.Parse(Request.Form["IdFornecedor"]),
                IdLocalArmazenamento = Int32.Parse(Request.Form["IdLocalArmazenamento"]),
                Ativo = (Request.Form["Ativo"] == "true"),
                Imagem = nomeArquivoImagem
            };

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
                        if (!string.IsNullOrEmpty(nomeArquivoImagem) && arquivo != null)
                        {
                            //var caminhoArquivo = Path.Combine(Server.MapPath("~/Content/Imagens"), nomeArquivoImagem);
                            var caminhoArquivo = Path.Combine(Server.MapPath("~/Content/Imagens"));

                            if (!Directory.Exists(caminhoArquivo))
                            {
                                Directory.CreateDirectory(caminhoArquivo);
                            }

                            arquivo.SaveAs($"{caminhoArquivo}\\{nomeArquivoImagem}");
                        }
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