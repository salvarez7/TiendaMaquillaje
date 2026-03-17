using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TiendadeBelleza
{
    internal class UICrearUsuario
    {
        public static void Mostrar()
        {
            string BuscarCarpetaArchivos(string inicio)
            {
                var dir = new DirectoryInfo(inicio);
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    var encontrado = dir.GetDirectories().FirstOrDefault(d => string.Equals(d.Name, "Archivos", StringComparison.OrdinalIgnoreCase) || string.Equals(d.Name, "archivos", StringComparison.OrdinalIgnoreCase));
                    if (encontrado != null)
                        return encontrado.FullName;

                    dir = dir.Parent;
                }
                return null;
            }

            var carpeta = BuscarCarpetaArchivos(AppContext.BaseDirectory);
            string ruta = null;
            if (!string.IsNullOrEmpty(carpeta))
            {
                var p = Path.Combine(carpeta, "clientes.csv");
                if (File.Exists(p) || Directory.Exists(carpeta)) ruta = p;
            }

            if (ruta == null)
            {
                var posiblesRutas = new[] {
                    Path.Combine(Environment.CurrentDirectory, "Archivos", "clientes.csv"),
                    Path.Combine(Environment.CurrentDirectory, "archivos", "clientes.csv"),
                    Path.Combine(AppContext.BaseDirectory, "Archivos", "clientes.csv"),
                    Path.Combine(AppContext.BaseDirectory, "archivos", "clientes.csv"),
                    Path.Combine("Archivos", "clientes.csv"),
                    Path.Combine("archivos", "clientes.csv")
                };

                ruta = posiblesRutas.FirstOrDefault(p => File.Exists(p) || Directory.Exists(Path.GetDirectoryName(p)));
            }

            // If still null, default to Archivos/clientes.csv relative to current directory
            if (ruta == null)
            {
                ruta = Path.Combine(Environment.CurrentDirectory, "Archivos", "clientes.csv");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(ruta) ?? ".");

            Console.Clear();
            Console.WriteLine("Crear usuario\n");

            string cedula;
            do
            {
                Console.Write("Ingrese la cédula: ");
                cedula = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(cedula)) Console.WriteLine("La cédula no puede estar vacía.");
            } while (string.IsNullOrEmpty(cedula));

            string nombre;
            do
            {
                Console.Write("Ingrese el nombre: ");
                nombre = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(nombre)) Console.WriteLine("El nombre no puede estar vacío.");
            } while (string.IsNullOrEmpty(nombre));

            string ciudad;
            do
            {
                Console.Write("Ingrese la ciudad: ");
                ciudad = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(ciudad)) Console.WriteLine("La ciudad no puede estar vacía.");
            } while (string.IsNullOrEmpty(ciudad));

            bool necesitaEncabezado = !File.Exists(ruta);

            string Escape(string s)
            {
                if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
                {
                    return '"' + s.Replace("\"", "\"\"") + '"';
                }
                return s;
            }

            var fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string registro = $"{Escape(cedula)},{Escape(nombre)},{Escape(ciudad)},{Escape(fecha)}";

            try
            {
                if (necesitaEncabezado)
                {
                    // Create file with header and first record
                    using (var sw = new StreamWriter(ruta, append: false, encoding: Encoding.UTF8))
                    {
                        sw.WriteLine("Cedula,Nombre,Ciudad,Fecha");
                        sw.WriteLine(registro);
                    }
                }
                else
                {
                    // If file exists, ensure we start on a new line when appending
                    bool endsWithNewLine = true;
                    using (var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (fs.Length > 0)
                        {
                            fs.Seek(-1, SeekOrigin.End);
                            int last = fs.ReadByte();
                            endsWithNewLine = last == '\n' || last == '\r';
                        }
                    }

                    using (var sw = new StreamWriter(ruta, append: true, encoding: Encoding.UTF8))
                    {
                        if (!endsWithNewLine) sw.WriteLine();
                        sw.WriteLine(registro);
                    }
                }

                Console.WriteLine($"\nUsuario guardado en: {ruta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar: {ex.Message}");
            }

            Console.WriteLine("Presione Enter para volver al menú...");
            Console.ReadLine();
        }
    }
}
