using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace WpfAppExcelXML
{
    public partial class MainWindow : Window
    {
        private List<Votaciones> _baseVotos;

        public class Votaciones
        {
            public string Nombre { get; set; }
            public int numParti { get; set; }
            public decimal porcentajeParti { get; set; }
            public string voto { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            _baseVotos = new List<Votaciones>
            {
                new Votaciones {Nombre = "a", numParti = 300,  porcentajeParti = 24.69m, voto = "SI" },
                new Votaciones {Nombre = "b", numParti = 200,  porcentajeParti = 16.46m, voto = "NO" },
                new Votaciones {Nombre = "c", numParti = 150,  porcentajeParti = 12.35m, voto = "SI" },
                new Votaciones {Nombre = "d", numParti = 20,   porcentajeParti = 1.65m,  voto = "SI" },
                new Votaciones {Nombre = "e", numParti = 40,   porcentajeParti = 3.29m,  voto = "NO" },
                new Votaciones {Nombre = "f", numParti = 160,  porcentajeParti = 13.17m, voto = "SI" },
                new Votaciones {Nombre = "g", numParti = 50,   porcentajeParti = 4.12m,  voto = "SI" },
                new Votaciones {Nombre = "h", numParti = 120,  porcentajeParti = 9.83m,  voto = "SI" },
                new Votaciones {Nombre = "i", numParti = 80,   porcentajeParti = 6.58m,  voto = "ABS" },
                new Votaciones {Nombre = "j", numParti = 95,   porcentajeParti = 7.82m,  voto = "SI" }
            };

            dgProductos.ItemsSource = _baseVotos;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Usar la fuente simulada de votaciones (_baseVotos)
            var datosParaReporte = _baseVotos.ToList();

            if (!datosParaReporte.Any()) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Libro de Excel (*.xlsx)|*.xlsx",
                FileName = $"Votacion_SanVicente_{System.DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    GenerarExcelVotaciones(datosParaReporte, saveFileDialog.FileName);

                    var result = MessageBox.Show("Excel generado. ¿Deseas abrirlo?", "Éxito", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        AbrirArchivo(saveFileDialog.FileName);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void GenerarExcelVotaciones(List<Votaciones> datos, string rutaArchivo)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Junta");

                // Título
                worksheet.Cell("A1").Value = "BANCO SANVICENTE";
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Font.FontSize = 16;
                worksheet.Cell("A1").Style.Font.FontColor = XLColor.FromHtml("#C00000");
                worksheet.Range("A1:D1").Merge();

                worksheet.Cell("A2").Value = "JUNTA DE ACCIONISTAS";
                worksheet.Cell("A2").Style.Font.Bold = true;
                worksheet.Range("A2:D2").Merge();

                // Encabezado de la tabla
                int filaInicio = 4;
                worksheet.Cell(filaInicio, 1).Value = "Nombre";
                worksheet.Cell(filaInicio, 2).Value = "n-part.";
                worksheet.Cell(filaInicio, 3).Value = "%part.";
                worksheet.Cell(filaInicio, 4).Value = "VOTO";

                var rangoCabecera = worksheet.Range(filaInicio, 1, filaInicio, 4);
                rangoCabecera.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");
                rangoCabecera.Style.Font.FontColor = XLColor.White;
                rangoCabecera.Style.Font.Bold = true;
                rangoCabecera.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Rellenar datos
                int filaActual = filaInicio + 1;
                foreach (var item in datos)
                {
                    worksheet.Cell(filaActual, 1).Value = item.Nombre;
                    worksheet.Cell(filaActual, 2).Value = item.numParti;
                    // voto inicial (puede ser SI/NO/ABS)
                    if (!string.IsNullOrWhiteSpace(item.voto))
                        worksheet.Cell(filaActual, 4).Value = item.voto;
                    filaActual++;
                }

                int ultimaFilaDatos = filaActual - 1;
                int filaTotal = ultimaFilaDatos + 1;

                // TOTAL participaciones
                worksheet.Cell(filaTotal, 1).Value = "TOTAL";
                worksheet.Cell(filaTotal, 2).FormulaA1 = $"=SUM(B{filaInicio + 1}:B{ultimaFilaDatos})";
                worksheet.Cell(filaTotal, 2).Style.Font.Bold = true;

                // %part. por fila (referenciado al TOTAL)
                for (int r = filaInicio + 1; r <= ultimaFilaDatos; r++)
                {
                    worksheet.Cell(r, 3).FormulaA1 = $"=IF($B${filaTotal}=0,0,B{r}/$B${filaTotal})";
                    worksheet.Cell(r, 3).Style.NumberFormat.Format = "0.00%";
                }

                // Data validation: lista desplegable en columna VOTO
                var rangoVotos = worksheet.Range(worksheet.Cell(filaInicio + 1, 4), worksheet.Cell(ultimaFilaDatos, 4));
                var dv = rangoVotos.CreateDataValidation();
                dv.List("SI,NO,ABS");
                dv.IgnoreBlanks = true;
                dv.InCellDropdown = true;

                // Formato de tabla: ajustar columnas y bordes
                worksheet.Columns(1, 4).AdjustToContents();
                var rangoDatos = worksheet.Range(filaInicio, 1, filaTotal, 4);
                rangoDatos.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rangoDatos.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Resultados
                int colRes = 6; 
                int resFila = filaInicio;

                worksheet.Cell(resFila, colRes).Value = "Resultados:";
                worksheet.Cell(resFila, colRes).Style.Font.Bold = true;

                // SI
                int siFila = resFila + 1;
                worksheet.Cell(siFila, colRes).Value = "SI (votos)";
                worksheet.Cell(siFila, colRes + 1).FormulaA1 = $"=COUNTIF(D{filaInicio + 1}:D{ultimaFilaDatos},\"SI\")";
                worksheet.Cell(siFila, colRes + 2).Value = "Participaciones";
                worksheet.Cell(siFila, colRes + 3).FormulaA1 = $"=SUMIF(D{filaInicio + 1}:D{ultimaFilaDatos},\"SI\",B{filaInicio + 1}:B{ultimaFilaDatos})";
                worksheet.Cell(siFila, colRes + 3).Style.NumberFormat.Format = "#,##0";

                // NO
                int noFila = siFila + 1;
                worksheet.Cell(noFila, colRes).Value = "NO (votos)";
                worksheet.Cell(noFila, colRes + 1).FormulaA1 = $"=COUNTIF(D{filaInicio + 1}:D{ultimaFilaDatos},\"NO\")";
                worksheet.Cell(noFila, colRes + 2).Value = "Participaciones";
                worksheet.Cell(noFila, colRes + 3).FormulaA1 = $"=SUMIF(D{filaInicio + 1}:D{ultimaFilaDatos},\"NO\",B{filaInicio + 1}:B{ultimaFilaDatos})";
                worksheet.Cell(noFila, colRes + 3).Style.NumberFormat.Format = "#,##0";

                // ABS
                int absFila = noFila + 1;
                worksheet.Cell(absFila, colRes).Value = "ABS (votos)";
                worksheet.Cell(absFila, colRes + 1).FormulaA1 = $"=COUNTIF(D{filaInicio + 1}:D{ultimaFilaDatos},\"ABS\")";
                worksheet.Cell(absFila, colRes + 2).Value = "Participaciones";
                worksheet.Cell(absFila, colRes + 3).FormulaA1 = $"=SUMIF(D{filaInicio + 1}:D{ultimaFilaDatos},\"ABS\",B{filaInicio + 1}:B{ultimaFilaDatos})";
                worksheet.Cell(absFila, colRes + 3).Style.NumberFormat.Format = "#,##0";

                // Porcentajes de participaciones 
                int porcFila = absFila + 1;
                worksheet.Cell(porcFila, colRes).Value = "% SI";
                worksheet.Cell(porcFila, colRes + 1).FormulaA1 = $"=IF($B${filaTotal}=0,0,({GetAddress(siFila, colRes + 3)})/$B${filaTotal})";
                worksheet.Cell(porcFila, colRes + 1).Style.NumberFormat.Format = "0.00%";

                worksheet.Cell(porcFila + 1, colRes).Value = "% NO";
                worksheet.Cell(porcFila + 1, colRes + 1).FormulaA1 = $"=IF($B${filaTotal}=0,0,({GetAddress(noFila, colRes + 3)})/$B${filaTotal})";
                worksheet.Cell(porcFila + 1, colRes + 1).Style.NumberFormat.Format = "0.00%";

                worksheet.Cell(porcFila + 2, colRes).Value = "% ABS";
                worksheet.Cell(porcFila + 2, colRes + 1).FormulaA1 = $"=IF($B${filaTotal}=0,0,({GetAddress(absFila, colRes + 3)})/$B${filaTotal})";
                worksheet.Cell(porcFila + 2, colRes + 1).Style.NumberFormat.Format = "0.00%";

                // Total votantes y participaciones 
                int auxFila = porcFila + 4;
                worksheet.Cell(auxFila, colRes).Value = "Total votantes";
                worksheet.Cell(auxFila, colRes + 1).FormulaA1 = $"=COUNTA(D{filaInicio + 1}:D{ultimaFilaDatos})";

                worksheet.Cell(auxFila + 1, colRes).Value = "Participaciones (Total)";
                worksheet.Cell(auxFila + 1, colRes + 1).FormulaA1 = $"=$B${filaTotal}";
                worksheet.Cell(auxFila + 1, colRes + 1).Style.NumberFormat.Format = "#,##0";

                // Resultado: APROBADO si participaciones SI / total participaciones > 66%
                int resultadoFila = auxFila + 3;
                worksheet.Cell(resultadoFila, colRes).Value = "Resultado";
                worksheet.Cell(resultadoFila, colRes + 1).FormulaA1 = $"=IF( ( {GetAddress(siFila, colRes + 3)} / $B${filaTotal} ) > 0.66, \"APROBADO\", \"NO APROBADO\")";
                worksheet.Cell(resultadoFila, colRes + 1).Style.Font.Bold = true;

                // --- Inserta una fila y una columna en blanco para crear margen ---
                worksheet.Row(1).InsertRowsAbove(1);
                worksheet.Column(1).InsertColumnsBefore(1);
                // ---------------------------------------------------------------

                // Ajustes finales
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(rutaArchivo);
            }
        }

        // Helper para construir una referencia A1 a una celda (retorna por ejemplo H6)
        private string GetAddress(int row, int column)
        {
            return $"{XLHelper.GetColumnLetterFromNumber(column)}{row}";
        }

        private void AbrirArchivo(string ruta)
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }
    }
}