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
		private readonly ApplicationDbContext _db;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
			_Logger = logger;
			_db = db;
		}

        [HttpGet]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<VillaDto>> GetVillas()
		{
			_Logger.LogInformation("Obteniendo villas");

			if (_db.Villas == null)
			{
				return NotFound();
			}

			return Ok(_db.Villas.ToList());
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

			//var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == Id);
			var villa = _db.Villas.FirstOrDefault(v => v.Id == Id);

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

			if (_db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
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

			//villaDto.Id = VillaStore.villasDtos.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

			//VillaStore.villasDtos.Add(villaDto);

			Villa modelo = new()
			{

				Nombre = villaDto.Nombre,
				Detalle = villaDto.Detalle,
				Tarifa = villaDto.Tarifa,
				Ocupantes = villaDto.Ocupantes,
				MetrosCudrados = villaDto.MetrosCuadrados,
				ImagenUrl = villaDto.ImagenUrl,
				Amenidad = villaDto.Amenidad
			};

			_db.Villas.Add(modelo);
			_db.SaveChanges();

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

			//var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == id);
			var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

			if (villa == null)
			{
				return NotFound();
			}

			//VillaStore.villasDtos.Remove(villa);

			_db.Villas.Remove(villa);
			_db.SaveChanges();

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

			//var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == id);

			Villa modelo = new()
			{
				Id = villaDto.Id,
				Nombre = villaDto.Nombre,
				Detalle = villaDto.Detalle,
				Tarifa = villaDto.Tarifa,
				Ocupantes = villaDto.Ocupantes,
				MetrosCudrados = villaDto.MetrosCuadrados,
				ImagenUrl = villaDto.ImagenUrl,
				Amenidad = villaDto.Amenidad
			};

			_db.Villas.Update(modelo);
					
			_db.SaveChanges();

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

			//var villa = VillaStore.villasDtos.FirstOrDefault(v => v.Id == id);
			var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

			if (villa == null) return BadRequest();

			VillaDto villaDto = new()
			{
				Id = villa.Id,
				Nombre = villa.Nombre,
				Detalle = villa.Detalle,
				Tarifa = villa.Tarifa,
				Ocupantes = villa.Ocupantes,
				MetrosCuadrados = villa.MetrosCudrados,
				ImagenUrl = villa.ImagenUrl,
				Amenidad = villa.Amenidad
			};

			patchDto.ApplyTo(villaDto, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Villa modelo = new()
			{
				Id = villaDto.Id,
				Nombre = villaDto.Nombre,
				Detalle = villaDto.Detalle,
				Tarifa = villaDto.Tarifa,
				Ocupantes = villaDto.Ocupantes,
				MetrosCudrados = villaDto.MetrosCuadrados,
				ImagenUrl = villaDto.ImagenUrl,
				Amenidad = villaDto.Amenidad
			};

			_db.Villas.Update(modelo);
			_db.SaveChanges();

			return NoContent();

		}
	}	
}
