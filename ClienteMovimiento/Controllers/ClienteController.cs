using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public AppDbContext _context { get; }
        public IMapper _mapper { get; }

        public ClienteController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<ClienteModel>>> Get()
        {
            List<Cliente> clientes = await _context.Clientes.Include(x=>x.Cuentas).ToListAsync();
            List<ClienteModel> clientesModel = _mapper.Map<List<ClienteModel>>(clientes);
            return clientesModel;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteModel>> Get(int id)
        {
            Cliente cliente = await _context.Clientes.Include(x => x.Cuentas).FirstOrDefaultAsync(x=>x.Id == id);
            ClienteModel clienteModel = _mapper.Map<ClienteModel>(cliente);
            return clienteModel;
        }
        [HttpPost]
        public async Task<ActionResult> Post(ClienteModel clienteModel)
        {
            Cliente cliente = _mapper.Map<Cliente>(clienteModel);   
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(ClienteActualizarModel clienteModel, int id)
        {
            Cliente cliente = _mapper.Map<Cliente>(clienteModel);

            if (cliente.Id != id)
            {
                return BadRequest("Cliente diferente al enviado a actualizar");
            }

            _context.Update(cliente);
            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool ExisteCliente = await _context.Clientes.AnyAsync(x => x.Id == id);    
            if (!ExisteCliente)
            {
                return NotFound(); ;
            }
            _context.Remove(new Cliente { Id=id});
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
