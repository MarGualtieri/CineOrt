using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BENT1A.Grupo2.Models;
using Microsoft.EntityFrameworkCore;
using Reserva_Cine.Models;

namespace BENT1A.Grupo2.Database
{
    public class CineDbContext : DbContext
    {
     


        public CineDbContext(DbContextOptions<CineDbContext> options ) : base(options)
        {
        }

       

        #region DbSets
        
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<TipoSala> TipoSalas{ get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
      

        #endregion
    }
}
