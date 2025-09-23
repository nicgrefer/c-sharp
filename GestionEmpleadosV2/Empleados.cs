// Empleado.cs
public class Empleado
{
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public double Salario { get; set; }
    public bool ContratoFijo { get; set; }

    public Empleado(string nombre, int edad, double salario, bool contratoFijo)
    {
        Nombre = nombre;
        Edad = edad;
        Salario = salario;
        ContratoFijo = contratoFijo;
    }
}