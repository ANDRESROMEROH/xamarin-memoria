using memoria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using memoria.Utils;

namespace memoria.ViewModels
{
    class JuegoViewModel
    {
        Celda [,] celdas { get; set; }
        //static readonly int MAX_TEMP = 2;

        readonly string VALUES = 
            "AAABBBCCCDDDEEEFFFGGGHHHIIIJJJKKKLLLMMMNNNOOOPPPQQQRRRSSSTTTUUUVVVWWWXXXYYYZZZ";

        public JuegoViewModel() // modo por defecto 5x5
        {
            this.celdas = new Celda[6,5];
            Stack<char> valores = generarValores();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    char valor = valores.Pop();
                    celdas[i, j] = new Celda(valor, Celda.OCULTA);
                }
            }
        }

        private Stack<char> generarValores()
        {
            List<char> valores = ListUtils<char>.getRandomElements(
                    VALUES.ToList(),
                    30
            );

            return new Stack<char>(valores);
        }
    }
}
