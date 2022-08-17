using AutoMapper;
using ClienteMovimiento.Entities;
using ClienteMovimiento.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteMovimiento.Controllers
{
    [Route("api/cuenta")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        public CuentaController(AppDbContext context, IMapper maper)
        {
            _context = context;
            _mapper = maper;
        }

        public AppDbContext _context { get; }
        public IMapper _mapper { get; }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CuentaModel>> Get(int id)
        {
            Cuenta cuenta = await _context.Cuentas.FirstOrDefaultAsync(x=>x.Id==id);

            CuentaModel CuentaModel = _mapper.Map<CuentaModel>(cuenta);

            return CuentaModel;
        }
        [HttpPost]
        public async Task<ActionResult> Post(CuentaModel cuentaModel)
        {
            Cuenta cuenta = _mapper.Map<Cuenta>(cuentaModel);   
            bool ExisteCuenta = await _context.Clientes.AnyAsync(x => x.Id == cuenta.ClienteId);
            if (!ExisteCuenta)
            {
                return BadRequest($"No existe el cliente {cuenta.ClienteId}");
            }
            _context.Add(cuenta);
            await _context.SaveChangesAsync();
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
            _context.Update(cuenta);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
