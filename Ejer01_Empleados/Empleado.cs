public class Empleado
{

    // Propiedades de la clase Empleado
    public string nombre { get; set; }
    public int edad { get; set; }
    public double salario { get; set; }

    public bool contratoFijo { get; set; }


    // Constructor de la clase Empleado
    public Empleado(string nombre, int edad, double salario, bool contratoFijo)
    {
        this.nombre = nombre;
        this.edad = edad;
        this.salario = salario;
        this.contratoFijo = contratoFijo;
    }

}