using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Models;

namespace TiendaWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProducto> PedidoProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RELACION MUCHOS A MUCHOS
            modelBuilder.Entity<PedidoProducto>()
                .HasKey(pp => new { pp.PedidoId, pp.ProductoId });

            modelBuilder.Entity<PedidoProducto>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.PedidoProductos)
                .HasForeignKey(pp => pp.PedidoId);

            modelBuilder.Entity<PedidoProducto>()
                .HasOne(pp => pp.Producto)
                .WithMany(p => p.PedidoProductos)
                .HasForeignKey(pp => pp.ProductoId);

            modelBuilder.Entity<PedidoProducto>()
            .Property(p => p.PrecioUnitario)
            .HasPrecision(10,2);
            }
    }
}