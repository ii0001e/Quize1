using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Quize1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            Button Quize = new Button
            {
                Text = "English quiz",
                BackgroundColor = Color.IndianRed

            };
            Quize.Clicked += Quize_Clicked;

            StackLayout st = new StackLayout
            {
                Children = { Quize },
                BackgroundColor = Color.Green
            };
            Content = st;





        }

        private async void Quize_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QuizePage());
        }
    }
}
