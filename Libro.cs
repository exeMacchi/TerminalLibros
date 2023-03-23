using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalDeLibros
{
    class Libro
    {
        public Libro(string titulo, string autor, int anioPublicacion, string tipoDeObra,
                     string generoLiterario)
        {
            this.titulo = titulo;
            this.autor = autor;
            this.anioPublicacion = anioPublicacion;
            this.tipoDeObra = tipoDeObra;
            this.generoLiterario = generoLiterario;
        }

        private string titulo;
        private string autor;
        private int    anioPublicacion;
        private string tipoDeObra;
        private string generoLiterario;

        public string Titulo { get { return titulo; } set { titulo = value; } }
        public string Autor { get { return autor;} set { autor = value; } }

        public int AnioPublicacion 
        { 
            get { return anioPublicacion;} 
            set { anioPublicacion = value; } 
        }

        public string TipoDeObra 
        { 
            get { return tipoDeObra; } 
            set { tipoDeObra = value; } 
        }

        public string GeneroLiterario
        {
            get { return generoLiterario; }
            set { generoLiterario = value; }
        }

        public override string ToString()
        {
            return ($"Título: {titulo}\n" +
                    $"Autor: {autor}\n" +
                    $"Año de publicación: {anioPublicacion}\n" +
                    $"Tipo de obra: {tipoDeObra}\n" +
                    $"Género literario: {generoLiterario}");
        }
    }
}
