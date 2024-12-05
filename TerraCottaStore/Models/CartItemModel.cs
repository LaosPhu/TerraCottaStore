namespace TerraCottaStore.Models
{
	public class CartItemModel
	{
		public int ProductID { get; set; }

		public string ProductName { get; set; }

		public int Quantati {  get; set; }

		public decimal Price { get; set; }

		public decimal PriceTotal { get
		 {
				return Quantati * Price;
		 }
		}
		public string image { get; set; }
		public CartItemModel()
		{

		}

		public CartItemModel(ProductModel product)
		{
			ProductID = product.Id;
			ProductName = product.Name;
			Price = product.Price;
			Quantati = product.Quantity;
			image = product.image;
		}
	}
}
