
using TiendadeBelleza;
string menu1 = """
                /\/\
               /  \ \
              / /\ \ \
              \/ /\/ /
             / /\/ /\
            / /\ \/\ \
           / / /\ \ \ \
          /\/ / / /\ \ \ \/\
         /  \/ / /  \ \ \ \ \
        / /\ \/ /    \ \/\ \ \       
       |  \/  |     | | (_)               
       | \  / | ___ | |_ ___   _____  ___ 
       | |\/| |/ _ \| __| \ \ / / _ \/ __|
       | |  | | (_) | |_| |\ V / (_) \__ \
       |_|  |_|\___/ \__|_| \_/ \___/|___/
        \/ /\/ /      \/ /\/ /
        / /\/ /\      / /\/ /\
        \ \ \/\ \    / /\ \/ /
        \ \ \ \ \  / / /\  /
         \/\ \ \ \/ / / /\/
            \ \ \ \/ / /
             \ \/\ \/ /
               \/ /\/ /
               / /\/ /\
               \ \ \/ /
                \ \  /
                 \/\/
1. Crear un usuario
2. Ingresar usuario
3. Eliminar usuario                                                                                           
-------------------------
-------------------------
Ingrese una opción: 
""";
string menu2 = """

1. Comprar productos de maquillaje
2. Comprar productos del cabello
3. Comprar productos para hombre
4. Finalizar compra
5.Salir 
-------------------------
Ingrese una opción: 
""";

do
{
    Console.Write(menu1);
    string entrada = Console.ReadLine();

    switch (entrada)
    {
        case "1":
            UICrearUsuario.Mostrar();
            Console.Write(menu2);
            var entrada2 = Console.ReadLine();

            switch (entrada2)
            {
                case "1":
                    UIMaquillaje.Mostrar();
                    break;
                case "2":
                    UICabello.Mostrar();
                    break;
                case "3":
                    UIProductosHombre.Mostrar();
                    break;
                case "4":
                    Caja.Mostrar();
                    Caja.Limpiar();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opción invalida");
                    break;
            }
            break; 



        case "2":
            UIIngresarUsuario.Mostrar();
            while (true)
            {
                Console.Write(menu2);
                var entrada3 = Console.ReadLine();
                switch (entrada3)
                {
                    case "1":
                        UIMaquillaje.Mostrar();
                        break;
                    case "2":
                        UICabello.Mostrar();
                        break;
                    case "3":
                        UIProductosHombre.Mostrar();
                        break;
                    case "4":
                        Caja.Mostrar();
                        Caja.Limpiar();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opción invalida");
                        break;
                }
            }
            break;

        case "3":
            UIEliminarUsuario.Mostrar();
            break;

        default:
            Console.WriteLine("Opción invalida");
            break;
    }
}
while (true);


