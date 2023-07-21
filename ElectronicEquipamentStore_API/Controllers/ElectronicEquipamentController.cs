using AutoMapper;
using ElectronicEquipamentStore_API.Data;
using ElectronicEquipamentStore_API.Models;
using ElectronicEquipamentStore_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ElectronicEquipamentStore_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ElectronicEquipamentController : ControllerBase
    {
        private readonly ILogger<ElectronicEquipamentController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ElectronicEquipamentController(ILogger<ElectronicEquipamentController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectronicEquipamentDto>>> GetAllEquipament()
        {
            _logger.LogInformation("Getting all equipament");

            IEnumerable<ElectronicEquipament> listEquipaments = await _db.Equipaments.ToListAsync();
            
            return Ok(_mapper.Map<IEnumerable<ElectronicEquipamentDto>>(listEquipaments));

        }

        [HttpGet("id:int", Name = "GetEquipament")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ElectronicEquipamentDto>> GetEquipamentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Id out of range");
                return BadRequest();
            }

            var equipament = (await _db.Equipaments.FirstOrDefaultAsync(x => x.Id == id));

            if (equipament == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Equipment was obtained");
            return Ok(_mapper.Map<ElectronicEquipamentDto>(equipament));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ElectronicEquipamentDto>> CreateEquipament([FromBody] ElectronicEquipamentCreateDto equipamentCreateDto)
        {
            if (equipamentCreateDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if ((await _db.Equipaments.FirstOrDefaultAsync(x => x.Name == equipamentCreateDto.Name)) != null)
            {
                ModelState.AddModelError("Name", "This name already exists");
                return BadRequest(ModelState);
            }

            var equipament = _mapper.Map<ElectronicEquipament>(equipamentCreateDto);
            equipament.CreationDate = DateTime.Now;
            equipament.UpdateDate = DateTime.Now;

            await _db.Equipaments.AddAsync(equipament);
            await _db.SaveChangesAsync();

            var equipamentDto = _mapper.Map<ElectronicEquipamentDto>(equipament);

            _logger.LogInformation("Equipament created");
            return CreatedAtRoute("GetEquipament", new { id = equipamentDto.Id }, equipamentDto);

        }
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEquipament(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            var equipament = await _db.Equipaments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (equipament == null) 
            {
                return NotFound();
            }

            _db.Equipaments.Remove(equipament);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Equipament deleted");
            return NoContent();
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateEquipament(int id, [FromBody] ElectronicEquipamentUpdateDto equipamentUpdateDto)
        {
            if(id <= 0 || id != equipamentUpdateDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipament = await _db.Equipaments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (equipament == null)
            {
                return NotFound();
            }

            equipament = _mapper.Map<ElectronicEquipament>(equipamentUpdateDto);
            equipament.UpdateDate = DateTime.Now;

            _db.Equipaments.Update(equipament);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Equipament Updated");
            return NoContent();
        }
    }
}
