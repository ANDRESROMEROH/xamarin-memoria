using memoria.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace memoria.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Juego : ContentPage
    {
        JuegoViewModel viewModel;

        public Juego()
        {
            InitializeComponent();
            this.viewModel = new JuegoViewModel();
        }
    }
}