using ClienteMovimiento.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClienteMovimiento.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Get()
        { 
            List<Cliente> valores = new List<Cliente>();
            valores.Add(new Cliente { ClienteId=1,Contrasena="23"});

            return valores;
        }
    }
}
