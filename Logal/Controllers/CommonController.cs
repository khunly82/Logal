using Microsoft.AspNetCore.Mvc;

namespace Logal.Controllers
{
    public abstract class CommonController : ControllerBase
    {
        [HttpGet("Method1")]
        public virtual IActionResult Method1()
        {
            return Ok("Common Method1");
        }

        [HttpGet("Method2")]
        public virtual IActionResult Method2()
        {
            return Ok("Common Method2");
        }


    }
}
