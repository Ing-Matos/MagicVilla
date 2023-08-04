using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaController : ControllerBase
	{
		public ILogger<VillaController> _Logger { get; }

		public VillaController(ILogger<VillaController> logger)
        {
			_Logger = logger;
		}

        [HttpGet]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDto>> GetVillas()
		{
			_Logger.LogInformation("Obteniendo villas");

			if (VillaStore.villasDtos == null)
			{
				return NotFound();
			}

			return Ok(VillaStore.villasDtos);
		}

		[HttpGet("{Id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<VillaDto?> GetVilla(int? Id)
		{
			if (Id == 0 || Id == null)
			{
				return BadRequest();
			}

			var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == Id);

			if (villa == null)
			{
				return NotFound();
			}

			return Ok(villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (VillaStore.villasDtos.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
			{
				ModelState.AddModelError("Nombre Existente", $"El nombre '{villaDto.Nombre}' ya existe en la base de datos");

				return BadRequest(ModelState);
			}

			if (villaDto == null)
			{
				return BadRequest(villaDto);
			}
			if (villaDto.Id>0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			villaDto.Id = VillaStore.villasDtos.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

			VillaStore.villasDtos.Add(villaDto);

			return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult DeleteVilla(int id)
		{
			if (id == 0 || id == null)
			{
				return BadRequest();	
			}

			var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == id);

			if (villa == null)
			{
				return NotFound();
			}

			VillaStore.villasDtos.Remove(villa);

			return NoContent();
		}

		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult UpdateVilla(int id , [FromBody] VillaDto villaDto)
		{
			if (id == null || id == 0 || id != villaDto.Id)
			{
				return BadRequest();
			}

			var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == id);

			villa.Id = villaDto.Id;
			villa.Nombre = villaDto.Nombre;
			villa.Ocupantes = villaDto.Ocupantes;
			villa.MetrosCuadrados = villaDto.MetrosCuadrados;

			return NoContent();

		}

		[HttpPatch("{id:int}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult UpdateParcialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
		{
			if (patchDto == null || id == 0)
			{
				return BadRequest();
			}

			var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == id);

			patchDto.ApplyTo(villa, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return NoContent();

		}
	}	
}
