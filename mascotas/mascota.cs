public class Mascota

{

    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Tipo { get; set; }

    public Mascota(string nombre, int edad, string tipo)
    {
        Nombre = nombre;
        Edad = edad;
        Tipo = tipo;
    }

    public void EdadHumana()
    {
        switch (Tipo)
        {
            case "perro":
                Console.WriteLine($"Mascota {Nombre}, Edad humana {Edad * 7} años.");
                break;
            case "gato":
                Console.WriteLine($"Mascota {Nombre}, Edad humana {Edad * 6} años.");
                break;
            default:
                Console.WriteLine($"Mascota {Nombre}, Edad humana {Edad} años.");
                break;
        }
    }

}