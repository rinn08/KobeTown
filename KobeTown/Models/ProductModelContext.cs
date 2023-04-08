using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace KobeTown.Models
{
	public partial class ProductModelContext : DbContext
	{
		public ProductModelContext()
			: base("name=ProductModelContext1")
		{
		}

		public virtual DbSet<OrderDetaill> OrderDetaills { get; set; }
		public virtual DbSet<Orderr> Orderrs { get; set; }
		public virtual DbSet<Products> Productss { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{

			modelBuilder.Entity<OrderDetaill>()
				.Property(e => e.Price)
				.HasPrecision(18, 0);

			modelBuilder.Entity<Products>()
				.Property(e => e.Description)
				.IsUnicode(false);

			//modelBuilder.Entity<Products>()
			//	.HasMany(e => e.OrderDetaills)
			//	.WithRequired(e => e.Products)
			//	.WillCascadeOnDelete(false);
				
		}
	}
}
