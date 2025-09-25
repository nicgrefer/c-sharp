using System.Collections.Generic;

class Carta
{
    public string Palo { get; }
    public string Valor { get; }

    public Carta(string palo, string valor)
    {
        Palo = palo;
        Valor = valor;
    }

    public override string ToString()
    {
        return $"{Valor} de {Palo}";
    }

    public static Carta[] CargarDatos()
    {
        List<Carta> cartas = new List<Carta>();
        foreach (var palo in new string[] { "Copas", "Bastos", "Espadas", "Oros" })
        {
            foreach (var valor in new string[] { "As", "2", "3", "4", "5", "6", "7", "8", "9", "Sota", "Caballo", "Rey" })
            {
                cartas.Add(new Carta(palo, valor));
            }
        }
        return cartas.ToArray();
    }
}