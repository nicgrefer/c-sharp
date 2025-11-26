using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using ClosedXML.Excel; // <--- Espacio de nombres de la librería

namespace WpfAppExcelXML
{
    public partial class MainWindow : Window
    {
        private List<Producto> _baseDeDatos;

        public class Producto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            _baseDeDatos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Laptop Gamer RTX", Precio = 1200.50m, Stock = 5 },
                new Producto { Id = 2, Nombre = "Mouse Ergonómico", Precio = 25.00m, Stock = 50 },
                new Producto { Id = 3, Nombre = "Teclado Mecánico RGB", Precio = 85.99m, Stock = 15 },
                new Producto { Id = 4, Nombre = "Monitor Curvo 4K", Precio = 450.00m, Stock = 0 },
                new Producto { Id = 5, Nombre = "Docking Station USB-C", Precio = 95.50m, Stock = 10 },
            };
            dgProductos.ItemsSource = _baseDeDatos;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1. Consulta LINQ
            var datosParaReporte = _baseDeDatos
                                    .Where(p => p.Stock > 0)
                                    .OrderBy(p => p.Nombre)
                                    .ToList();

            if (!datosParaReporte.Any()) return;

            // 2. Diálogo para guardar (Cambiamos filtro a .xlsx)
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Libro de Excel (*.xlsx)|*.xlsx",
                FileName = $"Inventario_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    GenerarExcel(datosParaReporte, saveFileDialog.FileName);

                    var result = MessageBox.Show("Excel generado. ¿Deseas abrirlo?", "Éxito", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        AbrirArchivo(saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void GenerarExcel(List<Producto> datos, string rutaArchivo)
        {
            // Creamos el libro de trabajo
            using (var workbook = new XLWorkbook())
            {
                // Creamos la hoja
                var worksheet = workbook.Worksheets.Add("Inventario");

                // ==========================================
                // 1. CABECERA Y LOGO
                // ==========================================

                // Agregar Logo (Si existe el archivo)
                if (File.Exists("logo.png"))
                {
                    var image = worksheet.AddPicture("logo.png")
                                         .MoveTo(worksheet.Cell("A1"))
                                         .Scale(0.5); // Ajustar tamaño si es necesario
                }

                // Datos de la empresa (Celdas B1 a B4)
                worksheet.Cell("B1").Value = "TechSolutions Importadora S.A.";
                worksheet.Cell("B1").Style.Font.Bold = true;
                worksheet.Cell("B1").Style.Font.FontSize = 14;

                worksheet.Cell("B2").Value = "Av. Siempreviva 742";
                worksheet.Cell("B3").Value = "Tel: (555) 123-4567";

                // Título del reporte
                worksheet.Cell("A5").Value = "Reporte de Stock Disponible";
                worksheet.Cell("A5").Style.Font.Bold = true;
                worksheet.Cell("A5").Style.Font.FontSize = 12;
                worksheet.Range("A5:D5").Merge(); // Unir celdas para el título

                // ==========================================
                // 2. ENCABEZADOS DE TABLA (Fila 7)
                // ==========================================
                int filaInicio = 7;

                worksheet.Cell(filaInicio, 1).Value = "ID";
                worksheet.Cell(filaInicio, 2).Value = "Producto";
                worksheet.Cell(filaInicio, 3).Value = "Precio";
                worksheet.Cell(filaInicio, 4).Value = "Stock";

                // Estilo para la cabecera
                var rangoCabecera = worksheet.Range(filaInicio, 1, filaInicio, 4);
                rangoCabecera.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD"); // Azul
                rangoCabecera.Style.Font.FontColor = XLColor.White;
                rangoCabecera.Style.Font.Bold = true;
                rangoCabecera.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // ==========================================
                // 3. DATOS (LINQ)
                // ==========================================
                int filaActual = filaInicio + 1;

                foreach (var item in datos)
                {
                    worksheet.Cell(filaActual, 1).Value = item.Id;
                    worksheet.Cell(filaActual, 2).Value = item.Nombre;

                    // IMPORTANTE: Insertamos el número puro, el formato lo damos después.
                    // Esto permite hacer sumas en Excel correctamente.
                    worksheet.Cell(filaActual, 3).Value = item.Precio;

                    worksheet.Cell(filaActual, 4).Value = item.Stock;

                    // Aplicar formato de moneda (€) a la celda de precio
                    // El formato personalizado para Euro en Excel suele ser este:
                    worksheet.Cell(filaActual, 3).Style.NumberFormat.Format = "#,##0.00 €";

                    filaActual++;
                }

                // ==========================================
                // 4. FORMATO FINAL
                // ==========================================

                // Crear una tabla "oficial" de Excel para permitir filtros automáticos (Opcional)
                // var rangoTabla = worksheet.Range(filaInicio, 1, filaActual - 1, 4);
                // rangoTabla.CreateTable(); 

                // Autoajustar ancho de columnas al contenido
                worksheet.Columns().AdjustToContents();

                // Guardar
                workbook.SaveAs(rutaArchivo);
            }
        }

        private void AbrirArchivo(string ruta)
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }
    }
}