using System;

namespace TiendadeBelleza
{
    // Hereda de UIIngresarUsuario para reutilizar CalcularDiasDesdeRegistro
    internal class Descuento : UIIngresarUsuario
    {
        // Devuelve el precio aplicado con descuento del 5% si el usuario tiene más de 100 días desde el registro
        public decimal AplicarDescuentoSiCorresponde(decimal precio, DateTime fechaRegistro)
        {
            int dias = CalcularDiasDesdeRegistro(fechaRegistro);
            if (dias > 100)
            {
                return precio * 0.95m; // 5% de descuento
            }
            return precio;
        }

        // Devuelve true si aplica descuento (más de 100 días)
        public bool AplicaDescuento(DateTime fechaRegistro)
        {
            return CalcularDiasDesdeRegistro(fechaRegistro) > 100;
        }
    }
}
