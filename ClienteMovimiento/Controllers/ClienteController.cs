using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;
using ClienteMovimiento.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public IMapper _mapper { get; }
        public IRepository<Cliente> _repository { get; }

        public ClienteController(IMapper mapper, IRepository<Cliente> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        public ActionResult<List<ClienteConCuentasModel>> Get()
        {
            List<Cliente> clientes = _repository.GetAll().Include(x=>x.Cuentas).ToList();
            List<ClienteConCuentasModel> clientesModel = _mapper.Map<List<ClienteConCuentasModel>>(clientes);
            return clientesModel;
        }
        [HttpGet("{id}")]
        public ActionResult<ClienteConCuentasModel> Get(int id)
        {
            Cliente cliente = _repository.GetAll().Include(x => x.Cuentas).FirstOrDefault(x => x.Id == id);
            ClienteConCuentasModel clienteModel = _mapper.Map<ClienteConCuentasModel>(cliente);
            return clienteModel;
        }
        [HttpPost]
        public async Task<ActionResult> Post(ClienteModel clienteModel)
        {
            Cliente cliente = _mapper.Map<Cliente>(clienteModel);
            await _repository.Add(cliente);
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
            bool ExisteCliente = _repository.GetAll().Any(x => x.Id == id);
            if (!ExisteCliente)
            {
                return NotFound(); ;
            }

            await _repository.Update(cliente);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool ExisteCliente = _repository.GetAll().Any(x => x.Id == id);
            if (!ExisteCliente)
            {
                return NotFound(); ;
            }
            await _repository.Delete(new Cliente { Id = id});
            return Ok();

        }
    }
}
