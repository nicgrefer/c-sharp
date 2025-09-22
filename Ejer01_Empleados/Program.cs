using System;
namespace Ejer01_Empleados
{
    class Program
    {
        static void Main(string[] args)
        {
            //Cramos lista de empleados
            List<Empleado> listaEmpleados = new List<Empleado>();
            // Añadimos empleados a la lista
            listaEmpleados.Add(new Empleado("Ana García", 28, 32000.50, true));
            listaEmpleados.Add(new Empleado("Juan Martínez", 35, 45000.00, true));
            listaEmpleados.Add(new Empleado("Luisa Fernández", 22, 21000.75, false));
            listaEmpleados.Add(new Empleado("Carlos Rodríguez", 41, 55000.25, true));

            // Recorremos la lista e imprimimos los detalles de cada empleado
            Console.WriteLine("Lista de Empleados:");
            foreach (Empleado emp in listaEmpleados)
            {

                string contratoFijoTexto = emp.contratoFijo ? "Sí" : "No";
                Console.WriteLine($"Nombre: {emp.nombre}, Edad: {emp.edad}, Salario: {emp.salario:C}, Tipo de Contrato fijo: {contratoFijoTexto}");
            }

            Console.WriteLine("--------------------------------");

            // Funcion calcular el preciopromedio de los salarios
            double salarioTotal = 0;
            foreach (Empleado emp in listaEmpleados)
            {
                salarioTotal += emp.salario;
            }

            double salarioPromedio = salarioTotal / listaEmpleados.Count;
            Console.WriteLine($"Salario promedio: {salarioPromedio:C}");

            Console.WriteLine("--------------------------------");

            // Funcion para encontrar el empleado con el salario más alto
            Empleado empleadoSalarioAlto = listaEmpleados[0];
            foreach (Empleado emp in listaEmpleados)
            {
                if (emp.salario > empleadoSalarioAlto.salario)
                {
                    empleadoSalarioAlto = emp;
                }
            }   

            Console.WriteLine($"Empleado con el salario más alto: {empleadoSalarioAlto.nombre}, Salario: {empleadoSalarioAlto.salario:C}");
            Console.WriteLine("--------------------------------");
            Console.ReadKey();
        }
    }


}

