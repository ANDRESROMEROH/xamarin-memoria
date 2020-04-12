using memoria.Models;
using System.Collections.Generic;
using System.Linq;
using memoria.Utils;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Threading;
using memoria.Views;

namespace memoria.ViewModels
{
    [Flags]
    public enum Turno
    {
        Primero,  // 0
        Segundo,  // 1
    }

    class JuegoViewModel : BaseViewModel
    {
        public ObservableCollection<Celda> Celdas { get; set; }
        public Celda PrimeraCelda { get; set; }
        public Celda SegundaCelda { get; set; }
        public Turno NumTurno { get; set; }
        public ICommand JugarTurnoCommand { get; set; }

        readonly string VALUES = 
            "AABBCCDDEEFFGGHHIIJJKKLLMMNNOOPPQQRRSSTTUUVVWWXXYYZZ";

        public JuegoViewModel()
        {
            this.Celdas = new ObservableCollection<Celda>();
            Stack<char> valores = GenerarValores();

            for (int i = 0; i < 30; i++)
            {
                char valor = valores.Pop();
                Celdas.Add(new Celda(i.ToString(), valor.ToString(), Celda.OCULTA));
            }

            PrimeraCelda = null;
            SegundaCelda = null;
            NumTurno = Turno.Primero;
            JugarTurnoCommand = new Command(JugarTurno);
        }

        private void JugarTurno(object obj)
        {
            string id = (string)obj;
            Celda celda = Celdas.ElementAt(int.Parse(id));

            switch (NumTurno)
            {
                case Turno.Primero:
                    JugarPrimerTurno(celda);
                    break;
                case Turno.Segundo:
                    JugarSegundoTurno(celda);
                    break;
                default:
                    break;
            }
        }

        private void JugarPrimerTurno(Celda celda)
        {
            CambiarEstado(celda);
            PrimeraCelda = celda;
            NumTurno = Turno.Segundo;
        }

        private void JugarSegundoTurno(Celda celda)
        {
            CambiarEstado(celda);
            SegundaCelda = celda;
            ValidarSeleccionExitosa();
            ValidarFinJuego();
            NumTurno = Turno.Primero;
        }

        private void CambiarEstado(Celda celda)
        {
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

        private void ValidarSeleccionExitosa()
        {
            if (PrimeraCelda.ContenidoOculto == SegundaCelda.ContenidoOculto)
            {
                PrimeraCelda.HacerFija();
                SegundaCelda.HacerFija();
                RaisePropertyChanged("Celdas");
            } else
            {
                var task = Task.Run(() =>
                {
                    foreach (var celda in Celdas)
                    {
                        if (celda.Estado != Celda.FIJA)
                        {
                            celda.Ocultar();
                        }
                    }
                    Thread.Sleep(500); // half a second
                    RaisePropertyChanged("Celdas");
                });
            }
            PrimeraCelda = null;
            SegundaCelda = null;
        }

        private async void ValidarFinJuego()
        {
            bool final = Celdas.All((Celda celda) =>
            {
                return celda.Estado == Celda.FIJA;
            });

            if (final)
            {
                bool jugarDeNuevo = await Application.Current.MainPage.DisplayAlert(
                    "Felicidades 🎉", 
                    "Has ganado!!! 😀, Quieres volver a jugar ?", 
                    "Si", 
                    "No"
                );

                if (jugarDeNuevo)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new Juego());
                } 
                else
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                }
            }
        }

    }
}
