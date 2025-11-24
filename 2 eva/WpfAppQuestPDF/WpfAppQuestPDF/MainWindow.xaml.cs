using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace WpfAppQuestPDF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Producto> _baseDeDatos;


        public MainWindow()
        {
            InitializeComponent();

            // IMPORTANTE: Configurar licencia para QuestPDF 2023+
            QuestPDF.Settings.License = LicenseType.Community;

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
                new Producto { Id = 6, Nombre = "Silla de Oficina Pro", Precio = 350.00m, Stock = 2 },
            };

            dgProductos.ItemsSource = _baseDeDatos;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1. Consulta LINQ (Filtrar y ordenar)
            var datosParaReporte = _baseDeDatos
                                    .Where(p => p.Stock > 0) // Solo productos con stock
                                    .OrderBy(p => p.Nombre)  // Ordenar alfabéticamente
                                    .ToList();

            if (!datosParaReporte.Any())
            {
                MessageBox.Show("No hay datos para mostrar en el informe.");
                return;
            }

            // 2. Diálogo para guardar
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf",
                FileName = $"Inventario_{DateTime.Now:yyyy-MM-dd_HHmm}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // 3. Generar PDF
                    GenerarDocumentoPdf(datosParaReporte, saveFileDialog.FileName);

                    // 4. Abrir PDF
                    var result = MessageBox.Show("PDF generado exitosamente. ¿Abrir archivo?", "Reporte Listo", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        AbrirPdf(saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error al generar PDF", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // --- MÉTODO PRINCIPAL DE GENERACIÓN PDF ---
        private void GenerarDocumentoPdf(List<Producto> datos, string rutaArchivo)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(2, Unit.Centimetre);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                    // ==================================================
                    // NUEVA CABECERA CON LOGO Y DATOS
                    // ==================================================
                    page.Header().PaddingBottom(1, Unit.Centimetre).Column(headerCol =>
                    {
                        // Fila superior: Logo a la izquierda, Datos a la derecha
                        headerCol.Item().Row(row =>
                        {
                            // --- COLUMNA 1: EL LOGO ---
                            row.ConstantItem(60)
                               .Height(60)
                               .AlignLeft()
                               // 1. Pasas primero la ruta
                               .Image("logo.png")
                               // 2. Encadenas el ajuste después
                               .FitArea();

                            // --- COLUMNA 2: DATOS DE LA EMPRESA ---
                            // 'RelativeItem' toma el resto del espacio disponible.
                            row.RelativeItem().PaddingLeft(15).Column(companyCol =>
                            {
                                companyCol.Item().Text("TechSolutions Importadora S.A.").FontSize(14).Bold().FontColor(Colors.Blue.Darken3);
                                companyCol.Item().Text("Av. Siempreviva 742, Parque Industrial");
                                companyCol.Item().Text("Ciudad Gótica, CP 54321");
                                companyCol.Item().Text("Tel: (983) 987654 | Email: contacto@techsolutions.fake");
                            });
                        });

                        // Línea separadora y título del reporte
                        headerCol.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Height(5);

                        headerCol.Item().PaddingTop(10).AlignCenter().Text("Informe de Stock Disponible")
                                 .FontSize(18).SemiBold().FontColor(Colors.Black);
                    });
                    // ==================================================


                    // --- CONTENIDO (LA TABLA) ---
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        // Definir ancho de columnas
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);  // Id
                            columns.RelativeColumn(3);   // Nombre (más ancho)
                            columns.RelativeColumn(1);   // Precio
                            columns.RelativeColumn(1);   // Stock
                        });

                        // Encabezados de tabla
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyleHeader).Text("ID");
                            header.Cell().Element(CellStyleHeader).Text("Producto");
                            header.Cell().Element(CellStyleHeader).AlignRight().Text("Precio");
                            header.Cell().Element(CellStyleHeader).AlignCenter().Text("Stock");
                        });

                        // Filas de datos (Iteración LINQ)
                        foreach (var item in datos)
                        {
                            // Alternar color de fondo para filas pares/impares (opcional)
                            var bgColor = datos.IndexOf(item) % 2 == 0 ? Colors.White : Colors.Grey.Lighten5;

                            table.Cell().Element(c => CellStyleBody(c, bgColor)).Text(item.Id.ToString());
                            table.Cell().Element(c => CellStyleBody(c, bgColor)).Text(item.Nombre);
                            table.Cell().Element(c => CellStyleBody(c, bgColor)).AlignRight().Text($"{item.Precio:C2}");
                            table.Cell().Element(c => CellStyleBody(c, bgColor)).AlignCenter().Text(item.Stock.ToString());
                        }

                        // Pie de tabla (Totales)
                        table.Footer(footer =>
                        {
                            footer.Cell().ColumnSpan(4).PaddingTop(10).AlignRight().Text(txt =>
                            {
                                txt.Span("Total Productos en Stock: ").SemiBold();
                                // Ejemplo de LINQ extra para el pie de página
                                txt.Span(datos.Sum(x => x.Stock).ToString()).Bold().FontSize(12);
                            });
                        });
                    });


                    // --- PIE DE PÁGINA ---
                    page.Footer().PaddingTop(5).Row(row =>
                    {
                        row.RelativeItem().Text(x =>
                        {
                            x.Span("Generado el: ");
                            x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });

                        row.RelativeItem().AlignRight().Text(x =>
                        {
                            x.Span("Pág. ");
                            x.CurrentPageNumber();
                            x.Span(" de ");
                            x.TotalPages();
                        });
                    });
                });
            })
            .GeneratePdf(rutaArchivo);
        }

        // Helpers de estilo para limpiar el código principal
        static IContainer CellStyleHeader(IContainer container)
        {
            return container.Background(Colors.Blue.Lighten4)
                            .BorderBottom(1).BorderColor(Colors.Blue.Medium)
                            .PaddingVertical(5).PaddingHorizontal(5)
                            .DefaultTextStyle(x => x.SemiBold().FontColor(Colors.Blue.Darken3));
        }

        static IContainer CellStyleBody(IContainer container, string backgroundColor)
        {
            return container.Background(backgroundColor)
                            .BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                            .PaddingVertical(8).PaddingHorizontal(5);
        }

        private void AbrirPdf(string ruta)
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }
    }
}

/* NOTA SOBRE EL LOGO EN LA CABECERA:
 
Si tienes un archivo llamado logo.png en la carpeta de tu proyecto 
(asegúrate de que en sus propiedades esté marcado como "Copiar al directorio de salida: Copiar si es posterior"), 

// CAMBIO PARA USAR IMAGEN REAL
byte[] imageData = File.ReadAllBytes("logo.png");

row.ConstantItem(60) // Definimos el ancho máximo del contenedor del logo
   .Height(60)       // Definimos el alto máximo
   .AlignLeft()
   .Image(imageData, ImageScaling.FitArea); // FitArea mantiene la relación de aspecto

*/// En el ejemplo se usa un recuadro azul con la palabra "LOGO" para simular un logo.