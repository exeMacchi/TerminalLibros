using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalDeLibros
{
    class Program
    {
        static void Main()
        {
            string rutaFichero = Path.Combine(Directory.GetCurrentDirectory(), "librosDB.txt");
            if ( !BaseDeDatos.VerificarBaseDeDatos(rutaFichero) )
            {
                return;
            }

            int cantidadLibros = 0;
            List<Libro> libros = BaseDeDatos.CargarBaseDeDatos(rutaFichero, ref cantidadLibros);
            byte opcionUsuario = 0;

            bool salida = false;
            while (!salida)
            {
                switch(opcionUsuario)
                {
                    case 0: 
                        opcionUsuario = Terminal.MostrarMenuPrincipal();
                        break;

                    case 1:
                        Terminal.MostrarBaseDeDatos(libros, cantidadLibros);
                        opcionUsuario = 0;
                        break;

                    case 2:
                        Terminal.ModificarBaseDeDatos(libros, ref cantidadLibros, rutaFichero);
                        opcionUsuario = 0;
                        break;

                    case 3:
                        Terminal.BusquedaPersonalizada(libros, cantidadLibros);
                        opcionUsuario = 0;
                        break;

                    case 4:
                        BaseDeDatos.GuardarLibros(libros, cantidadLibros, rutaFichero);
                        Avisos.SaliendoDelPrograma();
                        salida = true;
                        break;

                    default:
                        Console.WriteLine("Algo ha ocurrido...");
                        Console.ReadKey();
                        opcionUsuario = 0;
                        break;
                }
            }
        }
    }
}
