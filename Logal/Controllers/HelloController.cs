using Logal.DTO;
using Logal.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logal.Controllers
{
    [ApiController]
    [Route("api/default")]
    public class HelloController: ControllerBase
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
    }
}
