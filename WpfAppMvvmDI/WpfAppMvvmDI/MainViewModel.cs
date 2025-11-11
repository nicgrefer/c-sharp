using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfAppMvvmDI
{
    public partial class MainViewModel: ObservableObject
    {
        // Dependencias de los ViewModels de página
        private readonly HomeViewModel _homeViewModel;
        private readonly SettingsViewModel _settingsViewModel;

        // Propiedad que almacena el ViewModel actualmente visible
        [ObservableProperty]
        private ObservableObject _currentViewModel;

        // Comandos para cambiar el ViewModel actual
        public IRelayCommand NavigateHomeCommand { get; }
        public IRelayCommand NavigateSettingsCommand { get; }

        // ¡Inyección de Dependencias en el constructor!
        public MainViewModel(HomeViewModel homeViewModel, SettingsViewModel settingsViewModel)
        {
            _homeViewModel = homeViewModel;
            _settingsViewModel = settingsViewModel;

            // Definir los comandos
            NavigateHomeCommand = new RelayCommand(NavigateToHome);
            NavigateSettingsCommand = new RelayCommand(NavigateToSettings);

            // Establecer la vista inicial
            _currentViewModel = _homeViewModel;
        }

        private void NavigateToHome()
        {
            CurrentViewModel = _homeViewModel;
        }

        private void NavigateToSettings()
        {
            CurrentViewModel = _settingsViewModel;
        }
    }
}


