using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;
using ClienteMovimiento.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Controllers
{
    [Route("api/Movimientos")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        public IMapper _mapper { get; }
        public IRepository<Movimiento> _repositoryMovimiento { get; }
        public IRepository<Cuenta> _repositoryCuenta { get; }
        public MovimientosController(IMapper maper, IRepository<Movimiento> repositoryMovimiento, IRepository<Cuenta> repositoryCuenta)
        {
            _mapper = maper;
            _repositoryMovimiento = repositoryMovimiento;
            _repositoryCuenta = repositoryCuenta;
        }

        [HttpGet]
        public ActionResult<List<MovimientoModel>> Get()
        {
            List<Movimiento> movimientos = _repositoryMovimiento.GetAll().ToList();
            List<MovimientoModel> movimientosModel = _mapper.Map<List<MovimientoModel>>(movimientos);
            return movimientosModel;
        }

        [HttpGet("{id:int}")]
        public ActionResult<MovimientoModel> Get(int id)
        {
            Movimiento movimiento = _repositoryMovimiento.GetAll()
                                                        .FirstOrDefault(x => x.Id == id);
            MovimientoModel movimientoModel = _mapper.Map<MovimientoModel>(movimiento);
            return movimientoModel;
        }
        [HttpPost]
        public async Task<ActionResult> Post(MovimientoModel movimientoModel)
        {
            Movimiento movimiento = _mapper.Map<Movimiento>(movimientoModel);
            bool ExisteCuenta = await _repositoryCuenta.GetAll().AnyAsync(x => x.Id == movimiento.CuentaId);
            if (!ExisteCuenta)
            {
                return BadRequest($"No existe el cuenta {movimiento.CuentaId}");
            }
            await _repositoryMovimiento.Add(movimiento);
            return Ok();

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(MovimientoActualizarModel movimientoModel, int id)
        {
            Movimiento movimiento = _mapper.Map<Movimiento>(movimientoModel);

            if (movimiento.Id != id)
            {
                return BadRequest("Movimiento diferente al enviado a actualizar");
            }
            bool ExisteMovimiento = _repositoryMovimiento.GetAll().Any(x => x.Id == id);
            if (!ExisteMovimiento)
            {
                return NotFound(); ;
            }
            bool ExisteCuenta = await _repositoryCuenta.GetAll().AnyAsync(x => x.Id == movimiento.CuentaId);
            if (!ExisteCuenta)
            {
                return BadRequest($"No existe el cuenta {movimiento.CuentaId}");
            }

            await _repositoryMovimiento.Update(movimiento);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool ExisteCuenta = _repositoryMovimiento.GetAll().Any(x => x.Id == id);
            if (!ExisteCuenta)
            {
                return NotFound(); ;
            }
            await _repositoryMovimiento.Delete(new Movimiento { Id = id });
            return Ok();

        }
    }
}
