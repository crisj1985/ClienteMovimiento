using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;
using ClienteMovimiento.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Controllers
{
    [Route("api/reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        public IMapper _mapper { get; }
        public IRepository<Cuenta> _repositoryCuenta { get; }
        public IRepository<Cliente> _repositoryCliente { get; }
        public IRepository<Movimiento> _repositoryMovimiento { get; }
        public ReportesController(IMapper maper, IRepository<Cuenta> repositoryCuenta, IRepository<Cliente> repositoryCliente, IRepository<Movimiento> repositoryMovimiento)
        {
            _mapper = maper;
            _repositoryCuenta = repositoryCuenta;
            _repositoryCliente = repositoryCliente;
            _repositoryMovimiento = repositoryMovimiento;
        }
        [HttpGet]
        public ActionResult<List<ReporteModel>> Get(DateTime FechaDesde, DateTime HastaDesde, int id)
        {
            List<ReporteModel> Result = new List<ReporteModel>();
            List<Cuenta> MovimientosCliente = _repositoryCuenta.GetAll().Include(x => x.Movimientos).Where(x => x.ClienteId == id).ToList();
            MovimientosCliente.ForEach(x =>x.Movimientos.Where(y => y.Fecha.Date >= FechaDesde.Date && y.Fecha.Date <= HastaDesde.Date).ToList()
                .ForEach(z => Result.Add(new ReporteModel
                {
                    Fecha = z.Fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                    Cliente = _repositoryCliente.GetElementById(id).Nombre,
                    Estado = x.Estado,
                    Movimiento = z.Valor.ToString(),
                    NumeroCuenta = x.NumeroCuenta,
                    Saldo = z.Saldo.ToString(),
                    SaldoDisponible = z.Saldo.ToString(),
                    Tipo = x.TipoCuenta
                })));
            return Ok(Result.OrderBy(x=>x.Fecha));
        }
    }
}
