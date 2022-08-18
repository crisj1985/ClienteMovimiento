using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;
using ClienteMovimiento.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        public IMapper _mapper { get; }
        public IRepository<Cuenta> _repositoryCuenta { get; }
        public IRepository<Cliente> _repositoryCliente { get; }
        public CuentaController(IMapper maper, IRepository<Cuenta> repositoryCuenta, IRepository<Cliente> repositoryCliente)
        {
            _mapper = maper;
            _repositoryCuenta = repositoryCuenta;
            _repositoryCliente = repositoryCliente;   
        }

        [HttpGet]
        public ActionResult<List<CuentasConMovimientosModel>> Get()
        {
            List<Cuenta> cuentas = _repositoryCuenta.GetAll().Include(x => x.Movimientos).ToList();
            List<CuentasConMovimientosModel> cuentasModel = _mapper.Map<List<CuentasConMovimientosModel>>(cuentas);
            return cuentasModel;
        }

        [HttpGet("{id:int}")]
        public ActionResult<CuentasConMovimientosModel> Get(int id)
        {
            Cuenta cuenta = _repositoryCuenta.GetAll().Include(x => x.Movimientos).FirstOrDefault(x => x.Id == id);
            CuentasConMovimientosModel cuentaModel = _mapper.Map<CuentasConMovimientosModel>(cuenta);
            return cuentaModel;
        }
        [HttpPost]
        public async Task<ActionResult> Post(CuentaModel cuentaModel)
        {
            Cuenta cuenta = _mapper.Map<Cuenta>(cuentaModel);   
            bool ExisteCliente = await _repositoryCliente.GetAll().AnyAsync(x => x.Id == cuenta.ClienteId);
            if (!ExisteCliente)
            {
                return BadRequest($"No existe el cliente {cuenta.ClienteId}");
            }
            await _repositoryCuenta.Add(cuenta);
            return Ok();

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(CuentaActualizarModel cuentaModel, int id)
        {
            Cuenta cuenta = _mapper.Map<Cuenta>(cuentaModel);

            if (cuenta.Id != id)
            {
                return BadRequest("Cuenta diferente al enviado a actualizar");
            }
            bool ExisteCuenta = _repositoryCuenta.GetAll().Any(x => x.Id == id);
            if (!ExisteCuenta)
            {
                return NotFound(); ;
            }
            bool ExisteCliente = await _repositoryCliente.GetAll().AnyAsync(x => x.Id == cuenta.ClienteId);
            if (!ExisteCliente)
            {
                return BadRequest($"No existe el cliente {cuenta.ClienteId}");
            }

            await _repositoryCuenta.Update(cuenta);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool ExisteCuenta = _repositoryCuenta.GetAll().Any(x => x.Id == id);
            if (!ExisteCuenta)
            {
                return NotFound(); ;
            }
            await _repositoryCuenta.Delete(new Cuenta { Id = id });
            return Ok();

        }
    }
}
