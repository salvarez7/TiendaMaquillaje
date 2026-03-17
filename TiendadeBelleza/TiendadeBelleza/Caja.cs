using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TiendadeBelleza
{
    internal static class Caja
    {
        private static readonly List<(string Nombre, decimal Precio)> items = new();

        public static void AddItem(string nombre, string precioStr)
        {
            var precio = ParsePrecio(precioStr);
            items.Add((nombre, precio));
            Console.WriteLine($"Producto agregado a la caja: {nombre} - {precio:C2}");
            Console.WriteLine($"Total actual: {Total():C2}");
            Console.WriteLine("Presione Enter para continuar...");
            Console.ReadLine();
        }

        public static decimal Total()
        {
            return items.Sum(i => i.Precio);
        }

        public static void Mostrar()
        {
            Console.Clear();
            Console.WriteLine("=== Caja - Resumen de compra ===\n");
            if (items.Count == 0)
            {
                Console.WriteLine("No hay productos en la caja.");
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {items[i].Nombre} - {items[i].Precio:C2}");
                }
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Total a pagar: {Total():C2}");
            }

            Console.WriteLine("\nPresione Enter para volver al menú...");
            Console.ReadLine();
        }

        public static void Limpiar()
        {
            items.Clear();
        }

        private static decimal ParsePrecio(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            s = s.Trim();

            // Keep digits and separators
            var cleaned = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
            if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
                return d;
            if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                return d;

            // fallback
            cleaned = cleaned.Replace(',', '.');
            if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out d))
                return d;

            return 0m;
        }
    }
}
