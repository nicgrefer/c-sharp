// MainWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Windows;
using System.Windows.Controls; // Espacio de nombres para los controles (Button, TextBlock, etc.)
using System.Windows.Data;    // Espacio de nombres para el Binding

namespace GestionEmpleadosV2
{
    public partial class MainWindow : Window
    {
        // 1. DECLARAMOS LOS CONTROLES COMO CAMPOS DE LA CLASE
        // Esto permite que el método del botón pueda acceder a ellos más tarde.
        private ListView empleadosListView;
        private TextBlock salarioPromedioText;
        private TextBlock mejorPagadoText;

        public MainWindow()
        {
                // Establece la cultura para que use el formato de España (Euro)
    var cultureInfo = new CultureInfo("es-ES");
    Thread.CurrentThread.CurrentCulture = cultureInfo;
    Thread.CurrentThread.CurrentUICulture = cultureInfo;
            // Este método sigue siendo necesario para inicializar la ventana.
            InitializeComponent(); 
            
            // Llamamos a un método para crear y configurar nuestra interfaz.
            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            // 2. CREAMOS EL PANEL PRINCIPAL (Grid)
            Grid mainGrid = new Grid();
            mainGrid.Margin = new Thickness(15);

            // Definimos las filas del Grid, igual que en XAML
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Fila 0 para el título
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Fila 1 para la lista
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Fila 2 para el botón
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Fila 3 para los resultados

            // 3. CREAMOS CADA CONTROL Y LO CONFIGURAMOS

            // Título
            TextBlock titulo = new TextBlock
            {
                Text = "Lista de Empleados",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };
            Grid.SetRow(titulo, 0); // Asignamos el control a la fila 0 del Grid

            // ListView y GridView (la parte más compleja)
            empleadosListView = new ListView();
            empleadosListView.Margin = new Thickness(0, 0, 0, 10);
            
            GridView gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "Nombre", DisplayMemberBinding = new Binding("Nombre"), Width = 150 });
            gridView.Columns.Add(new GridViewColumn { Header = "Edad", DisplayMemberBinding = new Binding("Edad"), Width = 60 });
            gridView.Columns.Add(new GridViewColumn { Header = "Salario", DisplayMemberBinding = new Binding("Salario") { StringFormat = "C" }, Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Contrato Fijo", DisplayMemberBinding = new Binding("ContratoFijo"), Width = 100 });
            
            empleadosListView.View = gridView;
            Grid.SetRow(empleadosListView, 1); // Asignamos a la fila 1

            // Botón
            Button cargarDatosButton = new Button
            {
                Content = "Cargar Datos",
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(10, 5,10,5)
            };
            cargarDatosButton.Click += CargarDatosButton_Click; // Asociamos el evento al método
            Grid.SetRow(cargarDatosButton, 2); // Asignamos a la fila 2

            // Panel para los resultados
            StackPanel resultadosPanel = new StackPanel 
            {
                Margin = new Thickness(0, 10, 0, 0) 
            };
            
            salarioPromedioText = new TextBlock { FontSize = 14, FontWeight = FontWeights.SemiBold, Text = "Salario Promedio: " };
            mejorPagadoText = new TextBlock { FontSize = 14, FontWeight = FontWeights.SemiBold, Text = "Empleado con mayor salario: " };
            
            resultadosPanel.Children.Add(salarioPromedioText);
            resultadosPanel.Children.Add(mejorPagadoText);
            Grid.SetRow(resultadosPanel, 3); // Asignamos el panel a la fila 3

            // 4. AÑADIMOS TODOS LOS CONTROLES AL GRID
            mainGrid.Children.Add(titulo);
            mainGrid.Children.Add(empleadosListView);
            mainGrid.Children.Add(cargarDatosButton);
            mainGrid.Children.Add(resultadosPanel);

            // 5. FINALMENTE, ESTABLECEMOS EL GRID COMO EL CONTENIDO DE LA VENTANA
            this.Content = mainGrid;
        }

        // El método que se ejecuta al hacer clic en el botón (la lógica no cambia)
        private void CargarDatosButton_Click(object sender, RoutedEventArgs e)
        {
            List<Empleado> listaEmpleados = new List<Empleado>
            {
                new Empleado("Ana García", 28, 32000.50, true),
                new Empleado("Juan Martínez", 35, 45000.00, true),
                new Empleado("Luisa Fernández", 22, 21000.75, false),
                new Empleado("Carlos Rodríguez", 41, 55000.25, true)
            };

            // Usamos los controles que declaramos como campos de la clase
            empleadosListView.ItemsSource = listaEmpleados;
            
            double salarioPromedio = listaEmpleados.Average(emp => emp.Salario);
            salarioPromedioText.Text = $"Salario Promedio: {salarioPromedio:C}";

            Empleado? empleadoMejorPagado = listaEmpleados.OrderByDescending(emp => emp.Salario).FirstOrDefault();
            if (empleadoMejorPagado != null)
            {
                mejorPagadoText.Text = $"Empleado con mayor salario: {empleadoMejorPagado.Nombre}";
            }
        }
    }
}