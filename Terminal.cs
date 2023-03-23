using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TerminalDeLibros
{
    class Terminal
    {
        // MENU PRINCIPAL //
        public static byte MostrarMenuPrincipal()
        {
            byte opcion = 0;

            do
            {
                Console.Clear();

                Console.WriteLine("============== Gestor de libros ==============");
                Console.WriteLine("1. Mostrar base de datos");
                Console.WriteLine("2. Modificar base de datos");
                Console.WriteLine("3. Búsqueda personalizada");
                Console.WriteLine("4. Salir del programa");
                Console.WriteLine("==============================================");

                try
                {
                    Console.Write("\nSeleccione una opción: ");
                    opcion = Convert.ToByte(Console.ReadLine());

                    if (opcion > 4 || opcion < 1)
                    {
                        Avisos.OpcionNoDisponible();  
                    }
                }
                catch (FormatException)
                {
                    Avisos.ErrorDeFormato();
                    return 0;
                }

            } while (opcion > 4 || opcion < 1);

            return opcion;
        }

        public static void MostrarBaseDeDatos(List<Libro> libros, in int cantidadLibros)
        {
            int opcion = 0;
            bool continuarBusqueda = true;

            if (cantidadLibros <= 0)
            {
                Console.Clear(); 
                Avisos.NingunRegitroDB();
                return;
            }

            do
            {
                try
                {
                    do
                    {
                        Console.Clear();

                        Console.WriteLine("================ Base de datos ===============");
                        BaseDeDatos.ListadoDeLibros(libros, cantidadLibros);
                        Console.WriteLine("==============================================");

                        Console.Write("\nSeleccione un libro o ingrese 0 para volver al menú " +
                                      "principal: ");
                        opcion = int.Parse(Console.ReadLine());

                        if (opcion < 0 || opcion > cantidadLibros)
                        {
                            Avisos.OpcionNoDisponible();
                        }

                    } while (opcion < 0 || opcion > cantidadLibros);
                }
                catch(FormatException)
                {
                    Avisos.ErrorDeFormato();
                    return;
                }

                if (opcion != 0)
                {
                    Console.Clear();
                    BaseDeDatos.MostrarInfoLibro(libros[opcion - 1]);
                    continuarBusqueda = SeleccionarOtroLibro();
                }
                else
                {
                    continuarBusqueda = false;
                }

            } while (continuarBusqueda);
        }

        // MODIFICACION DE LA BASE DE DATOS //
        public static void ModificarBaseDeDatos(List<Libro> libros, ref int cantidadLibros,
                                                string rutaFichero)
        {
            byte opcion = MenuModificarDB();

            switch (opcion)
            {
                case 1:
                    AgregarNuevoLibro(libros, ref cantidadLibros, rutaFichero);
                    break;

                case 2:
                    BorrarUnLibro(libros, ref cantidadLibros, rutaFichero);
                    break;

                case 3:
                    ModificarLibros(libros, cantidadLibros, rutaFichero); 
                    break;

                case 4:
                    return;
            }
        }

        public static void AgregarNuevoLibro(List<Libro> libros, ref int cantidadLibros,
                                             string rutaFichero)
        {
            Console.Clear();

            Console.WriteLine("============= Agregar nuevo libro =============");
            Console.Write("Título: "); 
            string titulo = Console.ReadLine();
            Console.Write("Autor: ");
            string autor = Console.ReadLine();
            Console.Write("Año de publicación: ");
            int anioPublicacion = int.Parse(Console.ReadLine());
            Console.Write("Tipo de obra: ");
            string tipoDeObra = Console.ReadLine();
            Console.Write("Género literario: ");
            string generoLiterario = Console.ReadLine();

            Libro l = new Libro(titulo, autor, anioPublicacion, tipoDeObra, generoLiterario);

            Console.WriteLine();
            BaseDeDatos.MostrarInfoLibro(l);

            if ( InformacionCorrecta() )
            {
                libros.Add(l);
                cantidadLibros++;

                Avisos.LibroAgregadoConExito();
                BaseDeDatos.GuardarLibros(libros, cantidadLibros, rutaFichero);
            }
            else
            {
                Avisos.InfoLibroDescartado();
            }
        }

        public static void BorrarUnLibro(List<Libro> libros, ref int cantidadLibros,
                                         string rutaFichero)
        {
            Console.Clear();
            Console.WriteLine("================= Borrar un libro =================");
            int indice = LibroABuscar(libros);

            if (indice >= 0)
            {
                if ( ConfirmarBorrarLibro(libros[indice]) )
                {
                    libros.RemoveAt(indice);
                    cantidadLibros--;
                    Avisos.LibroBorradoConExito();
                    BaseDeDatos.GuardarLibros(libros, cantidadLibros, rutaFichero);
                }
                else
                {
                    Avisos.CambiosDescartados();
                }
            }
            else if (indice == -1)
            {
                Avisos.LibroNoEncontrado();
            }
            else if (indice < -1)
            {
                return;
            }
        }

        public static void ModificarLibros(List<Libro> libros, int cantidadLibros,
                                            string rutaFichero)
        {
            Console.Clear();
            Console.WriteLine("================= Modificar un libro =================");
            int indice = LibroABuscar(libros);
            bool huboModificacion = false;

            if (indice >= 0)
            {
                ModificandoUnLibro(libros[indice], ref huboModificacion);
            }
            else if (indice == -1)
            {
                Avisos.LibroNoEncontrado();
            }
            else if (indice < -1)
            {
                return;
            }

            if (huboModificacion)
            {
                BaseDeDatos.GuardarLibros(libros, cantidadLibros, rutaFichero);
            }
        }

        public static int LibroABuscar(List<Libro> libros)
        {
            Console.Write("Escriba el título de la obra: ");
            string nombreObra = Console.ReadLine().ToUpper();

            bool huboError = false;
            foreach (Libro libro in libros)
            {
                if ( libro.Titulo.ToUpper().Contains(nombreObra) )
                {
                    Console.Clear();
                    BaseDeDatos.MostrarInfoLibro(libro);
                    bool encontrado = ConfirmarResultadoBusqueda(ref huboError);
                    if ( encontrado && !huboError)
                    {
                        return libros.IndexOf(libro);
                    }
                    else if (huboError)
                    {
                        return -2;
                    }
                }
            }

            return -1;
        }

        public static byte MenuModificarDB()
        {
            byte opcion = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("========== Modificar Base de Datos ===========");
                Console.WriteLine("1. Agregar nuevo libro");
                Console.WriteLine("2. Borrar un libro");
                Console.WriteLine("3. Modificar un libro");
                Console.WriteLine("4. Volver al menú principal");
                Console.WriteLine("==============================================");

                try
                {
                    Console.Write("\nSeleccione una opción: ");
                    opcion = byte.Parse(Console.ReadLine());
                }
                catch
                {
                    Avisos.ErrorDeFormato();
                    return 0;
                }

                if (opcion < 1 || opcion > 4)
                {
                    Avisos.OpcionNoDisponible();
                }
            } while (opcion < 1 || opcion > 4);

            return opcion;
        }
        
        static void MenuModificarLibro(string titulo)
        {
            Console.Clear();
            Console.WriteLine($"================= Modificando \"{titulo}\" =================");
            Console.WriteLine("1. Título");
            Console.WriteLine("2. Autor");
            Console.WriteLine("3. Año de publicación");
            Console.WriteLine("4. Tipo de obra");
            Console.WriteLine("5. Género literario");
            Console.WriteLine("6. Volver al menú principal");
            Console.WriteLine($"============================================================");
        }

        public static void ModificandoUnLibro(Libro libro, ref bool huboModificacion)
        {
            bool continuarModificando = true;
            bool huboError = false;
            do
            {
                MenuModificarLibro(libro.Titulo);
                byte opcion = SolicitarOpcion();
                switch (opcion)
                {
                    case 0: return;

                    case 1:
                        continuarModificando = ModificarTituloLibro(libro, ref huboModificacion,
                                                                    ref huboError);
                        break;

                    case 2:
                        continuarModificando = ModificarAutorLibro(libro, ref huboModificacion,
                                                                   ref huboError);
                        break;

                    case 3:
                        continuarModificando = ModificarPublicacionLibro(libro, ref huboModificacion,
                                                                         ref huboError);
                        break;
                        
                    case 4:
                        continuarModificando = ModificarTipoLibro(libro, ref huboModificacion,
                                                                  ref huboError);
                        break;

                    case 5:
                        continuarModificando = ModificarGeneroLibro(libro, ref huboModificacion,
                                                                    ref huboError);
                        break;

                    case 6:
                        continuarModificando = false;
                        break;
                }

                if (huboError)
                {
                    return;
                }

            } while (continuarModificando);
        }


        static bool ModificarTituloLibro(Libro libro, ref bool huboModificacion, 
                                         ref bool huboError)
        {
            Console.Write("Ingrese el nuevo título: ");
            string nuevoTitulo;
            try
            {
                nuevoTitulo = Console.ReadLine();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"\n¿Desea cambiar \"{libro.Titulo}\" por \"{nuevoTitulo}\"? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                libro.Titulo = nuevoTitulo;
                huboModificacion = true;
                Avisos.ModificacionExitosa();
                return SeguirModificando();

            }
            else
            {
                Avisos.ModificacionDescartada();
                return SeguirModificando();
            }
        }

        static bool ModificarAutorLibro(Libro libro, ref bool huboModificacion, 
                                        ref bool huboError)
        {
            Console.Write("Ingrese el nuevo autor: ");
            string nuevoAutor;
            try
            {
                nuevoAutor = Console.ReadLine();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"\n¿Desea cambiar \"{libro.Autor}\" por \"{nuevoAutor}\"? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                libro.Autor = nuevoAutor;
                huboModificacion = true;
                Avisos.ModificacionExitosa();
                return SeguirModificando();

            }
            else
            {
                Avisos.ModificacionDescartada();
                return SeguirModificando();
            }
        }

        static bool ModificarPublicacionLibro(Libro libro, ref bool huboModificacion, 
                                              ref bool huboError)
        {
            Console.Write("Ingrese el nuevo año de publicación: ");
            int nuevoAnio;
            try
            {
                nuevoAnio = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"\n¿Desea cambiar \"{libro.AnioPublicacion}\" por \"{nuevoAnio}\"? " +
                          $"(Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                libro.AnioPublicacion = nuevoAnio;
                huboModificacion = true;
                Avisos.ModificacionExitosa();
                return SeguirModificando();
            }
            else
            {
                Avisos.ModificacionDescartada();
                return SeguirModificando();
            }
        }

        static bool ModificarTipoLibro(Libro libro, ref bool huboModificacion, 
                                       ref bool huboError)
        {
            Console.Write("Ingrese el nuevo tipo de obra: ");
            string nuevoTipo;
            try
            {
                nuevoTipo = Console.ReadLine();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"\n¿Desea cambiar \"{libro.TipoDeObra}\" por \"{nuevoTipo}\"? " +
                          $"(Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                libro.TipoDeObra = nuevoTipo;
                huboModificacion = true;
                Avisos.ModificacionExitosa();
                return SeguirModificando();

            }
            else
            {
                Avisos.ModificacionDescartada();
                return SeguirModificando();
            }
        }

        static bool ModificarGeneroLibro(Libro libro, ref bool huboModificacion, 
                                       ref bool huboError)
        {
            Console.Write("Ingrese el nuevo género literario: ");
            string nuevoGenero;
            try
            {
                nuevoGenero = Console.ReadLine();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"\n¿Desea cambiar \"{libro.GeneroLiterario}\" por \"{nuevoGenero}\"? " +
                          $"(Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                libro.GeneroLiterario = nuevoGenero;
                huboModificacion = true;
                Avisos.ModificacionExitosa();
                return SeguirModificando();

            }
            else
            {
                Avisos.ModificacionDescartada();
                return SeguirModificando();
            }
        }

        // BÚSQUEDA PERSONALIZADA //
        public static void BusquedaPersonalizada(List<Libro> libros, int cantidadLibros)
        {
            bool continuarBuscando = true;
            do
            {
                MostrarMenuBusqueda();
                byte opcionUsuario = SolicitarOpcion();

                switch (opcionUsuario)
                {
                    case 0: 
                        return;

                    case 1:
                        continuarBuscando = BuscarTituloLibro(libros, cantidadLibros);
                        break;

                    case 2:
                        continuarBuscando = BuscarAutorLibro(libros, cantidadLibros);
                        break;

                    case 3:
                        continuarBuscando = BuscarAnioLibro(libros, cantidadLibros);
                        break;

                    case 4:
                        continuarBuscando = BuscarTipoLibro(libros, cantidadLibros);
                        break;

                    case 5:
                        continuarBuscando = BuscarGeneroLibro(libros, cantidadLibros);
                        break;

                    case 6:
                        continuarBuscando = false;
                        break;
                }

            } while (continuarBuscando);

        }

        static void MostrarMenuBusqueda()
        {
            Console.Clear();
            Console.WriteLine("============= Búsqueda personalizada =============");
            Console.WriteLine("1. Por título");
            Console.WriteLine("2. Por autor");
            Console.WriteLine("3. Por año de publicación");
            Console.WriteLine("4. Por tipo de obra");
            Console.WriteLine("5. Por género literario");
            Console.WriteLine("6. Volver a la terminal");
            Console.WriteLine("==================================================");
        }

        static bool BuscarTituloLibro(List<Libro> libros, int cantidadLibros)
        {
            Console.Clear();
            Console.WriteLine("============= Búsqueda personalizada =============");
            Console.Write("Ingrese el título de la obra: ");

            string tituloObra;
            try
            {
                tituloObra = Console.ReadLine().ToUpper();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            List<Libro> librosCoincidentes = new List<Libro>(cantidadLibros);
            bool encontrado = false;
            int resultados = 0;

            foreach (Libro l in libros)
            {
                if (l.Titulo.ToUpper().Contains(tituloObra))
                {
                    encontrado = true;
                    resultados++;
                    librosCoincidentes.Add(l);
                }
            }

            if (encontrado)
            {
                MostrarResultadosCoincidentes(librosCoincidentes, resultados);
                return SeguirBuscando();
            }
            else
            {
                Avisos.ResultadosNoCoincidentes();
                return SeguirBuscando();
            }
        }

        static bool BuscarAutorLibro(List<Libro> libros, int cantidadLibros)
        {
            Console.Clear();
            Console.WriteLine("============= Búsqueda personalizada =============");
            Console.Write("Ingrese el autor de la obra: ");

            string autorObra;
            try
            {
                autorObra = Console.ReadLine().ToUpper();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            List<Libro> librosCoincidentes = new List<Libro>(cantidadLibros);
            bool encontrado = false;
            int resultados = 0;

            foreach (Libro l in libros)
            {
                if (l.Autor.ToUpper().Contains(autorObra))
                {
                    encontrado = true;
                    resultados++;
                    librosCoincidentes.Add(l);
                }
            }

            if (encontrado)
            {
                MostrarResultadosCoincidentes(librosCoincidentes, resultados);
                return SeguirBuscando();
            }
            else
            {
                Avisos.ResultadosNoCoincidentes();
                return SeguirBuscando();
            }
        }

        static bool BuscarAnioLibro(List<Libro> libros, int cantidadLibros)
        {
            Console.Clear();
            Console.WriteLine("============= Búsqueda personalizada =============");
            Console.Write("Ingrese el año de publicación de la obra: ");

            int anioPublicacionObra;
            try
            {
                anioPublicacionObra = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            List<Libro> librosCoincidentes = new List<Libro>(cantidadLibros);
            bool encontrado = false;
            int resultados = 0;

            foreach (Libro l in libros)
            {
                if (l.AnioPublicacion == anioPublicacionObra)
                {
                    encontrado = true;
                    resultados++;
                    librosCoincidentes.Add(l);
                }
            }

            if (encontrado)
            {
                MostrarResultadosCoincidentes(librosCoincidentes, resultados);
                return SeguirBuscando();
            }
            else
            {
                Avisos.ResultadosNoCoincidentes();
                return SeguirBuscando();
            }
        }

        static bool BuscarTipoLibro(List<Libro> libros, int cantidadLibros)
        {
            Console.Clear();
            Console.WriteLine("============= Búsqueda personalizada =============");
            Console.Write("Ingrese el tipo de obra: ");

            string tipoDeObra;
            try
            {
                tipoDeObra = Console.ReadLine().ToUpper();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            List<Libro> librosCoincidentes = new List<Libro>(cantidadLibros);
            bool encontrado = false;
            int resultados = 0;

            foreach (Libro l in libros)
            {
                if (l.TipoDeObra.ToUpper().Contains(tipoDeObra))
                {
                    encontrado = true;
                    resultados++;
                    librosCoincidentes.Add(l);
                }
            }

            if (encontrado)
            {
                MostrarResultadosCoincidentes(librosCoincidentes, resultados);
                return SeguirBuscando();
            }
            else
            {
                Avisos.ResultadosNoCoincidentes();
                return SeguirBuscando();
            }
        }

        static bool BuscarGeneroLibro(List<Libro> libros, int cantidadLibros)
        {
            Console.Clear();
            Console.WriteLine("============= Búsqueda personalizada =============");
            Console.Write("Ingrese el género literario de la obra: ");

            string generoObra;
            try
            {
                generoObra = Console.ReadLine().ToUpper();
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            List<Libro> librosCoincidentes = new List<Libro>(cantidadLibros);
            bool encontrado = false;
            int resultados = 0;

            foreach (Libro l in libros)
            {
                if (l.GeneroLiterario.ToUpper().Contains(generoObra))
                {
                    encontrado = true;
                    resultados++;
                    librosCoincidentes.Add(l);
                }
            }

            if (encontrado)
            {
                MostrarResultadosCoincidentes(librosCoincidentes, resultados);
                return SeguirBuscando();
            }
            else
            {
                Avisos.ResultadosNoCoincidentes();
                return SeguirBuscando();
            }
        }
        static void MostrarResultadosCoincidentes(List<Libro> librosCoincidentes, int resultados)
        {
            Console.WriteLine($"\nResultados encontrados {resultados}");
            foreach (Libro libro in librosCoincidentes)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine(libro);
            }

        }

        static bool SeguirBuscando()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\n¿Continuar buscando? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // PEDIDOS AL USUARIO //
        static bool SeleccionarOtroLibro()
        {
            char respusta = 'n';

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\n¿Quiere seleccionar otro libro? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            try
            {
                respusta = char.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            if (respusta != 'y' && respusta != 'Y')
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static bool InformacionCorrecta()
        {
            char respusta = 'n';

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("¿La información es correcta? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            try
            {
                respusta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            if (respusta != 'y' && respusta != 'Y')
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static bool ConfirmarResultadoBusqueda(ref bool huboError)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("¿Este es el libro que busca? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");


            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                huboError = true;
                return false; 
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                return true;    
            }
            else
            {
                return false;
            }
        }

        static bool ConfirmarBorrarLibro(Libro libro)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\n¿Quiere borrar este libro de la base de datos? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';

            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static byte SolicitarOpcion()
        {
            byte opcion = 6;
            do
            {
                try
                {
                    Console.Write("\nSeleccione una opción: ");
                    opcion = byte.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Avisos.ErrorDeFormato();
                    return 0;
                }

                if (opcion < 1 || opcion > 6)
                {
                    Avisos.OpcionNoDisponible();
                    return 10;
                }

            } while (opcion < 1 || opcion > 6);

            return opcion;
        }

        static bool SeguirModificando()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("¿Continuar modificando el libro? (Y / N):");
            Console.ResetColor();
            Console.Write(" ");

            char respuesta = 'n';
            try
            {
                respuesta = char.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Avisos.ErrorDeFormato();
                return false;
            }

            if (respuesta == 'y' || respuesta == 'Y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
