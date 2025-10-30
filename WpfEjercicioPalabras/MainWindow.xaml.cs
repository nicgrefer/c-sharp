using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfEjercicioPalabras
{
    public partial class MainWindow : Window
    {
        public static string[] abecedario = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static string[] palabras = { "PERRO", "GATO", "ELEFANTE", "CONEJO", "TIGRE", "LEON", "JIRAFA", "ZORRO", "OSO", "LIEBRE" };

        private readonly List<TextBlock> palabra = new();
        private readonly Random rnd = new();
        private string palabraSeleccionada = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            CargarBotones();
            
        }

        public void CargarBotones()
        {
            lbLEtras.Children.Clear();
            foreach (string letra in abecedario)
            {
                var btn = new Button
                {
                    Content = letra,
                    Margin = new Thickness(2),
                    Width = 30,
                    Height = 30
                };
                btn.Click += Btn_Click; //MUY IMPORTANTE ASOCIAR EL EVENTO CLICK
                lbLEtras.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            btn.IsEnabled = false; // no se puede volver a pulsar
            string letra = btn.Content?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(letra) || string.IsNullOrEmpty(palabraSeleccionada)) return;

            // Revela la letra en todas las posiciones donde coincida
            for (int i = 0; i < palabraSeleccionada.Length; i++)
            {
                if (palabraSeleccionada[i].ToString().Equals(letra, StringComparison.OrdinalIgnoreCase))
                {
                    palabra[i].Text = letra + " ";
                }
            }
        }

        // Botón "Siguiente"
        private void CargarPalabra(object sender, RoutedEventArgs e)
        {
            // Habilita todas las letras
            foreach (var child in lbLEtras.Children)
                if (child is Button b) b.IsEnabled = true;

            CrearPalabra();
        }

        private void CrearPalabra()
        {
            // Limpia visual y estado
            panelPalabra.Children.Clear();
            palabra.Clear();

            // Selecciona palabra aleatoria y la almacena en el campo
            int indicePalabra = rnd.Next(palabras.Length);
            palabraSeleccionada = palabras[indicePalabra];

            // Crea los guiones bajos
            for (int i = 0; i < palabraSeleccionada.Length; i++)
            {
                var tb = new TextBlock
                {
                    Text = "_ ",
                    FontSize = 24,
                    Margin = new Thickness(2)
                };
                palabra.Add(tb);
                panelPalabra.Children.Add(tb);
            }
        }
    }
}