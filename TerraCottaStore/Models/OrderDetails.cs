﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TerraCottaStore.Models
{
	public class OrderDetails
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string OrderCode { get; set; }
		public int ProductId { get; set; }

		public decimal  Price { get; set; }

		public int Quantati {  get; set; }
		[ForeignKey("ProductId")]
		public ProductModel Product { get; set; }

	}
}
