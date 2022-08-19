using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.ManejoExcepciones;
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
            List<Cuenta> cuentasDB = _repositoryCuenta.GetAll().ToList();
            cuenta = cuentasDB.FirstOrDefault(x => x.Id == movimientoModel.CuentaId);
            List<object> Result = ProcesarMovimiento(movimientoModel, cuenta);
            
            if (Result.Count > 0 && Result[0].ToString().Equals("ok", StringComparison.OrdinalIgnoreCase))
            {
                movimientoModel.Saldo = (Result[1] as MovimientoModel).Saldo;
                Movimiento movimiento = _mapper.Map<Movimiento>(movimientoModel);
                cuenta.SaldoInicial = movimientoModel.Saldo + movimientoModel.Valor;
                await _repositoryMovimiento.Add(movimiento);
                await _repositoryCuenta.Add(cuenta);
            }
            else
            {
                return BadRequest(Result[0].ToString());
            }

            return Ok();

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(MovimientoActualizarModel movimientoModel, int id)
        {
            Movimiento movimiento = _mapper.Map<Movimiento>(movimientoModel);

            if (movimiento.Id != id)
            {
                ClsExcepcionCapturada.EscribirEvento($"Movimiento diferente al enviado a actualizar {movimientoModel.CuentaId}");
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
                ClsExcepcionCapturada.EscribirEvento($"No existe el cuenta {movimiento.CuentaId}");
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
            decimal total = 0;
            IEnumerable<Movimiento> MovsFecha = null;
            try
            {
                List<Movimiento> movs = _repositoryMovimiento.GetAll().Where(x => x.CuentaId == movimientoModel.CuentaId).ToList();
                if (movs.Count > 0)
                {
                    MovsFecha = movs.Where(x => x.Fecha.Date == movimientoModel.Fecha.Date
                                                && x.TipoMovimiento.Equals("Retiro", StringComparison.OrdinalIgnoreCase));
                    if(MovsFecha.Count() > 0)
                        total = MovsFecha.Sum(x => Math.Abs(x.Valor)) + Math.Abs(movimientoModel.Valor);
                }
            }
            catch (Exception ex)
            {
                ClsExcepcionCapturada.EscribirEvento($"{ex?.Message} - {ex?.StackTrace}");
            }
            return total >= _config.GetValue<decimal>("CupoDiario") ;
        }
        private List<object> ProcesarMovimiento(MovimientoModel movimientoModel, Cuenta cuenta)
        {
            List<object> Result = new List<object>();
            List<Movimiento> movimientosCuenta = null;
            string mensaje = "ok";
            MovimientoModel movimientoResult = movimientoModel;
            DateTime MaxFecha;
            try
            {
                if (cuenta == null)
                {
                    mensaje = $"No existe cuenta {movimientoModel.CuentaId} para el movimiento";
                    ClsExcepcionCapturada.EscribirEvento(mensaje);
                }
                else
                {
                    movimientosCuenta = _repositoryMovimiento.GetAll().Where(x => x.CuentaId == movimientoModel.CuentaId).ToList();
                    if (movimientosCuenta.Count== 0)
                    {
                        movimientoResult.Saldo = cuenta.SaldoInicial;
                    }
                    else
                    { 
                        MaxFecha = movimientosCuenta.Max(x => x.Fecha);
                        movimientoResult.Saldo = movimientosCuenta.FirstOrDefault(x=>x.Fecha == MaxFecha).Saldo;
                    }
          
                    movimientoResult.Saldo += movimientoModel.Valor;

                    if (movimientoResult.Saldo < 0)
                    {
                        mensaje = $"Saldo Insuficiente para {movimientoModel.CuentaId}";
                        ClsExcepcionCapturada.EscribirEvento(mensaje);
                        Result.Add(mensaje);
                        return Result;

                    }

                    if (SuperoLimiteDiario(movimientoModel))
                    {
                        mensaje = $"Cupo Excedido para {movimientoModel.CuentaId}";
                        ClsExcepcionCapturada.EscribirEvento(mensaje);
                        Result.Add(mensaje);
                        return Result;
                    }
                    Result.Add(mensaje);
                    Result.Add(movimientoResult);
                }
            }
            catch (Exception ex)
            {
                ClsExcepcionCapturada.EscribirEvento(ex.Message);
            }
            return Result;
        }
           
    }
}
