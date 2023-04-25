using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KobeTown.Models
{
	public class Categories
	{
		public Categories()
		{
			products = new HashSet<Products>();
		}

		public int CategoryId { get; set; }

		[Required]
		[StringLength(50)]
		public string CategoryName { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Products> products { get; set; }
	}
}
