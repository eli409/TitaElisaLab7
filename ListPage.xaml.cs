using TitaElisaLab7.Models;
namespace TitaElisaLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
        InitializeComponent();
    }

	async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		var slist = (ShopList)BindingContext;
		slist.Date = DateTime.UtcNow;

		Shop selectedShop = (ShopPicker.SelectedItem as Shop);
		slist.ShopID = selectedShop.ID;

		await App.Database.SaveShopListAsync(slist);
		await Navigation.PopAsync();
	}
	async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		var slist = (ShopList)BindingContext;
		await App.Database.DeleteShopListAsync(slist);
		await Navigation.PopAsync();
	}
	async void OnChooseButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
		{
			BindingContext = new Product()
		});
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var items = await App.Database.GetShopsAsync();
		ShopPicker.ItemsSource = (System.Collections.IList)items;
		ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

		var shopl = (ShopList)BindingContext;
		listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
	}
	async void OnDeleteItemClicked(object sender, EventArgs e)
	{
		var selectedProduct = listView.SelectedItem as Product;
		var shopList = (ShopList)BindingContext;

		if (selectedProduct != null)
		{
			await App.Database.DeleteListProductByIdsAsync(shopList.ID, selectedProduct.ID);
			listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
		}
	}
}