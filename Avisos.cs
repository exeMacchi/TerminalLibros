using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalDeLibros
{
    class Avisos
    {
        // Procesos //
        public static void CargandoBaseDeDatos()
        {
            Console.WriteLine("Preparando base de datos...");

            System.Threading.Thread.Sleep(2000);

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Programa listo para ser utilizado.");
            Console.ResetColor();
            Console.WriteLine("Presione 'Intro' para entrar en el menú principal.");
            Console.ReadKey();
        }

        public static void GuardandoInformacion()
        {
            Console.Clear();
            Console.WriteLine("Guardando información en la base de datos...");
            System.Threading.Thread.Sleep(1500);

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("¡Información guardada con éxito!");
            Console.ResetColor();
            Console.WriteLine("Presione 'Intro' para continuar.");
            Console.ReadKey();
        }

        public static void SaliendoDelPrograma()
        {
            Console.Clear();
            Console.WriteLine("Saliendo del programa...");
            System.Threading.Thread.Sleep(1000);
        }
        
        // Correctos //
        public static void BaseDeDatosCreada()
        {
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("¡Nueva base de datos creada con éxito!");
            Console.ResetColor();

            Console.WriteLine("Presione 'Intro' para ir al menú principal de la terminal.");
            Console.ReadKey();

            Console.Clear();
        }

        public static void LibroAgregadoConExito()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("¡Libro añadido con éxito a la base de datos!");
            Console.ResetColor();

            Console.WriteLine("Presione 'Intro' para volver al menú principal.");
            Console.ReadKey();
        }

        public static void LibroBorradoConExito()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("¡Libro eliminado con éxito de la base de datos!");
            Console.ResetColor();

            Console.WriteLine("Presione 'Intro' para volver al menú principal.");
            Console.ReadKey();
        }

        public static void ModificacionExitosa()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("!Modificación exitosa!");
            Console.ResetColor();
        }

        // Avisos //
        public static void NingunRegitroDB()
        {
            Console.WriteLine("================ Base de datos ===============");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Por el momento, no hay ningún libro registrado en la base " +
                              "de datos.");
            Console.ResetColor();
            Console.WriteLine("==============================================");
            Console.WriteLine("Presione 'Intro' para volver al menú principal.");
            Console.ReadKey();
        }

        public static void InfoLibroDescartado()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("La información del libro será descartada.");
            Console.ResetColor();

            Console.WriteLine("Presione 'Intro' para volver al menú principal.");
            Console.ReadKey();
        }

        public static void CambiosDescartados()
        {
            Console.BackgroundColor= ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Cambios descartados.");
            Console.ResetColor();
            Console.WriteLine("Pulse 'Intro' para volver al menú principal.");
            Console.ReadKey();
        }

        public static void ModificacionDescartada()
        {
            Console.BackgroundColor= ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Cambios descartados.");
            Console.ResetColor();

        }


        // Errores //
        public static char DBNotFound()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Error: no se ha encontrado la base de datos en la dirección " +
                              "especificada.");
            Console.ResetColor();

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("¿Desea crear una nueva base de datos en la ruta indicada? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = char.Parse(Console.ReadLine());

            return respuesta;
        }

        public static void LibroNoEncontrado()
        {
            Console.BackgroundColor= ConsoleColor.DarkRed;
            Console.ForegroundColor= ConsoleColor.Black;
            Console.WriteLine("Los datos ingresados no concuerdan con ningún libro en la " +
                              "base de datos." +
                              "\nVerifique que los datos sean correctos o que el libro " +
                              "esté registrado en la base de datos.");
            Console.ResetColor();
            Console.WriteLine("Pulse 'Intro' para volver al menú principal.");
            Console.ReadKey();
        }
        
        public static void ResultadosNoCoincidentes()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("No se han encontrado resultados coincidentes.");
            Console.ResetColor();
        }

        public static void OpcionNoDisponible()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Error: opción no disponible. Por favor, seleccione una " +
                              "opción válida.");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void ErrorDeFormato()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Error: formato incorrecto.");
            Console.ResetColor();
            Console.WriteLine("Presione 'Intro' para volver al menú principal.");
            Console.ReadKey();
        }
        
        public static void ErrorDeFormatoBaseDeDatos()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Error: lectura incorrecta de la estructura esperada de la " +
                              "base de datos.");
            Console.ResetColor();
            Console.ReadKey();

        }

    }
}
