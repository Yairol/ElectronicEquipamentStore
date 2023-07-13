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

        public ElectronicEquipamentController(ILogger<ElectronicEquipamentController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ElectronicEquipamentDto>> GetAllEquipament()
        {
            _logger.LogInformation("Getting all equipament");
            return Ok(_db.Equipaments.ToList());
        }

        [HttpGet("id:int", Name = "GetEquipament")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ElectronicEquipamentDto> GetEquipamentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Id out of range");
                return BadRequest();
            }

            var equipament = _db.Equipaments.FirstOrDefault(x => x.Id == id);

            if (equipament == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Equipment was obtained");
            return Ok(equipament);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ElectronicEquipamentDto> CreateEquipament([FromBody] ElectronicEquipamentDto equipamentDto)
        {
            if (equipamentDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_db.Equipaments.FirstOrDefault(x => x.Name == equipamentDto.Name) != null)
            {
                ModelState.AddModelError("Name", "This name already exists");
                return BadRequest(ModelState);
            }

            if (equipamentDto.Id != 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            ElectronicEquipament equipament = new()
            {
                Name = equipamentDto.Name,
                Description = equipamentDto.Description,
                EquipamentType = equipamentDto.EquipamentType,
                Price = equipamentDto.Price,
            };

            _db.Equipaments.Add(equipament);
            _db.SaveChanges();

            _logger.LogInformation("Equipament created");
            return CreatedAtRoute("GetEquipament", new { id = equipament.Id }, equipament);

        }
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEquipament(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            if(_db.Equipaments.AsNoTracking().FirstOrDefault(x => x.Id == id) == null) 
            {
                return NotFound();
            }

            _db.Equipaments.Remove(_db.Equipaments.AsNoTracking().FirstOrDefault(x => x.Id == id));
            _db.SaveChanges();

            _logger.LogInformation("Equipament deleted");
            return NoContent();
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateEquipament(int id, [FromBody] ElectronicEquipamentDto equipamentDto)
        {
            if(id <= 0 || id != equipamentDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ElectronicEquipament equipament = new()
            {
                Id = equipamentDto.Id,
                Name = equipamentDto.Name,
                Description = equipamentDto.Description,
                EquipamentType = equipamentDto.EquipamentType,
                Price = equipamentDto.Price
            };

            if (_db.Equipaments.AsNoTracking().FirstOrDefault(x => x.Id == id) == null)
            {
                return NotFound();
            }

            _db.Equipaments.Update(equipament);
            _db.SaveChanges();

            _logger.LogInformation("Equipament Updated");
            return NoContent();
        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialEquipament(int id, JsonPatchDocument<ElectronicEquipamentDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var equipament = _db.Equipaments.FirstOrDefault(x => x.Id == id);

            if (equipament == null)
            {
                return NotFound();
            }

            ElectronicEquipamentDto equipamentDto = new()
            {
                Id =equipament.Id,
                Name = equipament.Name,
                Description = equipament.Description,
                EquipamentType = equipament.EquipamentType,
                Price = equipament.Price
            };

            patchDto.ApplyTo(equipamentDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ElectronicEquipament newEquipament = new()
            {
                Id = equipament.Id,
                Name = equipamentDto.Name,
                Description = equipamentDto.Description,
                EquipamentType = equipamentDto.EquipamentType,
                Price = equipament.Price
            };

            _db.Equipaments.Update(newEquipament);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
