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
        private readonly IConfiguration config;

        public IMapper _mapper { get; }
        public IRepository<Movimiento> _repositoryMovimiento { get; }
        public IRepository<Cuenta> _repositoryCuenta { get; }
        public IConfiguration _config { get; set; }
        public MovimientosController(IMapper maper, IRepository<Movimiento> repositoryMovimiento, IRepository<Cuenta> repositoryCuenta, IConfiguration config)
        {
            _mapper = maper;
            _repositoryMovimiento = repositoryMovimiento;
            _repositoryCuenta = repositoryCuenta;
            _config = config;
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
            Movimiento movimiento = _repositoryMovimiento.GetAll().FirstOrDefault(x => x.Id == id);
            MovimientoModel movimientoModel = _mapper.Map<MovimientoModel>(movimiento);
            return movimientoModel;
        }
        [HttpPost]
        public async Task<ActionResult> Post(MovimientoModel movimientoModel)
        {
            Cuenta cuenta = new Cuenta();
            bool ExisteCuenta = await _repositoryCuenta.GetAll().AnyAsync(x => x.Id == movimientoModel.CuentaId);
            if (!ExisteCuenta)
            {
                return BadRequest($"No existe el cuenta {movimientoModel.CuentaId}");
            }
            else
            {
                if (SuperoLimiteDiario(movimientoModel))
                    return BadRequest($"Cupo Excedido");
                else 
                { 
                    cuenta = await _repositoryCuenta.GetAll().FirstOrDefaultAsync(x => x.Id == movimientoModel.CuentaId);

                    if (movimientoModel.Valor < 0)
                    {
                        if (cuenta.SaldoInicial - Math.Abs(movimientoModel.Valor) < 0)
                        {
                            return BadRequest($"Saldo Insuficiente");
                        }
                        movimientoModel.Saldo = cuenta.SaldoInicial - Math.Abs(movimientoModel.Valor);
                    }
                    else
                    {
                        movimientoModel.Saldo = cuenta.SaldoInicial + Math.Abs(movimientoModel.Valor);
                    }
                    cuenta.SaldoInicial = movimientoModel.Saldo;
                }

            }
            Movimiento movimiento = _mapper.Map<Movimiento>(movimientoModel);

            await _repositoryMovimiento.Add(movimiento);
            await _repositoryCuenta.Update(cuenta);
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
        private bool SuperoLimiteDiario(MovimientoModel movimientoModel)
        {
            List<Movimiento> movs = _repositoryMovimiento.GetAll().ToList();
            decimal total = movs.Where(x => x.Fecha.Date == movimientoModel.Fecha.Date 
                                        && x.CuentaId == movimientoModel.CuentaId 
                                        && x.TipoMovimiento.Equals("Retiro",StringComparison.OrdinalIgnoreCase))
                            .Sum(x => Math.Abs(x.Valor)) + Math.Abs(movimientoModel.Valor);
            return total >= _config.GetValue<decimal>("CupoDiario") ;
        } 
        private List<Movimiento> BuscarMovimientosByFecha(DateTime FechaDesde, DateTime HastaDesde)
        {
            return _repositoryMovimiento.GetAll().Where(x => x.Fecha.Date >= FechaDesde.Date && x.Fecha.Date <= HastaDesde.Date).ToList<Movimiento>();
        }
    }
}
