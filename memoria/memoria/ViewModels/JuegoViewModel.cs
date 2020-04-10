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
        public ICommand JugarTurnoCommand { get; set; }
        public Celda CeldaEnJuego { get; set; }
        public Turno NumTurno { get; set; }

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
            celda.HacerTemporal();
            CeldaEnJuego = celda;
            NumTurno = Turno.Segundo;
        }

        private void JugarSegundoTurno(Celda celda)
        {
            CambiarEstado(celda);
            ValidarCeldaActual(celda);
            NumTurno = celda.ContenidoOculto == CeldaEnJuego.ContenidoOculto? Turno.Tercero : Turno.Primero;
        }

        private void JugarTercerTurno(Celda celda)
        {
            CambiarEstado(celda);
            ValidarCeldaActual(celda);
            ValidarSeleccionExitosa();
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

        private void ValidarCeldaActual(Celda celdaActual)
        {
            if (celdaActual.ContenidoOculto == CeldaEnJuego.ContenidoOculto)
            {
                celdaActual.HacerTemporal();
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
        }

        private void ValidarSeleccionExitosa()
        {
            foreach (var celda in Celdas)
            {
                if (celda.Estado == Celda.TEMPORAL)
                {
                    celda.HacerFija();
                }
            }
        }

    }
}
