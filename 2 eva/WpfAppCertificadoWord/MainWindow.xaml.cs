using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace WpfAppCertificado
{
    public partial class MainWindow : Window
    {
        // Definición de Clases según el PDF
        public class Estudiante
        {
            public string ID { get; set; }
            public string Nombre { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string NivelEstudios { get; set; }
            public DateTime FechaMatricula { get; set; }
            public DateTime FechaFinalizacion { get; set; }
        }

        public class Nota
        {
            public string EstudianteID { get; set; }
            public string AsignaturaID { get; set; }
            public string Asignatura { get; set; }
            public double Calificacion { get; set; }
            public int Creditos { get; set; }
        }

        private List<Estudiante> listaEstudiantes;
        private List<Nota> listaNotas;

        public MainWindow()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            // Datos de ejemplo según el PDF
            listaEstudiantes = new List<Estudiante>
            {
                new Estudiante {
                    ID = "E009876",
                    Nombre = "Ana María Torres Soto",
                    FechaNacimiento = new DateTime(2000, 5, 15),
                    NivelEstudios = "Grado en Ingeniería Informática"
                }
            };


            listaNotas = new List<Nota>
            {
                new Nota { EstudianteID = "E009876", AsignaturaID="INF-101", Asignatura = "Programación Avanzada", Calificacion = 8.5, Creditos = 6 },
                new Nota { EstudianteID = "E009876", AsignaturaID="DAT-202", Asignatura = "Bases de Datos", Calificacion = 9.2, Creditos = 6 },
                new Nota { EstudianteID = "E009876", AsignaturaID="MAT-105", Asignatura = "Cálculo I", Calificacion = 6.8, Creditos = 4 }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string idBuscado = "E009876";

            // CONSULTA LINQ EXACTA DEL PDF
            var datosCertificado = listaEstudiantes
                .Where(est => est.ID == idBuscado)
                .Select(est => new
                {
                    NombreCompleto = est.Nombre,
                    ID_Estudiante = est.ID,
                    FechaNacimiento = est.FechaNacimiento,
                    NivelEstudios = est.NivelEstudios,
                    Institucion = "Universidad de Valladolid",
                    FechaEmision = DateTime.Now,
                    // Recupera y agrupa las notas
                    NotasRegistradas = listaNotas
                        .Where(n => n.EstudianteID == est.ID)
                        .Select(n => new
                        {
                            n.AsignaturaID,
                            n.Asignatura,
                            n.Calificacion,
                            n.Creditos
                        }).ToList()
                })
                .FirstOrDefault(); // Toma el primer resultado

            if (datosCertificado == null)
            {
                MessageBox.Show("Estudiante no encontrado.");
                return;
            }

            // Diálogo para guardar
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Documento de Word (*.docx)|*.docx",
                FileName = $"Expediente_{datosCertificado.ID_Estudiante}.docx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Pasamos 'dynamic' porque la consulta LINQ devuelve un tipo anónimo
                    GenerarWord(datosCertificado, saveFileDialog.FileName);

                    var result = MessageBox.Show("Certificado generado. ¿Deseas abrirlo?", "Éxito", MessageBoxButton.YesNo, MessageBoxImage.Information);
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

        // Recibe 'dynamic' para manejar el objeto anónimo de la LINQ
        private void GenerarWord(dynamic datos, string rutaArchivo)
        {
            using (var doc = DocX.Create(rutaArchivo))
            {
                // ==========================================
                // 1. CABECERA CON LOGO Y TÍTULO
                // ==========================================
                var headerTable = doc.AddTable(1, 2);
                headerTable.Design = TableDesign.None;
                headerTable.Alignment = Xceed.Document.NET.Alignment.center;
                headerTable.AutoFit = AutoFit.Window;

                // Columna 1: Logo
                var celdaLogo = headerTable.Rows[0].Cells[0];
                celdaLogo.Width = 80;

                // Buscar el logo en varias ubicaciones y como recurso embebido
                string[] candidates = new[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png"),
                    Path.Combine(Environment.CurrentDirectory, "logo.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "logo.png"),
                    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "logo.png"))
                };

                string found = candidates.FirstOrDefault(File.Exists);
                if (found != null)
                {
                    var img = doc.AddImage(found);
                    var pic = img.CreatePicture(60, 60);
                    celdaLogo.Paragraphs[0].AppendPicture(pic);
                }
                else
                {
                    // Intentar recurso embebido en el ensamblado
                    var asm = Assembly.GetExecutingAssembly();
                    var resName = asm.GetManifestResourceNames()
                                     .FirstOrDefault(n => n.EndsWith("logo.png", StringComparison.OrdinalIgnoreCase));
                    if (resName != null)
                    {
                        using (var stream = asm.GetManifestResourceStream(resName))
                        {
                            var img = doc.AddImage(stream);
                            var pic = img.CreatePicture(60, 60);
                            celdaLogo.Paragraphs[0].AppendPicture(pic);
                        }
                    }
                    else
                    {
                        // Fallback visual si no se encuentra el logo
                        celdaLogo.Paragraphs[0].Append("[LOGO]").Bold().Color(Xceed.Drawing.Color.Red);
                    }
                }

                // Columna 2: Títulos
                var celdaTexto = headerTable.Rows[0].Cells[1];
                // Cualificar VerticalAlignment de Xceed para evitar conflicto con System.Windows
                celdaTexto.VerticalAlignment = Xceed.Document.NET.VerticalAlignment.Center;
                celdaTexto.Paragraphs[0].Append(datos.Institucion.ToString().ToUpper())
                                        .FontSize(16).Bold().Color(Xceed.Drawing.Color.DarkBlue).AppendLine()
                                        .Append("EXPEDIENTE ACADÉMICO")
                                        .FontSize(14).Bold();

                doc.InsertTable(headerTable);
                doc.InsertParagraph(""); // Espaciador

                // ==========================================
                // 2. DATOS DEL ESTUDIANTE
                // ==========================================
                var infoPanel = doc.InsertParagraph();
                infoPanel.Append($"ESTUDIANTE: {datos.NombreCompleto}\n").Bold();
                infoPanel.Append($"DNI/ID: {datos.ID_Estudiante}\n");
                infoPanel.Append($"ESTUDIOS: {datos.NivelEstudios}\n");
                infoPanel.Append($"FECHA DE NACIMIENTO: {datos.FechaNacimiento:dd/MM/yyyy}\n");
                infoPanel.SpacingAfter(20);

                // ==========================================
                // 3. TABLA DE NOTAS
                // ==========================================
                // Columnas requeridas: Código, Asignatura, Créditos ECTS, Calificación
                var notasRegistradasLocal = (System.Collections.IEnumerable)datos.NotasRegistradas;

                // Contar filas para crear la tabla
                int rowCount = 0;
                foreach (var _ in notasRegistradasLocal) rowCount++;

                var tablaNotas = doc.AddTable(rowCount + 1, 4);
                tablaNotas.Design = TableDesign.TableGrid; // Diseño limpio académico
                tablaNotas.Alignment = Xceed.Document.NET.Alignment.center;
                tablaNotas.AutoFit = AutoFit.Window;

                // Cabeceras
                tablaNotas.Rows[0].Cells[0].Paragraphs[0].Append("CÓDIGO").Bold();
                tablaNotas.Rows[0].Cells[1].Paragraphs[0].Append("ASIGNATURA").Bold();
                tablaNotas.Rows[0].Cells[2].Paragraphs[0].Append("CRÉDITOS").Bold().Alignment = Xceed.Document.NET.Alignment.center;
                tablaNotas.Rows[0].Cells[3].Paragraphs[0].Append("CALIFICACIÓN").Bold().Alignment = Xceed.Document.NET.Alignment.center;

                // Rellenar datos
                int i = 1;
                double sumaCreditos = 0; // Para el total

                foreach (dynamic nota in notasRegistradasLocal)
                {
                    tablaNotas.Rows[i].Cells[0].Paragraphs[0].Append(nota.AsignaturaID ?? "---");
                    tablaNotas.Rows[i].Cells[1].Paragraphs[0].Append(nota.Asignatura);
                    tablaNotas.Rows[i].Cells[2].Paragraphs[0].Append(nota.Creditos.ToString()).Alignment = Xceed.Document.NET.Alignment.center;
                    tablaNotas.Rows[i].Cells[3].Paragraphs[0].Append(nota.Calificacion.ToString("N1")).Alignment = Xceed.Document.NET.Alignment.center;

                    sumaCreditos += nota.Creditos;
                    i++;
                }

                doc.InsertTable(tablaNotas);

                // ==========================================
                // 4. TOTALES
                // ==========================================
                var pTotal = doc.InsertParagraph();
                pTotal.SpacingBefore(10);
                pTotal.Alignment = Xceed.Document.NET.Alignment.right;
                pTotal.Append($"TOTAL CRÉDITOS SUPERADOS: {sumaCreditos:N2}").Bold().FontSize(12);

                // ==========================================
                // 5. PIE Y FIRMA
                // ==========================================
                doc.InsertParagraph("\n\n\n"); // Espacio para firma

                var pFirma = doc.InsertParagraph();
                pFirma.Alignment = Xceed.Document.NET.Alignment.center;
                // Formato de fecha solicitado: En Valladolid, a xx de xxx de xxxxx
                string fechaTexto = $"En Valladolid, a {DateTime.Now.Day} de {DateTime.Now.ToString("MMMM")} de {DateTime.Now.Year}";

                pFirma.Append(fechaTexto).AppendLine();

                pFirma.Append("Firma del Director/a").UnderlineStyle(Xceed.Document.NET.UnderlineStyle.singleLine).Bold();
                // ==========================================
                // 6. PIE DE PÁGINA (CSV/Seguridad)
                // ==========================================
                doc.AddFooters();
                var footer = doc.Footers.Odd;
                footer.InsertParagraph("Documento firmado digitalmente. Código Seguro de Verificación (CSV): XYZ-12345-UPV")
                      .FontSize(8).Color(Xceed.Drawing.Color.Gray).Alignment = Xceed.Document.NET.Alignment.center;

                doc.Save();
            }
        }

        private void AbrirArchivo(string ruta)
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
        }
    }
}