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
        Tercero,  // 2
    }

    class JuegoViewModel : BaseViewModel
    {
        public ObservableCollection<Celda> Celdas { get; set; }
        public ObservableCollection<Celda> Temporales { get; set; }
        public ICommand JugarTurnoCommand { get; set; }
        public Celda CeldaEnJuego { get; set; }
        public Turno NumTurno { get; set; }

        readonly string VALUES = 
            "AAABBBCCCDDDEEEFFFGGGHHHIIIJJJKKKLLLMMMNNNOOOPPPQQQRRRSSSTTTUUUVVVWWWXXXYYYZZZ";

        public JuegoViewModel()
        {
            this.Celdas = new ObservableCollection<Celda>();
            this.Temporales = new ObservableCollection<Celda>();
            Stack<char> valores = GenerarValores();

            for (int i = 0; i < 30; i++)
            {
                char valor = valores.Pop();
                Celdas.Add(new Celda(i.ToString(), valor.ToString(), Celda.OCULTA));
            }

            CeldaEnJuego = null;
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
                case Turno.Tercero:
                    JugarTercerTurno(celda);
                    break;
                default:
                    break;
            }
        }

        private void JugarPrimerTurno(Celda celda)
        {
            CambiarEstado(celda);
            CeldaEnJuego = celda;
            Temporales.Add(celda);
            NumTurno = Turno.Segundo;
        }

        private void JugarSegundoTurno(Celda celda)
        {
            CambiarEstado(celda);
            ValidarCeldaActual(celda);
            NumTurno = Turno.Tercero;
        }

        private void JugarTercerTurno(Celda celda)
        {
            CambiarEstado(celda);
            ValidarCeldaActual(celda);
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

        private void ValidarCeldaActual(Celda celda)
        {
            if (celda.ContenidoOculto == CeldaEnJuego.ContenidoOculto)
            {
                Temporales.Add(celda);
            }
        }

        private void ValidarSeleccionExitosa()
        {
            if (Temporales.Count() == 3)
            {
                foreach (var celda in Temporales)
                {
                    celda.HacerFija();
                }
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
            Temporales.Clear();
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
