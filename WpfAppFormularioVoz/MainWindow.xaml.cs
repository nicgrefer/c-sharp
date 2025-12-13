using System;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Input; // Necesario para Keyboard.FocusedElement

namespace WpfAppSpeech
{
    public partial class MainWindow : Window
    {
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synth = new SpeechSynthesizer();
        private bool isListening = false;

        // Comandos de navegación predefinidos
        private string[] comandosNavegacion = {
            "editar nombre",
            "editar ciudad",
            "editar edad",
            "borrar campo",
            "detener"
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpeechRecognizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
        }

        private void InitializeSpeechRecognizer()
        {
            try
            {
                recognizer = new SpeechRecognitionEngine(new CultureInfo("es-ES"));

                // 1. CARGAR GRAMÁTICA DE COMANDOS (Prioridad Alta)
                // Son frases exactas que la app debe obedecer siempre.
                Choices sList = new Choices();
                sList.Add(comandosNavegacion);
                GrammarBuilder gb = new GrammarBuilder(sList);
                Grammar commandGrammar = new Grammar(gb);
                commandGrammar.Name = "ComandosSistema"; // Le damos nombre para identificarla
                recognizer.LoadGrammar(commandGrammar);

                // 2. CARGAR GRAMÁTICA DE DICTADO (Texto Libre)
                // Esto permite reconocer nombres, ciudades, números, etc.
                DictationGrammar dictation = new DictationGrammar();
                dictation.Name = "DictadoLibre";
                recognizer.LoadGrammar(dictation);

                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                recognizer.SetInputToDefaultAudioDevice();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error microfono: {ex.Message}");
            }
        }

        private void BtnEscuchar_Click(object sender, RoutedEventArgs e)
        {
            if (!isListening)
            {
                try
                {
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);
                    isListening = true;
                    BtnEscuchar.Content = "DETENER ESCUCHA";
                    TxtEstado.Text = "Estado: Escuchando...";
                    TxtEstado.Foreground = System.Windows.Media.Brushes.Green;

                    // Poner foco inicial en el primer campo
                    TxtNombre.Focus();
                }
                catch (Exception ex) { MessageBox.Show("Error al iniciar: " + ex.Message); }
            }
            else
            {
                DetenerEscucha();
            }
        }

        private void DetenerEscucha()
        {
            if (isListening)
            {
                recognizer.RecognizeAsyncCancel();
                isListening = false;
                BtnEscuchar.Content = "INICIAR ESCUCHA";
                TxtEstado.Text = "Estado: Pausado";
                TxtEstado.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // --- LÓGICA PRINCIPAL ---
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string textoReconocido = e.Result.Text;
            float confianza = e.Result.Confidence;

            // Filtro de calidad (ignorar ruidos bajos)
            if (confianza < 0.4) return;

            TxtDebug.Text = $"Recibido: {textoReconocido} ({confianza:P0})";

            // 1. VERIFICAR SI ES UN COMANDO DE NAVEGACIÓN
            // Convertimos a minúsculas para comparar fácilmente
            string comando = textoReconocido.ToLower();

            if (comandosNavegacion.Contains(comando))
            {
                EjecutarComando(comando);
            }
            else
            {
                // 2. SI NO ES COMANDO, ES TEXTO PARA RELLENAR
                EscribirEnCampoActivo(textoReconocido);
            }
        }

        private void EjecutarComando(string comando)
        {
            switch (comando)
            {
                case "editar nombre":
                    synth.SpeakAsync("Campo Nombre");
                    TxtNombre.Focus();
                    break;
                case "editar ciudad":
                    synth.SpeakAsync("Campo Ciudad");
                    TxtCiudad.Focus();
                    break;
                case "editar edad":
                    synth.SpeakAsync("Campo Edad");
                    TxtEdad.Focus();
                    break;
                case "borrar campo":
                    // Borra el contenido del TextBox que tenga el foco
                    if (Keyboard.FocusedElement is TextBox textBox)
                    {
                        textBox.Text = "";
                        synth.SpeakAsync("Borrado");
                    }
                    break;
                case "detener":
                    synth.SpeakAsync("Deteniendo reconocimiento");
                    DetenerEscucha();
                    break;
            }
        }

        private void EscribirEnCampoActivo(string texto)
        {
            // Verificamos qué control tiene el foco actualmente en la ventana
            // Usamos Dispatcher para asegurar que accedemos a la UI desde el hilo correcto
            Dispatcher.Invoke(() =>
            {
                if (Keyboard.FocusedElement is TextBox textBoxActual)
                {
                    // Lógica opcional: Si el campo es "Edad", intentar filtrar solo números (opcional)
                    if (textBoxActual.Name == "TxtEdad")
                    {
                        // Intentar limpiar texto si solo queremos números, 
                        // o dejarlo así si confiamos en el usuario.
                        // Ejemplo simple: no hacemos nada especial, solo pegamos.
                    }

                    // Añadir espacio si ya hay texto
                    if (!string.IsNullOrEmpty(textBoxActual.Text))
                        textBoxActual.Text += " ";

                    textBoxActual.Text += texto;

                    // Mover el cursor al final del texto
                    textBoxActual.CaretIndex = textBoxActual.Text.Length;
                }
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            recognizer?.Dispose();
            synth?.Dispose();
            base.OnClosed(e);
        }
    }
}