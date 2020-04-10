using System;
using System.Collections.Generic;
using System.Text;

namespace memoria.Models
{
    class Celda
    {
        public string Id { get; set; }
        public string ContenidoVisible  { get; set; }
        public string ContenidoOculto { get; set; }
        public int Estado { get; set; }

        public const int OCULTA = 0;
        public const int VISIBLE = 1;
        public const int TEMPORAL = 2;
        public const int FIJADA = 3;

        public Celda(String Id, String contenidoOculto, int estado)
        {
            this.Id = Id;
            this.ContenidoVisible = "";
            this.ContenidoOculto = contenidoOculto;
            this.Estado = estado;
        }

        public override string ToString()
        {
            return "contenido: " + this.ContenidoOculto + ", estado: " + this.Estado;
        }

        public void HacerVisible()
        {
            this.Estado = Celda.VISIBLE;
            this.ContenidoVisible = this.ContenidoOculto;
        }
        public void HacerTemporal()
        {
            this.Estado = Celda.TEMPORAL;
        }

        public void HacerFija()
        {
            this.Estado = Celda.FIJADA;
        }

        public void Ocultar()
        {
            this.Estado = Celda.OCULTA;
            this.ContenidoVisible = "";
        }
    }
}
