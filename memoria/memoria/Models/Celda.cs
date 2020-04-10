using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace memoria.Models
{
    class Celda
    {
        public string Id { get; set; }
        public string ContenidoVisible  { get; set; }
        public string ContenidoOculto { get; set; }
        public int Estado { get; set; }
        public Color ColorEstado { get; set; }
        public bool Habilitada { get; set; }

        public const int OCULTA = 0;
        public const int VISIBLE = 1;
        public const int FIJA = 2;

        public Celda(String Id, String contenidoOculto, int estado)
        {
            this.Id = Id;
            this.ContenidoVisible = "";
            this.ContenidoOculto = contenidoOculto;
            this.Estado = estado;
            this.Habilitada = true;
            this.ColorEstado = Color.Transparent;
        }

        public override string ToString()
        {
            return "contenido: " + this.ContenidoOculto + ", estado: " + this.Estado;
        }

        public void HacerVisible()
        {
            this.Estado = Celda.VISIBLE;
            this.ContenidoVisible = this.ContenidoOculto;
            this.Habilitada = false;
            this.ColorEstado = Color.Yellow;
        }

        public void HacerFija()
        {
            this.Estado = Celda.FIJA;
            this.ColorEstado = Color.LightGreen;
        }

        public void Ocultar()
        {
            this.Estado = Celda.OCULTA;
            this.ContenidoVisible = "";
            this.Habilitada = true;
            this.ColorEstado = Color.Transparent;
        }
    }
}
