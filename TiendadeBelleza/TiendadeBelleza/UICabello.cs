using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TiendadeBelleza
{
    internal class UICabello
    {
        public static void Mostrar()
        {
            // Empezamos desde el directorio base de la aplicación y subimos hasta la raíz buscando una carpeta llamada "Archivos" 
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

            // 1) Ruta exacta dentro de una carpeta Archivos encontrada subiendo desde el base directory
            var carpeta = BuscarCarpetaArchivos(AppContext.BaseDirectory);
            if (!string.IsNullOrEmpty(carpeta))
            {
                var p = Path.Combine(carpeta, "productosCabello.csv");
                if (File.Exists(p)) ruta = p;
            }


            if (ruta == null)
            {
                Console.WriteLine("No se encontró el archivo 'Archivos/productosCabello.csv'. Asegúrate de que la carpeta 'Archivos' esté en la estructura del proyecto o en la carpeta padre del ejecutable.");
                return;
            }

            string[] lineas;
            try
            {
                lineas = File.ReadAllLines(ruta).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
                return;
            }

            var productos = new List<(string Nombre, string Precio)>();

            foreach (var linea in lineas)
            {
                // Intentar con separador por comas, si no, por punto y coma
                var columnas = linea.Split(',');
                if (columnas.Length < 2)
                {
                    columnas = linea.Split(';');
                }

                if (columnas.Length == 0) continue;

                string nombre = columnas.Length > 1 ? columnas[1].Trim() : columnas[0].Trim();
                string precio = columnas[columnas.Length - 1].Trim();

                productos.Add((nombre, precio));
            }

            if (productos.Count == 0)
            {
                Console.WriteLine("No hay productos en el archivo.");
                return;
            }

            Console.WriteLine("Productos de cabello disponibles:");
            for (int i = 0; i < productos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {productos[i].Nombre} - {productos[i].Precio}");
            }

            Console.Write("¿Qué producto quiere? ");
            var respuesta = Console.ReadLine();

            if (int.TryParse(respuesta, out int seleccionIndex))
            {
                if (seleccionIndex >= 1 && seleccionIndex <= productos.Count)
                {
                    var elegido = productos[seleccionIndex - 1];
                    Console.WriteLine($"Has seleccionado: {elegido.Nombre} - {elegido.Precio}");
                    return;
                }
            }

            // Si no es número, buscar por nombre
            var encontrado = productos.FirstOrDefault(p => string.Equals(p.Nombre, respuesta?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(encontrado.Nombre))
            {
                Console.WriteLine($"Has seleccionado: {encontrado.Nombre} - {encontrado.Precio}");
            }
            else
            {
                Console.WriteLine("Producto no encontrado.");
            }
        }
    }
}
