using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaController : ControllerBase
	{
		public ILogger<VillaController> _Logger { get; }
		private readonly ApplicationDbContext _DbContext;
		private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper)
        {
			_Logger = logger;
			_DbContext = db;
			_mapper = mapper;
		}

        [HttpGet]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
		{
			_Logger.LogInformation("Obteniendo villas");

			if (_DbContext.Villas == null)
			{
				return NotFound();
			}

			IEnumerable<Villa> villaList = await _DbContext.Villas.ToListAsync();

			return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
		}

		[HttpGet("{Id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<VillaDto?>> GetVilla(int? Id)
		{
			if (Id == 0 || Id == null)
			{
				return BadRequest();
			}

			var villa = await _DbContext.Villas.FirstOrDefaultAsync(v => v.Id == Id);

			if (villa == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<VillaDto>(villa));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto CreateVillaDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (await _DbContext.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == CreateVillaDto.Nombre.ToLower()) != null)
			{
				ModelState.AddModelError("Nombre Existente", $"El nombre '{CreateVillaDto.Nombre}' ya existe en la base de datos");

				return BadRequest(ModelState);
			}

			if (CreateVillaDto == null)
			{
				return BadRequest(CreateVillaDto);
			}

			//villaDto.Id = VillaStore.villasDtos.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

			//VillaStore.villasDtos.Add(villaDto);

			Villa modelo = _mapper.Map<Villa>(CreateVillaDto);

			await _DbContext.Villas.AddAsync(modelo);
			await _DbContext.SaveChangesAsync();

			return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> DeleteVilla(int? id)
		{
			if (id == 0 || id == null)
			{
				return BadRequest();	
			}

			//var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == id);
			var villa = await _DbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

			if (villa == null)
			{
				return NotFound();
			}

			//VillaStore.villasDtos.Remove(villa);

			_DbContext.Villas.Remove(villa);
			await _DbContext.SaveChangesAsync();

			return NoContent();
		}

		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task< IActionResult> UpdateVilla(int? id , [FromBody] VillaUpDateDto UpDateVillaDto)
		{
			if (id == null || id == 0 || id != UpDateVillaDto.Id)
			{
				return BadRequest();
			}

			Villa modelo = _mapper.Map<Villa>(UpDateVillaDto);

			_DbContext.Villas.Update(modelo);
					
			await _DbContext.SaveChangesAsync();

			return NoContent();

		}

		[HttpPatch("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> UpdateParcialVilla(int id, JsonPatchDocument<VillaUpDateDto> patchDto)
		{
			if (patchDto == null || id == 0)
			{
				return BadRequest();
			}

			var villa = await _DbContext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

			if (villa == null) return BadRequest();

			VillaUpDateDto villaDto = _mapper.Map<VillaUpDateDto>(villa);

			patchDto.ApplyTo(villaDto, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Villa modelo = _mapper.Map<Villa>(villaDto);

			_DbContext.Villas.Update(modelo);
			await _DbContext.SaveChangesAsync();

			return NoContent();

		}
	}	
}
