using Logal.DTO;
using Logal.Models;
using Logal.Templates;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using C = Microsoft.AspNetCore.Components;

namespace Logal.Controllers
{
    [ApiController]
    [Route("api/default")]
    public class HelloController(HtmlRenderer renderer, HtmlToPdf pdf): ControllerBase
    {

        [HttpGet("hello")]
        public string SayHello([FromQuery]string name)
        {
            return "Hello " + name;
        }

        // /product/42 recupérer le produit 42
        // /product?name=coca-cola récupérer tous les produits dont le nom est coca cola

        [HttpGet("log")]
        
        public IActionResult Log([FromQuery]double nombre)
        {
            if(nombre > 0)
            {
                return Ok(Math.Log(nombre));
            }
            else
            {
                return BadRequest("La valeur encodée doit être strictement supérieure à 0");
            }
        }

        [HttpPost]
        public IActionResult Test(dynamic parameters)
        {
            int id = parameters.id;
            string name = parameters.nom;

            return Ok(new ResponseDTO { 
                Message = "Test", Status = Enums.Status.Warning 
            });
        }

        [HttpGet]
        public IActionResult GetPDF()
        {
            FactureModel model = GetModel();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["Model"] = model;

            string html = renderer.Dispatcher.InvokeAsync(
                () =>  renderer.RenderComponentAsync<InvoiceTemplate>(
                     C.ParameterView.FromDictionary(parameters)
                ).Result.ToHtmlString()
            ).Result;

            var document = pdf.ConvertHtmlString(html);
            using MemoryStream stream = new MemoryStream();
            document.Save(stream);

            return File(stream.ToArray(), "application/pdf", "facture.pdf");
        }

        private FactureModel GetModel()
        {
            return new FactureModel
            {
                Reference = "XXXXXXX",
                Name = "Logal",
                VAT = "BE0411.897.335",
                Articles = new List<Article>
                {
                    new Article { Id = 1, Name = "Coca cola", Quantity = 75, UnitPrice = 0.53m },
                    new Article { Id = 2, Name = "Fanta", Quantity = 50, UnitPrice = 0.52m },
                    new Article { Id = 3, Name = "Dr Pepper", Quantity = 100, UnitPrice = 0.60m },
                }
            };
        }
    }
}
