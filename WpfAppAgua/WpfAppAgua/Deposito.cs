using System;

namespace WpfAppAgua
{
    internal class Deposito
    {
        // declaro el evento Cantidad_Changed de tipo Eventhandler (básico)
        // en algunas publicaciones encontrareis que antes declara un delegate (delegado)
        // en las nuevas versiones no es necesario 
        public event EventHandler Cantidad_Changed; //cada vez que cambie la cantidad de agua
        public event EventHandler DepositoVacio; //cuando la cantidad sea 0

        public Deposito() //constructor
        {
            // tengo la idea de asignar un valor 10 a Cantidad para que sea su valor inicial
            Cantidad = 10;
            // pero observad que los eventos no están suscritos en este momento
            // es lógico porque estamos en el proceso de creación de la instancia
            // sí que asignará el valor pero el evento no será detectado
        }

        // en Java:
        // para acceder a propiedades encapsuladas
        // deposito.SetCantidad(valor)
        // tipo dato=deposito.GetCantidad()


        /*
         Deposito deposito = new()
         ...
         deposito.Cantidad = valor;
         ...
         valor = deposito.Cantidad;
        */

        /*
        public int CantidadEjemplo1 { get; set; } //propiedad autoimplementada

        private int cantidadEjemplo2;
        public int CantidadEjemplo2
        {
            get //getCantidad
            {
                return cantidadEjemplo2;
            }
            set //setCantidad
            {
                //if (value > 20) throw new ArgumentException();
                cantidadEjemplo2 = value; //value contiene el valor asignado a la propiedad
            }
        }

        */

        private int cantidad; //miembro privado (encapsula la propiedad Cantidad)

        // la propiedad se llama Cantidad y es pública
        public int Cantidad //tengo acceso a esta propiedad pública desde MainWindow
        {
            get
            {
                return cantidad; // valor que devuelve cuando valor=deposito.Cantidad
                // deposito es una instancia de tipo Deposito
            }
            set // entra por esta rama cuando deposito.Cantidad=value
            {
                //incluir condiciones de encapsulación (validación,etc.)
                // si son necesarias
                // p.e. if (value <0) { value = 0; }//nunca menor que 0
                // p.e. si el valor no es válido lanzar una excepción
                // recordad que esta clase se "consume desde otro código/clase"

                // Abstracción de la Clase:
                // ERROR (no cumple POO) MessageBox.Show()
                // ERROR (no cumple POO) labelItos.Content= ...

                // en value tenemos el valor asignado
                // cuando deposito.Cantidad=7; //value contiene 7

                cantidad = value; // en este caso se asigna el valor incondicionalmente
                //invocamos el evento Cantidad_Changed de forma incondicional cada vez que 
                // cambie el valor de Cantidad
                Cantidad_Changed?.Invoke(this, EventArgs.Empty);
                if (cantidad == 0) // si se queda vacío (condicional) lanzo el evento DepositoVacio
                    DepositoVacio.Invoke(this, new EventArgs());
                //el interrogante evita un error (NULL)
                //cuando no hay "nadie" suscrito al evento este será NULL
                // ... y provocará una excepción 
            }
        }
        //... int valor=o.Cantidad; // ejecuta el bloque get
        //... o.Cantidad=valor; // ejecuta el bloque set

        public void Llenar(int valor)
        {
            Cantidad = valor; //en mayúsculas a pesar de estar dentro de la clase
            // porque es la forma de que pase por el bloque set con las condiciones de encapsulado
        }
    }
}
