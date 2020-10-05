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
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }
        private async void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            if (EntPassword.Text.Equals(EntConfirmPassword.Text))
            {
                await DisplayAlert("Contraseña no conincide", "Confirma tu contraseña", "Cancelar");
            }
            else
            {
                var response = await ApiService.RegisterUser(EntName.Text, EntEmail.Text, EntPassword.Text);
                if (response)
                {
                    await DisplayAlert("Hola", "Tu Cuenta ha sido Creada", "Aceptar");
                }
                else
                {
                    await DisplayAlert("Oops", "Algo salió mal", "Cancelar");
                }
            }
        }
    }
}