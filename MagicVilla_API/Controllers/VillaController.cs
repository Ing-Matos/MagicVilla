using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaController : ControllerBase
	{
		[HttpGet]
		public ActionResult<IEnumerable<VillaDto>> GetVillas()
		{
			if (VillaStore.villasDtos == null)
			{
				return NotFound();
			}

			return Ok(VillaStore.villasDtos);
		}

		[HttpGet("{Id:int}")]
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
	}
}
