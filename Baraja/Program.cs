using System;

namespace baraja;

class Program
{
    static void Main(string[] args)
    {
        Baraja baraja = new Baraja();

        baraja.CargarCartas();

        Console.WriteLine("--- Baraja Española Ordenada ---");
        foreach (var carta in baraja.Cartas)
        {
            Console.WriteLine(carta.ToString());
        }

        Console.WriteLine("==============================================");

        Console.WriteLine("--- Baraja Española Desordenada ---");

        Random random = new Random();
        for (int i = 0; i < baraja.Cartas.Length; i++)
        {
            int j = random.Next(i, baraja.Cartas.Length);
            (baraja.Cartas[i], baraja.Cartas[j]) = (baraja.Cartas[j], baraja.Cartas[i]);
        }

        foreach (var carta in baraja.Cartas)
        {
            Console.WriteLine(carta.ToString());
        }

        Console.WriteLine("==============================================");

        Console.WriteLine("--- Repartiendo Cartas a los Jugadores ---");

        int numJugadores = 4;
        int cartasPorJugador = 4;

        for (int jugador = 1; jugador <= numJugadores; jugador++)
        {
            Console.WriteLine($"Jugador {jugador}:");
            for (int carta = 0; carta < cartasPorJugador; carta++)
            {
                int indiceCarta = (jugador - 1) * cartasPorJugador + carta;
                Console.WriteLine($"  {baraja.Cartas[indiceCarta]}");
            }
        }


    }
}
