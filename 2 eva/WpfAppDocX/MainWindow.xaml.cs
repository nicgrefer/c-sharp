using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Xceed.Document.NET; // <--- Necesario para Formatos (Colors, Alignment, etc.)
using Xceed.Words.NET;    // <--- Necesario para crear el Doc

namespace WpfAppDocX
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

            // 2. Diálogo para guardar (.docx)
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Documento de Word (*.docx)|*.docx",
                FileName = $"Inventario_{DateTime.Now:yyyyMMdd}.docx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    GenerarWord(datosParaReporte, saveFileDialog.FileName);

                    var result = MessageBox.Show("Documento Word generado. ¿Deseas abrirlo?", "Éxito", MessageBoxButton.YesNo, MessageBoxImage.Information);
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

        private void GenerarWord(List<Producto> datos, string rutaArchivo)
        {
            // Creamos el documento en memoria
            using (var document = DocX.Create(rutaArchivo))
            {
                // ==========================================
                // 1. CABECERA (Usando Tabla Invisible)
                // ==========================================

                // Creamos una tabla de 1 fila y 2 columnas para maquetar
                var headerTable = document.AddTable(1, 2);
                headerTable.Design = TableDesign.None; // Sin bordes visibles
                headerTable.Alignment = Alignment.center;

                // --- COLUMNA 1: LOGO ---
                var celdaLogo = headerTable.Rows[0].Cells[0];
                celdaLogo.Width = 100; // Ancho fijo para el logo

                if (File.Exists("logo.png"))
                {
                    // En DocX, primero cargas la imagen en el documento
                    var imagenObj = document.AddImage("logo.png");
                    // Luego creas un "Picture" para insertarlo
                    var picture = imagenObj.CreatePicture();
                    // Ajustar tamaño (opcional, ancho y alto en pixeles)
                    picture.Width = 60;
                    picture.Height = 60;

                    celdaLogo.Paragraphs[0].AppendPicture(picture);
                }
                else
                {
                    celdaLogo.Paragraphs[0].Append("SIN LOGO").Bold();
                }

                // --- COLUMNA 2: DATOS EMPRESA ---
                var celdaTexto = headerTable.Rows[0].Cells[1];
                celdaTexto.VerticalAlignment = Xceed.Document.NET.VerticalAlignment.Center;

                celdaTexto.Paragraphs[0].Append("TechSolutions Importadora S.A.\n").Bold().FontSize(14).Color(Xceed.Drawing.Color.DarkBlue)
                                        .Append("Av. Siempreviva 742, Ciudad Gótica\n").FontSize(10).Color(Xceed.Drawing.Color.Gray)
                                        .Append("Tel: (555) 123-4567").FontSize(10).Color(Xceed.Drawing.Color.Gray);

                // Insertar la tabla de cabecera al documento
                document.InsertTable(headerTable);

                // Insertar un salto de línea
                document.InsertParagraph("");

                // ==========================================
                // 2. TÍTULO
                // ==========================================
                document.InsertParagraph("Reporte de Stock Disponible")
                        .FontSize(16)
                        .Bold()
                        .Alignment = Alignment.center;

                document.InsertParagraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(10)
                        .Italic()
                        .Alignment = Alignment.center;

                document.InsertParagraph(""); // Espacio

                // ==========================================
                // 3. TABLA DE DATOS (LINQ)
                // ==========================================

                // Creamos la tabla: filas = datos + 1 (encabezado), columnas = 4
                var productTable = document.AddTable(datos.Count + 1, 4);

                // Estilo predefinido de Word (Azul claro, muy limpio)
                productTable.Design = TableDesign.LightListAccent1;
                productTable.Alignment = Alignment.center;

                // --- ENCABEZADOS ---
                productTable.Rows[0].Cells[0].Paragraphs[0].Append("ID").Bold();
                productTable.Rows[0].Cells[1].Paragraphs[0].Append("Producto").Bold();
                productTable.Rows[0].Cells[2].Paragraphs[0].Append("Precio").Bold().Alignment = Alignment.right;
                productTable.Rows[0].Cells[3].Paragraphs[0].Append("Stock").Bold().Alignment = Alignment.center;

                // --- RELLENADO DE DATOS ---
                // Cultura española para el formato de moneda
                var culturaES = System.Globalization.CultureInfo.GetCultureInfo("es-ES");

                for (int i = 0; i < datos.Count; i++)
                {
                    var item = datos[i];
                    // Nota: Las filas empiezan en 1 porque la 0 es el encabezado
                    int rowIndex = i + 1;

                    productTable.Rows[rowIndex].Cells[0].Paragraphs[0].Append(item.Id.ToString());
                    productTable.Rows[rowIndex].Cells[1].Paragraphs[0].Append(item.Nombre);

                    // Precio con formato Euro
                    productTable.Rows[rowIndex].Cells[2].Paragraphs[0].Append(item.Precio.ToString("C", culturaES)).Alignment = Alignment.right;

                    productTable.Rows[rowIndex].Cells[3].Paragraphs[0].Append(item.Stock.ToString()).Alignment = Alignment.center;
                }

                // Insertar tabla en el documento
                document.InsertTable(productTable);

                // ==========================================
                // 4. PIE CON TOTALES
                // ==========================================
                document.InsertParagraph("");
                var totalP = document.InsertParagraph();
                totalP.Alignment = Alignment.right;
                totalP.Append("Total Items en Stock: ").Bold();
                totalP.Append(datos.Sum(x => x.Stock).ToString()).Bold().Color(Xceed.Drawing.Color.DarkBlue);

                // Guardar cambios en disco
                document.Save();
            }
        }

        private void AbrirArchivo(string ruta)
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }
    }
}