using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TerminalDeLibros
{
    class BaseDeDatos
    {
        public static bool VerificarBaseDeDatos(string rutaFichero)
        {
            // Esto verifica la no existencia del fichero.
            if ( !File.Exists(rutaFichero) )
            {
                char respuesta = Avisos.DBNotFound();
                
                if (respuesta == 'y' || respuesta == 'Y')
                {
                    CrearBaseDeDatos(rutaFichero);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool VerificarPrimeraLinea(string rutaFichero)
        {
            StreamReader lectorFlujo = File.OpenText(rutaFichero);
            string linea = lectorFlujo.ReadLine();
            lectorFlujo.Close();

            if (linea != null && linea != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CrearBaseDeDatos(string rutaFichero)
        {
            File.CreateText(rutaFichero).Close();
            Avisos.BaseDeDatosCreada();
        }

        public static List<Libro> CargarBaseDeDatos(string rutaFichero, ref int cantidadLibros)
        {
            List<Libro> libros = new List<Libro>();

            if (VerificarPrimeraLinea(rutaFichero))
            {
                StreamReader lectorFlujo = File.OpenText(rutaFichero);

                try
                {
                    cantidadLibros = int.Parse(lectorFlujo.ReadLine());
                }
                catch (FormatException)
                {
                    lectorFlujo.Close();
                    Avisos.ErrorDeFormatoBaseDeDatos();
                    return libros;
                }

                for (int i = 0; i < cantidadLibros; i++)
                {
                    string separador = lectorFlujo.ReadLine();
                    string titulo = lectorFlujo.ReadLine();
                    string autor = lectorFlujo.ReadLine();
                    int anioPublicacion = int.Parse(lectorFlujo.ReadLine());
                    string tipoDeObra = lectorFlujo.ReadLine();
                    string generoLiterario = lectorFlujo.ReadLine();

                    Libro l = new Libro(titulo, autor, anioPublicacion, tipoDeObra, 
                                        generoLiterario);

                    libros.Add(l);
                }

                lectorFlujo.Close();
            }

            Avisos.CargandoBaseDeDatos();
            return libros;
        }

        public static void GuardarLibros(List<Libro> libros, int cantidadLibros, string rutaFichero)
        {
            StreamWriter escritorFlujo = File.CreateText(rutaFichero);

            escritorFlujo.WriteLine(cantidadLibros);

            foreach (Libro libro in libros)
            {
                escritorFlujo.WriteLine();
                escritorFlujo.WriteLine(libro.Titulo);
                escritorFlujo.WriteLine(libro.Autor);
                escritorFlujo.WriteLine(libro.AnioPublicacion);
                escritorFlujo.WriteLine(libro.TipoDeObra);
                escritorFlujo.WriteLine(libro.GeneroLiterario);
            }

            Avisos.GuardandoInformacion();

            escritorFlujo.Close();
        }

        public static void ListadoDeLibros(List<Libro> libros, in int cantidadLibros)
        {
            for (int i = 0; i < cantidadLibros; i++)
            {
                Console.WriteLine($"{i + 1}. {libros[i].Titulo}");
            }
        }

        public static void MostrarInfoLibro(Libro libro)
        {
            Console.WriteLine("================= Información =================");
            Console.WriteLine($"Título: {libro.Titulo}");
            Console.WriteLine($"Autor: {libro.Autor}");
            Console.WriteLine($"Año de publicación: {libro.AnioPublicacion}");
            Console.WriteLine($"Tipo de obra: {libro.TipoDeObra}");
            Console.WriteLine($"Género literario: {libro.GeneroLiterario}");
            Console.WriteLine("===============================================");
        }
    }
}
