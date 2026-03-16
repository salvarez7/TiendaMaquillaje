using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TiendadeBelleza
{
    internal class UIIngresarUsuario
    {
        public static void Mostrar()
        {
            Console.Clear();
            Console.WriteLine("=== Ingresar (Buscar) Usuario ===\n");

            // 1. Obtener la ruta del archivo
            string ruta = ObtenerRutaArchivo();

            // 2. Validar existencia del archivo
            if (string.IsNullOrEmpty(ruta) || !File.Exists(ruta))
            {
                Console.WriteLine("⚠️ No se encontró la base de datos de clientes.");
                Console.WriteLine("Por favor, cree un usuario primero (Opción 1).");
                EsperarEnter();
                return;
            }

            // 3. Capturar cédula a buscar
            Console.Write("Ingrese la cédula del usuario: ");
            string cedulaBusqueda = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(cedulaBusqueda))
            {
                Console.WriteLine("❌ Cédula inválida.");
                EsperarEnter();
                return;
            }

            // 4. Leer y procesar el archivo
            try
            {
                var lineas = File.ReadAllLines(ruta, Encoding.UTF8)
                                 .Where(l => !string.IsNullOrWhiteSpace(l))
                                 .ToList();

                bool encontrado = false;

                // Saltamos el encabezado (asumiendo que la primera línea es Cedula,Nombre,Ciudad)
                foreach (var linea in lineas.Skip(1))
                {
                    var campos = ParsearLineaCSV(linea);

                    if (campos.Count > 0 && campos[0].Trim().Equals(cedulaBusqueda, StringComparison.OrdinalIgnoreCase))
                    {
                        string nombre = campos.Count > 1 ? campos[1] : "No registrado";
                        string ciudad = campos.Count > 2 ? campos[2] : "No registrada";

                        Console.WriteLine("\n✅ ¡Usuario encontrado!");
                        Console.WriteLine("-----------------------------");
                        Console.WriteLine($"Nombre: {nombre}");
                        Console.WriteLine($"Ciudad: {ciudad}");
                        Console.WriteLine("-----------------------------");
                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado)
                {
                    Console.WriteLine("\n❌ Usuario no encontrado. Verifique la cédula o cree el registro.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error al leer los datos: {ex.Message}");
            }

            EsperarEnter();
        }

        // --- MÉTODOS DE APOYO ---

        private static List<string> ParsearLineaCSV(string linea)
        {
            var resultado = new List<string>();
            var sb = new StringBuilder();
            bool enComillas = false;

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];
                if (c == '"')
                {
                    if (enComillas && i + 1 < linea.Length && linea[i + 1] == '"')
                    {
                        sb.Append('"'); // Doble comilla escapada
                        i++;
                    }
                    else enComillas = !enComillas;
                }
                else if (c == ',' && !enComillas)
                {
                    resultado.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else sb.Append(c);
            }
            resultado.Add(sb.ToString().Trim());
            return resultado;
        }

        private static string ObtenerRutaArchivo()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 6 && dir != null; i++)
            {
                var carpeta = dir.GetDirectories().FirstOrDefault(d =>
                    d.Name.Equals("Archivos", StringComparison.OrdinalIgnoreCase));

                if (carpeta != null)
                {
                    string p = Path.Combine(carpeta.FullName, "clientes.csv");
                    if (File.Exists(p)) return p;
                }
                dir = dir.Parent;
            }
            return null;
        }

        private static void EsperarEnter()
        {
            Console.WriteLine("\nPresione Enter para volver al menú...");
            Console.ReadLine();
        }
    }
}
