using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KobeTown.Models
{
	public class CartItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
		public int Quantity { get; set; }
		public string Category { get; set; }
		public decimal Money
		{
			get
			{
				return Quantity * Price;
			}
		}
	}
}