using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace TiendadeBelleza
{
    internal class UIEliminarUsuario
    {
        public static void Mostrar()
        {
            Console.Clear();
            Console.WriteLine("=== Eliminar Usuario ===\n");

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

            string ruta = null;
            var carpeta = BuscarCarpetaArchivos(AppContext.BaseDirectory);
            if (!string.IsNullOrEmpty(carpeta))
            {
                var p = Path.Combine(carpeta, "clientes.csv");
                if (File.Exists(p)) ruta = p;
            }

            if (ruta == null)
            {
                
                var posibles = new[] {
                    Path.Combine(Environment.CurrentDirectory, "Archivos", "clientes.csv"),
                    Path.Combine(Environment.CurrentDirectory, "archivos", "clientes.csv"),
                    Path.Combine(AppContext.BaseDirectory, "Archivos", "clientes.csv"),
                    Path.Combine(AppContext.BaseDirectory, "archivos", "clientes.csv"),
                    Path.Combine("Archivos", "clientes.csv"),
                    Path.Combine("archivos", "clientes.csv")
                };
                ruta = posibles.FirstOrDefault(File.Exists);
            }

            if (ruta == null)
            {
                Console.WriteLine("No se encontró la base de datos de clientes. Cree primero un usuario.");
                Console.WriteLine("Presione Enter para volver al menú...");
                Console.ReadLine();
                return;
            }

            Console.Write("Ingrese la cédula del usuario a eliminar: ");
            string cedula = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(cedula))
            {
                Console.WriteLine("Cédula inválida.");
                Console.WriteLine("Presione Enter para volver al menú...");
                Console.ReadLine();
                return;
            }

            try
            {
                var lineas = File.ReadAllLines(ruta, Encoding.UTF8).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
                if (lineas.Count == 0)
                {
                    Console.WriteLine("El archivo de clientes está vacío.");
                    Esperar();
                    return;
                }

                // Keep header (first line)
                var header = lineas[0];
                var registros = lineas.Skip(1).ToList();

                bool encontrado = false;
                var nuevos = new List<string>();

                foreach (var linea in registros)
                {
                    var primera = ParseFirstField(linea)?.Trim();
                    if (primera != null && primera.Equals(cedula, StringComparison.OrdinalIgnoreCase))
                    {
                        encontrado = true;
                        continue; 
                    }
                    nuevos.Add(linea);
                }

                if (!encontrado)
                {
                    Console.WriteLine("No se encontró un usuario con esa cédula.");
                    Esperar();
                    return;
                }

                
                var salida = new List<string> { header };
                salida.AddRange(nuevos);
                File.WriteAllLines(ruta, salida, Encoding.UTF8);

                Console.WriteLine("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
            }

            Esperar();
        }

        private static string ParseFirstField(string linea)
        {
            if (linea == null) return null;
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < linea.Length && linea[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++; 
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    break;
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Trim();
        }

        private static void Esperar()
        {
            Console.WriteLine("\nPresione Enter para volver al menú...");
            Console.ReadLine();
        }
    }
}
