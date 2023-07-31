using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Datos
{
	public static class VillaStore
	{
		public static List<VillaDto> villasDtos = new List<VillaDto>
		{
			new VillaDto{Id=1, Nombre="Vista a la Piscina", Ocupantes=3, MetrosCuadrados=300},
			new VillaDto{Id=2, Nombre="Vista a la Playa", Ocupantes=4, MetrosCuadrados=400}
		};
	}
}
