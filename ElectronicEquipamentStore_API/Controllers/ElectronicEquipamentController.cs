using AutoMapper;
using ElectronicEquipamentStore_API.Data;
using ElectronicEquipamentStore_API.Models;
using ElectronicEquipamentStore_API.Models.Dto;
using ElectronicEquipamentStore_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace ElectronicEquipamentStore_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ElectronicEquipamentController : ControllerBase
    {
        private readonly ILogger<ElectronicEquipamentController> _logger;
        private readonly IElectronicEquipamentRepository _equipamentRepository;
        private readonly IMapper _mapper;

        public ElectronicEquipamentController(ILogger<ElectronicEquipamentController> logger, IElectronicEquipamentRepository equipamentRepository, IMapper mapper)
        {
            _logger = logger;
            _equipamentRepository = equipamentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ElectronicEquipamentDto>>> GetAllEquipament()
        {
            var equipaments = await _equipamentRepository.GetAll();

            return Ok(_mapper.Map<IEnumerable<ElectronicEquipamentDto>>(equipaments));
        }

        [HttpGet("id:int", Name = "GetEquipament")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEquipamentById(int id)
        {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var equipament = await _equipamentRepository.Get(x => x.Id == id);

                if (equipament == null)
                {
                    return NotFound();
                }

                var equipamentDto = _mapper.Map<ElectronicEquipamentDto>(equipament);

                return Ok(equipamentDto);
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

            if ((await _equipamentRepository.Get(x => x.Name == equipamentCreateDto.Name, false)) != null)
            {
                ModelState.AddModelError("Name", "This name already exists");
                return BadRequest(ModelState);
            }

            var equipament = _mapper.Map<ElectronicEquipament>(equipamentCreateDto);

            equipament.CreationDate = DateTime.Now;
            equipament.UpdateDate= DateTime.Now;

            await _equipamentRepository.Create(equipament);

            var equipamentDto = _mapper.Map<ElectronicEquipamentDto>(equipament);

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

            var equipament = await _equipamentRepository.Get(x => x.Id == id, false);

            if (equipament == null) 
            {
                return NotFound();
            }

            await _equipamentRepository.Remove(equipament);

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

            var equipament = await _equipamentRepository.Get(x => x.Id == id, false);

            if (equipament == null)
            {
                return NotFound();
            }

            equipament = _mapper.Map<ElectronicEquipament>(equipamentUpdateDto);

            await _equipamentRepository.Update(equipament);

            return NoContent();
        }
    }
}
