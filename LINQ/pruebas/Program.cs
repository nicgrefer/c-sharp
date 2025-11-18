using System.Linq;

namespace pruebas;

class Program
{
    static int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    static void Main(string[] args)
    {
        //ejer1();   
        //ejer2();  
        //ejer3();
        //ejer4();
        ejer1_1();  
        ejer1_2();  
        ejer1_3();
        Console.ReadLine();
    }

    // --- Ejercicio 1  ---
    public static void ejer1()
    {
        // --- Consulta LINQ para filtrar números pares ---
        var result = numbers.Where(n => n % 2 == 0);
        //  --- Mostrar resultados ---
        Console.WriteLine(result);
        // --- Forma alternativa de mostrar resultados ---
        foreach (var n in result)
        {
            Console.WriteLine(n);
        }
    }

    // --- Lista de personas ---
    // --- Simula Datos recibidos de una base de datos ---
    static List<Persona> listaPersona()
    {
        return new List<Persona>
        {
            new Persona { Nombre = "Carlos López", Edad = 28, Ciudad = "Madrid" },
            new Persona { Nombre = "María Fernández", Edad = 34, Ciudad = "Barcelona" },
            new Persona { Nombre = "Ana Gómez", Edad = 22, Ciudad = "Valencia" },
            new Persona { Nombre = "Jorge Ramírez", Edad = 45, Ciudad = "Sevilla" },
            new Persona { Nombre = "Lucía Morales", Edad = 31, Ciudad = "Bilbao" },
            new Persona { Nombre = "Pedro Sánchez", Edad = 27, Ciudad = "Zaragoza" },
            new Persona { Nombre = "Marta Díaz", Edad = 36, Ciudad = "Zaragoza" },
            new Persona { Nombre = "Sofía Navarro", Edad = 29, Ciudad = "Alicante" },
            new Persona { Nombre = "Diego Torres", Edad = 38, Ciudad = "Málaga" },
            new Persona { Nombre = "Elena Ruiz", Edad = 24, Ciudad = "Córdoba" },
            new Persona { Nombre = "Raúl Martín", Edad = 50, Ciudad = "Valladolid" }
        };
    }

    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
    }

    public static List<Producto> productos = new List<Producto>
    {
        new Producto { Id = 1, Nombre = "Teclado", Precio = 45.99m, Categoria = "Periféricos", Stock = 10 },
        new Producto { Id = 2, Nombre = "Ratón", Precio = 25.50m, Categoria = "Periféricos", Stock = 15 },
        new Producto { Id = 3, Nombre = "Monitor", Precio = 199.99m, Categoria = "Monitores", Stock = 5 },
        new Producto { Id = 4, Nombre = "Auriculares", Precio = 79.90m, Categoria = "Audio", Stock = 8 },
        new Producto { Id = 5, Nombre = "Webcam", Precio = 55.00m, Categoria = "Monitores", Stock = 0 }
    };


    // --- Ejercicio 2 funcionando ---
    public static void ejer2()
    {
        // --- Consulta LINQ para filtrar personas mayores de 30 años ---
        var personasMayor30 = from persona in listaPersona()
                              where persona.Edad > 30
                              select $"{persona.Nombre}, edad : {persona.Edad}";
                              //select persona;
                        
        var result = from p in listaPersona()
                     where p.Edad > 30
                     select new
                     {
                         // Proyección a un objeto anónimo para mostrar solo ciertos campos
                        Nombrecito = p.Nombre,
                        Anitos = p.Edad,
                        Localidad = p.Ciudad
                     };

        // --- Mostrar resultados ---
        // formato 1, proyección a cadena
        /*
        foreach (var item in personasMayor30)
        {
            //Console.WriteLine($"{item.Nombre}, edad: {item.Edad}");
            Console.WriteLine(item);
        }*/
        // formato 2, proyección a objeto anónimo
        foreach (var item in result)
        {
            //Console.WriteLine($"{item.Nombre}, edad: {item.Edad}");
            Console.WriteLine(item.Localidad);
        }
    }

    // --- Clase Persona ---
    public class Persona
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Ciudad { get; set; }

        
    }

    public static void ejer3()
    {
        var personasOrdenadasPorEdad = from persona in listaPersona()
                                       orderby persona.Edad descending
                                       select persona;
        foreach (var persona in personasOrdenadasPorEdad)
        {
            Console.WriteLine($"{persona.Nombre}, Edad: {persona.Edad}, Ciudad: {persona.Ciudad}");
        }

    }

    public static void ejer4()
    {
        // Agrupar personas por ciudad
        var personasAgrupadasPorCiudad = 
            from persona in listaPersona()
            group persona by persona.Ciudad into grupoCiudad // Crea un "mapa" con clave Ciudad y valor la lista de personas
            select new
                {
                 Ciudad = grupoCiudad.Key, // La clave del grupo (Ciudad) 
                 //Personas = grupoCiudad.ToList()
                 Personas = grupoCiudad.ToList<Persona>() // La lista de personas en esa ciudad
            };

        // Mostrar resultados
        foreach (var grupo in personasAgrupadasPorCiudad)
        {
            Console.WriteLine($"Ciudad: {grupo.Ciudad}");
            foreach (var persona in grupo.Personas)
            {
                Console.WriteLine($"\t{persona.Nombre}, Edad: {persona.Edad}");
            }
        }

    }


    public static void ejer1_1()
    {
        //Sintaxis de Consulta (Query Syntax): Obtener los nombres de los productos que cuestan más de 50.
        var productosCaros = from p in productos
                            where p.Precio > 50
                            select p.Nombre;
        // Resultado: ["Monitor", "Auriculares", "Webcam"]
        foreach (var nombre in productosCaros)
        {
            Console.WriteLine(nombre);
        }
        
    }

    public static void ejer1_2()
    {
       //Sintaxis de Método (Method Syntax): El mismo ejemplo, pero con sintaxis fluida y expresiones lambda.
        var productosCarosMetodo = productos.Where(p => p.Precio > 50).Select(p => p.Nombre);
        // Resultado: ["Monitor", "Auriculares", "Webcam"]
        foreach (var nombre in productosCarosMetodo)
        {
            Console.WriteLine(nombre);
        }
        
    }

    public static void ejer1_3()
    {
       //Ejecución Diferida (Deferred Execution): La consulta no se ejecuta hasta que se itera.
        var productosStockBajo = productos.Where(p => p.Stock < 10);

        // Añadimos un nuevo producto a la lista ORIGINAL después de definir la consulta
        productos.Add(new Producto { Id = 6, Nombre = "Micrófono", Precio = 60.00m, Categoria = "Audio", Stock = 3 });

        // La consulta se ejecuta AQUÍ, e incluye el nuevo producto
        foreach (var p in productosStockBajo)
        {
            Console.WriteLine(p.Nombre); // Imprimirá "Monitor", "Auriculares", "Webcam" y "Micrófono"
        }

    }

}
