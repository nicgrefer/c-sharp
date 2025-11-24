using System.Windows;
using Microsoft.Win32;
using WpfAppFactura.Clases;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WpfAppFactura
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Clase auxiliar para las líneas de la factura
        private class LineaFactura
        {
            public string Descripcion { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public decimal Importe => Cantidad * Precio;
        }

        // Datos de ejemplo (simulación de base de datos)
        private readonly List<Cliente> _clientes;
        private readonly List<Articulos> _articulos;
        private readonly List<Pedido> _pedidos;
        private readonly List<DetallePedido> _detallePedidos;

        public MainWindow()
        {
            InitializeComponent();

            // Inicializar datos de ejemplo
            _clientes = new List<Cliente>
            {
                new Cliente { IdCliente = 1, NIF = "12345678A", Nombre = "Nicolás Jové Cubillo", Domicilio = "Av. América, 45 - 28002 Madrid" }
            };

            _articulos = new List<Articulos>
            {
                new Articulos { IdArticulo = 1, Descripcion = "Producto A - Alta Calidad", Stock = 100, Precio = 25.50m },
                new Articulos { IdArticulo = 2, Descripcion = "Producto B - Edición Limitada", Stock = 50, Precio = 45.00m },
                new Articulos { IdArticulo = 3, Descripcion = "Producto C - Premium", Stock = 75, Precio = 15.75m }
            };

            _pedidos = new List<Pedido>
            {
                new Pedido { IdPedido = 1, Fecha = new DateTime(2025, 11, 24), IdCliente = 1 },
            };

            _detallePedidos = new List<DetallePedido>
            {
                new DetallePedido { IdDetalle = 1, IdPedido = 1, IdArticulo = 1, Cantidad = 5 },
                new DetallePedido { IdDetalle = 2, IdPedido = 1, IdArticulo = 2, Cantidad = 2 },
                new DetallePedido { IdDetalle = 3, IdPedido = 2, IdArticulo = 2, Cantidad = 3 },
                new DetallePedido { IdDetalle = 4, IdPedido = 2, IdArticulo = 3, Cantidad = 10 }
            };

            DatePickerFecha.SelectedDate = DateTime.Today;
        }

        private void ButtonGenerarFactura_Click(object sender, RoutedEventArgs e)
        {
            TextBlockMensaje.Text = string.Empty;

            // Validar fecha
            if (!DatePickerFecha.SelectedDate.HasValue)
            {
                TextBlockMensaje.Text = "Por favor, seleccione una fecha.";
                return;
            }

            // Validar ID Cliente
            if (!int.TryParse(TextBoxIdCliente.Text, out int idCliente) || idCliente <= 0)
            {
                TextBlockMensaje.Text = "Por favor, ingrese un ID de cliente válido.";
                return;
            }

            // Diálogo para guardar el archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"Factura_Cliente{idCliente}_{DatePickerFecha.SelectedDate.Value:yyyyMMdd}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    GenerarFacturaPDF(
                        DatePickerFecha.SelectedDate.Value,
                        idCliente,
                        saveFileDialog.FileName);

                    MessageBox.Show(
                        $"Factura generada exitosamente en:\n{saveFileDialog.FileName}",
                        "Éxito",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Abrir el PDF generado
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = saveFileDialog.FileName,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error al generar la factura:\n{ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void GenerarFacturaPDF(DateTime fecha, int idCliente, string rutaArchivo)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var cliente = _clientes.FirstOrDefault(c => c.IdCliente == idCliente);
            if (cliente == null)
                throw new Exception($"Cliente con ID {idCliente} no encontrado");

            var pedido = _pedidos.FirstOrDefault(p => p.Fecha.Date == fecha.Date && p.IdCliente == idCliente);
            if (pedido == null)
                throw new Exception($"No hay pedidos para el cliente {idCliente} en la fecha {fecha:dd/MM/yyyy}");

            var detalles = _detallePedidos.Where(d => d.IdPedido == pedido.IdPedido).ToList();
            var lineasFactura = new List<LineaFactura>();

            foreach (var detalle in detalles)
            {
                var articulo = _articulos.FirstOrDefault(a => a.IdArticulo == detalle.IdArticulo);
                if (articulo != null)
                {
                    lineasFactura.Add(new LineaFactura
                    {
                        Descripcion = articulo.Descripcion,
                        Cantidad = detalle.Cantidad,
                        Precio = articulo.Precio
                    });
                }
            }

            var datosEmpresa = new DatosEmpresa();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Element(c => GenerarEncabezado(c, datosEmpresa));

                    page.Content()
                        .Element(c => GenerarContenido(c, cliente, pedido, lineasFactura));

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ");
                            x.CurrentPageNumber();
                            x.Span(" de ");
                            x.TotalPages();
                        });
                });
            })
            .GeneratePdf(rutaArchivo);
        }

        private void GenerarEncabezado(IContainer container, DatosEmpresa empresa)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    // Columna izquierda: datos de la empresa
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text(empresa.Nombre).Bold().FontSize(16).FontColor(Colors.Blue.Darken3);
                        col.Item().Text($"CIF: {empresa.CIF}").FontSize(10);
                        col.Item().Text(empresa.Domicilio).FontSize(10);
                        col.Item().Text(empresa.Ciudad).FontSize(10);
                        col.Item().Text($"Tel: {empresa.Telefono}").FontSize(10);
                    });

                    // Columna derecha: logo
                    row.ConstantItem(100).Column(logoContainer =>
                    {
                        if (System.IO.File.Exists(empresa.RutaLogo))
                        {
                            logoContainer.Item().Height(60).Image(empresa.RutaLogo);
                        }
                        else
                        {
                            logoContainer.Item().Height(60)
                                .Border(1).BorderColor(Colors.Grey.Lighten2)
                                .AlignCenter().AlignMiddle()
                                .Text("LOGO").FontSize(8).FontColor(Colors.Grey.Medium);
                        }
                    });
                });

                column.Item().PaddingVertical(10).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
            });
        }

        private void GenerarContenido(IContainer container, Cliente cliente, Pedido pedido, List<LineaFactura> lineas)
        {
            container.Column(col =>
            {
                // Información del cliente y fecha
                col.Item().PaddingVertical(15).Row(row =>
                {
                    row.RelativeItem().Column(clienteCol =>
                    {
                        clienteCol.Item().Text("DATOS DEL CLIENTE").Bold().FontSize(12);
                        clienteCol.Item().PaddingTop(5).Text($"NIF: {cliente.NIF}");
                        clienteCol.Item().Text($"Nombre: {cliente.Nombre}");
                        clienteCol.Item().Text($"Domicilio: {cliente.Domicilio}");
                    });

                    row.RelativeItem().Column(fechaCol =>
                    {
                        fechaCol.Item().AlignRight().Text("FACTURA").Bold().FontSize(14);
                        fechaCol.Item().PaddingTop(5).AlignRight().Text($"Nº: {pedido.IdPedido:D6}");
                        fechaCol.Item().AlignRight().Text($"Fecha: {pedido.Fecha:dd/MM/yyyy}");
                    });
                });

                // Tabla de detalles
                col.Item().PaddingTop(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Descripción
                        columns.RelativeColumn(1); // Cantidad
                        columns.RelativeColumn(1); // Precio
                        columns.RelativeColumn(1); // Importe
                    });

                    // Encabezado de tabla
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Descripción").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignCenter().Text("Cantidad").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight().Text("Precio").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight().Text("Importe").Bold();
                    });

                    // Líneas de detalle
                    foreach (var linea in lineas)
                    {
                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(linea.Descripcion);
                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignCenter().Text(linea.Cantidad.ToString());
                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text($"{linea.Precio:N2} €");
                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text($"{linea.Importe:N2} €");
                    }
                });

                // Total
                var total = lineas.Sum(l => l.Importe);
                col.Item().PaddingTop(20).AlignRight().Row(row =>
                {
                    row.ConstantItem(150).Background(Colors.Blue.Lighten4).Padding(10).Text($"TOTAL: {total:N2} €").Bold().FontSize(14);
                });
            });
        }
    }
}