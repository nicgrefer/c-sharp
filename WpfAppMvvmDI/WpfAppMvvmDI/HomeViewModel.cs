using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfAppMvvmDI
{
    public partial class HomeViewModel : ObservableObject
    {
        // Propiedad simple para demostrar el binding
        public string WelcomeMessage => "¡Bienvenido a la Página de Inicio!";
    }
}

