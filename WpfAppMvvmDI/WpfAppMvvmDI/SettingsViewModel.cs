using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfAppMvvmDI
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _enableNotifications = true;
    }
}

