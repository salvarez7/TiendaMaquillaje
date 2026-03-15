using TiendadeBelleza;

string menu =
"""
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
     __  __       _   _                
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
                                                                                             
1. Comprar productos de maquillaje
2. Comprar productos del cabello
3. Comprar productos para hombre
4. Crear un usuario
5. Ingresar usuario
6.Salir 
-------------------------
Ingrese una opción: 
""";

do
{
    Console.Write(menu);
    string entrada = Console.ReadLine();

    switch (entrada)
    {
        case "1":
            
            break;

        case "2":
            UICabello.Mostrar();
            break;

        case "3":
            
            break;

        case "4":
            
            break;

        case "5":
            
            break;

        case "6":
            return;

        default:
            Console.WriteLine("Opción invalida");
            break;
    }
}
while (true);
