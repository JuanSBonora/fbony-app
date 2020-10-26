using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        public ProductDetailPage(int productId)
        {
            InitializeComponent();
            GetProductDetails(productId);
        }

        private async void GetProductDetails(int productId)
        {
           var product = await ApiService.GetProductById(productId);
            LblName.Text = product.name;
            LblDetail.Text = product.detail;
            ImgProduct.Source = product.FullImageUrl;
            LblPrice.Text = product.price.ToString();
            LblTotalPrice.Text = LblPrice.Text;
        }
    }
}