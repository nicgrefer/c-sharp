using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Windows;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAppSpeech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] comandos = { "hola", "asistente", "detener", "cerrar aplicación" }; // Comandos para el reconocimiento

        // 1. Locución (Text-to-Speech)
        private SpeechSynthesizer synth = new SpeechSynthesizer();

        // 2. Reconocimiento (Speech-to-Text)
        private SpeechRecognitionEngine recognizer;
        private bool isRecognizerReady = false; // Bandera para el estado del micrófono

        public MainWindow()
        {
            InitializeComponent();
            // Inicializar el locutor
            synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);

            // Inicializar el motor de reconocimiento
            InitializeSpeechRecognizer();
        }
        private void InitializeSpeechRecognizer()
        {
            try
            {
                // Usa la cultura para especificar el idioma del reconocimiento
                recognizer = new SpeechRecognitionEngine(new CultureInfo("es-ES"));

                // Definir las palabras o frases que el motor debe escuchar.
                Choices commands = new Choices();
                commands.Add(comandos);

                /*
                 * Crea un constructor de gramática (GrammarBuilder) a partir de un conjunto de opciones (Choices, en tu caso commands). 
                 * El GrammarBuilder define la estructura de las frases que el motor intentará reconocer (por ejemplo, una sola palabra de entre las opciones).
                 */
                GrammarBuilder gb = new GrammarBuilder(commands);

                /*
                 * Finalmente, crea una gramática (Grammar) a partir del GrammarBuilder. 
                 * La gramática es lo que realmente se carga en el motor de reconocimiento para que pueda identificar las frases definidas.
                 * Convierte el GrammarBuilder en un objeto Grammar concreto que el reconocimiento puede usar. 
                 * Grammar es la representación final (puede incluir un nombre, culturas, o venir de SRGS XML).
                 */
                Grammar grammar = new Grammar(gb);

                /* Registra esta Grammar en el SpeechRecognitionEngine. 
                 * A partir de ese momento el motor considerará las frases de esa gramática cuando procese audio, lo que mejora precisión y limita lo que se reconoce a los elementos definidos.
                 */
                recognizer.LoadGrammar(grammar);

                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

                // *** GESTIÓN DE EXCEPCIÓN AQUÍ ***
                // Esta línea lanzará una excepción si no hay un micrófono disponible.
                recognizer.SetInputToDefaultAudioDevice();

                // Configurar los tiempos de espera
                recognizer.InitialSilenceTimeout = TimeSpan.FromSeconds(5); //tiempo máximo a esperar antes de que comience la primera voz.
                recognizer.BabbleTimeout = TimeSpan.FromSeconds(3); //tiempo máximo para ruido/voz no válida al inicio.
                recognizer.EndSilenceTimeout = TimeSpan.FromSeconds(1); //tiempo que espera tras el final de la voz para considerar la entrada terminada.

                // Si llegamos aquí, el reconocimiento está listo
                isRecognizerReady = true;
                BtnReconocimiento.Content = "Iniciar Reconocimiento";
            }
            catch (InvalidOperationException ex)
            {
                // Si el motor no puede encontrar el dispositivo de audio.
                TxtResultadoSR.Text = "ERROR: Micrófono no disponible o configurado. El reconocimiento de voz está deshabilitado.";
                BtnReconocimiento.IsEnabled = false; // Deshabilitar el botón
                isRecognizerReady = false;

                // Opcional: Mostrar el error detallado para el desarrollador
                MessageBox.Show($"Error de hardware de voz: {ex.Message}", "Error de Micrófono", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Para cualquier otro error durante la inicialización
                TxtResultadoSR.Text = $"ERROR al inicializar el reconocimiento: {ex.Message}";
                BtnReconocimiento.IsEnabled = false;
                isRecognizerReady = false;
            }
        }
        // --- EVENTOS DE LOCUCIÓN (TTS) ---
        private void BtnLocutar_Click(object sender, RoutedEventArgs e)
        {
            string textToSpeak = TxtLocucion.Text;
            if (!string.IsNullOrEmpty(textToSpeak))
            {
                // La llamada SpeakAsync no bloquea la UI de WPF
                synth.SpeakAsync(textToSpeak);
            }
        }

        // --- EVENTOS DE RECONOCIMIENTO (SR) ---

        // 1. Iniciar/Detener el reconocimiento
        private void BtnReconocimiento_Click(object sender, RoutedEventArgs e)
        {
            // Solo intentar si el reconocimiento está listo (micrófono OK)
            if (!isRecognizerReady)
            {
                TxtResultadoSR.Text = "El reconocimiento no está listo. Revisa tu micrófono.";
                return;
            }
            try
            {
                synth.SpeakAsync("Iniciando el reconocimiento de voz.");
                // Comienza a escuchar una sola vez
                // recognizer.RecognizeAsync(RecognizeMode.Single);

                // Comienza a escuchar de forma continua hasta que lo detengamos
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                BtnReconocimiento.Content = "Escuchando...";
                TxtResultadoSR.Text = $"Esperando la entrada de voz...\nComandos: {string.Join(", ", comandos)}\n";
            }
            catch (Exception ex)
            {
                // Si falla durante la ejecución (poco común si la inicialización fue bien)
                MessageBox.Show($"Error al iniciar la escucha: {ex.Message}", "Error SR");
                BtnReconocimiento.Content = "Iniciar Reconocimiento (Error)";
            }
        }


        // 2. Manejador de evento cuando se reconoce algo
        private async void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Capturar el texto reconocido
            string recognizedText = e.Result.Text;

            TxtResultadoSR.Text = $"Reconocido: {recognizedText}\n";

            // Procesar el comando de forma asíncrona
            await HandleCommandAsync(recognizedText);
        }

        private async Task HandleCommandAsync(string recognizedText)
        {
            string voz = recognizedText.ToLower();
            switch (voz)
            {
                case "hola":
                    {
                        string userName = Environment.UserName;
                        string saludo = DateTime.Now.Hour < 12 ? "Buenos días" : "Buenas tardes";
                        await SpeakTextAsync($"{saludo} {userName}. ¿Cómo estás?");
                        break;
                    }
                case "asistente":
                    await SpeakTextAsync("¿En qué puedo ayudarte?");
                    break;
                case "detener":
                    recognizer.RecognizeAsyncCancel(); // o RecognizeAsyncStop() si prefieres esperar a que termine la frase
                    await SpeakTextAsync("Reconocimiento de voz detenido");
                    BtnReconocimiento.Content = "Iniciar Reconocimiento"; // Resetear el botón
                    break;
                case "cerrar aplicación":
                    await SpeakTextAsync("Aaadios");
                    // Esperar un momento para que termine la locución y luego cerrar
                    await Task.Delay(500);
                    Close();
                    break;
                default:
                    break;
            }
        }

        private Task SpeakTextAsync(string text)
        {
            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<SpeakCompletedEventArgs>? handler = null;
            handler = (s, e) =>
            {
                synth.SpeakCompleted -= handler;
                if (e.Error != null) tcs.TrySetException(e.Error);
                else if (e.Cancelled) tcs.TrySetCanceled();
                else tcs.TrySetResult(null);
            };

            synth.SpeakCompleted += handler;
            try
            {
                synth.SpeakAsync(text);
            }
            catch (Exception ex)
            {
                synth.SpeakCompleted -= handler;
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }

        // Limpieza al cerrar la ventana
        protected override void OnClosed(EventArgs e)
        {
            // Liberar recursos
            recognizer?.Dispose();
            synth?.Dispose();
            base.OnClosed(e);
        }

        private void TxtLocucion_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}