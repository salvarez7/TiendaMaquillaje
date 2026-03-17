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
            // Mostrar mantiene la funcionalidad: crea un usuario solicitando datos por consola
            CrearUsuario();
        }

        // --- CRUD methods ---
        public static string ObtenerRutaClientes()
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

            if (ruta == null)
            {
                ruta = Path.Combine(Environment.CurrentDirectory, "Archivos", "clientes.csv");
            }

            return ruta;
        }

        public static void CrearUsuario()
        {
            var ruta = ObtenerRutaClientes();
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
                    using (var sw = new StreamWriter(ruta, append: false, encoding: Encoding.UTF8))
                    {
                        sw.WriteLine("Cedula,Nombre,Ciudad,Fecha");
                        sw.WriteLine(registro);
                    }
                }
                else
                {
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

        public static List<string> LeerRegistros()
        {
            var ruta = ObtenerRutaClientes();
            if (!File.Exists(ruta)) return new List<string>();
            return File.ReadAllLines(ruta, Encoding.UTF8).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        }

        public static bool ActualizarUsuario(string cedula, string nuevoNombre, string nuevaCiudad)
        {
            var ruta = ObtenerRutaClientes();
            if (!File.Exists(ruta)) return false;

            var lineas = File.ReadAllLines(ruta, Encoding.UTF8).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            if (lineas.Count == 0) return false;

            var header = lineas[0];
            var registros = lineas.Skip(1).ToList();
            bool encontrado = false;

            List<string> nuevos = new List<string>();
            foreach (var linea in registros)
            {
                var primera = ParseFirstField(linea)?.Trim();
                if (primera != null && primera.Equals(cedula, StringComparison.OrdinalIgnoreCase))
                {
                    // Preserve Fecha if present
                    var campos = ParseCSVLine(linea);
                    string fecha = campos.Count > 3 ? campos[3] : DateTime.Now.ToString("yyyy-MM-dd");
                    string Escape(string s)
                    {
                        if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
                        {
                            return '"' + s.Replace("\"", "\"\"") + '"';
                        }
                        return s;
                    }
                    var nuevoRegistro = $"{Escape(cedula)},{Escape(nuevoNombre)},{Escape(nuevaCiudad)},{Escape(fecha)}";
                    nuevos.Add(nuevoRegistro);
                    encontrado = true;
                }
                else
                {
                    nuevos.Add(linea);
                }
            }

            if (!encontrado) return false;

            var salida = new List<string> { header };
            salida.AddRange(nuevos);
            File.WriteAllLines(ruta, salida, Encoding.UTF8);
            return true;
        }

        public static bool EliminarUsuario(string cedula)
        {
            var ruta = ObtenerRutaClientes();
            if (!File.Exists(ruta)) return false;

            var lineas = File.ReadAllLines(ruta, Encoding.UTF8).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            if (lineas.Count == 0) return false;

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
                    continue; // skip
                }
                nuevos.Add(linea);
            }

            if (!encontrado) return false;

            var salida = new List<string> { header };
            salida.AddRange(nuevos);
            File.WriteAllLines(ruta, salida, Encoding.UTF8);
            return true;
        }

        // --- util ---
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
                    else inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    break;
                }
                else sb.Append(c);
            }
            return sb.ToString();
        }

        private static List<string> ParseCSVLine(string linea)
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
                        sb.Append('"');
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
    }
}
