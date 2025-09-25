using System;
using System.Collections.Generic;

class Baraja
{
    public Carta[] Cartas { get; set; }

    public void CargarCartas()
    {
        Cartas = Carta.CargarDatos();
    }
}
