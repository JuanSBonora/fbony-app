using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<PopularProduct> ProductsCollection;
        public ObservableCollection<Category> CategoriesCollection;
        public HomePage()
        {
            InitializeComponent();
            ProductsCollection = new ObservableCollection<PopularProduct>();
            CategoriesCollection = new ObservableCollection<Category>();
            GetPopularProducts();
            GetCategories();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
        }

        private async void GetCategories()
        {
            var categories = await ApiService.GetCategories();
            foreach (var category in categories)
            {
                CategoriesCollection.Add(category);
            }
            CvCategories.ItemsSource = CategoriesCollection;
        }

        private async void GetPopularProducts()
        {
            var products = await ApiService.GetPopularProducts();
            foreach (var product in products)
            {
                ProductsCollection.Add(product);
            }
            CvProducts.ItemsSource = ProductsCollection;
        }

        private async void ImgMenu_Tapped(object seder, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }
        private async void TapCloseMenu_Tapped(object seder, EventArgs e)
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("accessToken", string.Empty);
            Preferences.Set("tokenExpirationTime", 0);
            Application.Current.MainPage = new NavigationPage(new SignupPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var response = await ApiService.GetTotalCartItems(Preferences.Get("userId", 0));
            LblTotalItems.Text = response.totalItems.ToString();
        }

        private void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as Category;
            if (currentSelection == null) return; 
            Navigation.PushModalAsync(new ProductListPage(currentSelection.id,currentSelection.name));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}