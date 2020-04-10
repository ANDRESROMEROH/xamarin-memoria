using memoria.Models;
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
        JuegoViewModel ViewModel;

        public Juego()
        {
            InitializeComponent();
            this.ViewModel = new JuegoViewModel();
            BindingContext = this.ViewModel;
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            List<Button> btns = new List<Button>();

            for (int i = 0; i < ViewModel.Celdas.Count(); i++)
            {
                Button btn = new Button
                {
                    Command = this.ViewModel.JugarTurnoCommand,
                    CommandParameter = ViewModel.Celdas[i].Id
                };

                Binding bindingContenido = new Binding($"Celdas[{i}].ContenidoVisible");
                Binding bindingHabilitada = new Binding($"Celdas[{i}].Habilitada");

                bindingContenido.Source = ViewModel;
                bindingHabilitada.Source = ViewModel;

                btn.SetBinding(Button.TextProperty, bindingContenido);
                btn.SetBinding(Button.IsEnabledProperty, bindingHabilitada);
                btns.Add(btn);
            }

            this.AddButtonsToGrid(new Stack<Button>(btns));
        }

        private void AddButtonsToGrid(Stack<Button> btns)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    Grid.Children.Add(btns.Pop(), i, k);
                }
            }
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine(((Button)sender).Text);
        //}
    }
}