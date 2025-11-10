using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfAppMVVM001
{
    //1 Heredamos de ObservableObject
    public partial class MainViewModel : ObservableObject
    {
        //2 Definimos un campo para el nombre
        // [ObservableProperty] crea automáticamente la propiedad pública "UserName"
        // que notifica al UI cuando cambia su valor.
        [ObservableProperty]
        private string _userName;

        //3 Definimos un comando para el mensaje de saludo
        [ObservableProperty]
        private string _greeting;

        // 4 Definimos el método que se ejecutará cuando se invoque el comando
        // [RelayCommand] crea automáticamente un comando público "ShowGreetingCommand" que el botón puede ejecutar.
        [RelayCommand]
        private void Greet()
        {
            // Logica simple para generar el saludo
            if (!string.IsNullOrEmpty(UserName))
            {
                Greeting = $"Hello, {UserName}!";
            }
            
        }

    }
}
