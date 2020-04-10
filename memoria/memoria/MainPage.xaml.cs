using memoria.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace memoria
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            main_image.Source = ImageSource.FromResource(
                    "memoria.Resources.brain.png"
            );
        }

        [Obsolete]
        async void Button_Clicked(object sender, EventArgs e)
        {
            var nextPage = new Juego();
            await Navigation.PushModalAsync(nextPage);
        }

    }
}
