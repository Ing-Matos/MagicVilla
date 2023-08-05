using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre= "Villa Real",
                    Detalle= "Detalle de la Vila...",
                    Tarifa= 200,
                    Ocupantes=5,
                    MetrosCudrados= 50,
                    ImagenUrl="",
                    Amenidad="",
                    FechaCreacion= DateTime.Now,
                    FechaActualizacion= DateTime.Now
                },
				new Villa()
				{
					Id = 2,
					Nombre = "Primium Villa Real",
					Detalle = "Detalle de la Vila...",
					Tarifa = 400,
					Ocupantes = 10,
					MetrosCudrados = 100,
					ImagenUrl = "",
					Amenidad = "",
					FechaCreacion = DateTime.Now,
					FechaActualizacion = DateTime.Now
				}

			);
        }
    }
}
