using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoRent.Domain
{
    public class Pelicula
    {
        private int peliculaId;
        private String titulo;
        private bool subtitulada;
        private bool estreno;
        private Genero genero;
        private List<Actor> actores;
        public Pelicula()
        {
            this.genero = new Genero();
            this.actores = new List<Actor>();
        }

        public Pelicula(int peliculaId, string titulo, bool subtitulada, bool estreno,
            Genero genero, List<Actor> actores)
        {
            this.peliculaId = peliculaId;
            this.titulo = titulo;
            this.subtitulada = subtitulada;
            this.estreno = estreno;
            this.genero = genero;
            this.actores = actores;
        }

        public int PeliculaId { get => peliculaId; set => peliculaId = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public bool Subtitulada { get => subtitulada; set => subtitulada = value; }
        public bool Estreno { get => estreno; set => estreno = value; }
        public Genero Genero { get => genero; set => genero = value; }
        public List<Actor> Actores { get => actores; set => actores = value; }
    }
}
