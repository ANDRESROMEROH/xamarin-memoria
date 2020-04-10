using memoria.Models;
using System.Collections.Generic;
using System.Linq;
using memoria.Utils;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace memoria.ViewModels
{
    class JuegoViewModel : BaseViewModel
    {
        public ObservableCollection<Celda> Celdas { get; set; }
        public ICommand CambiarEstadoCommand { get; set; }

        readonly string VALUES = 
            "AAABBBCCCDDDEEEFFFGGGHHHIIIJJJKKKLLLMMMNNNOOOPPPQQQRRRSSSTTTUUUVVVWWWXXXYYYZZZ";

        public JuegoViewModel()
        {
            this.Celdas = new ObservableCollection<Celda>();
            Stack<char> valores = GenerarValores();

            for (int i = 0; i < 30; i++)
            {
                char valor = valores.Pop();
                Celdas.Add(new Celda(i.ToString(), valor.ToString(), Celda.OCULTA));
            }
            
            CambiarEstadoCommand = new Command(CambiarEstado);
        }

        private void CambiarEstado(object obj)
        {
            string id = (string) obj;
            Celda celda = Celdas.ElementAt(int.Parse(id));

            switch (celda.Estado)
            {
                case Celda.OCULTA:
                    celda.HacerVisible();
                    break;

                case Celda.VISIBLE:
                    celda.Ocultar();
                    break;

                default:
                    break;
            }

            
            RaisePropertyChanged("Celdas");
        }

        private Stack<char> GenerarValores()
        {
            List<char> valores = ListUtils<char>.getRandomElements(VALUES.ToList(), 30);
            return new Stack<char>(valores);
        }

    }
}
