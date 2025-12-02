namespace MauiPageTypesDemo
{
    public partial class App : Application
    {
        public App()
        {
            //InitializeComponent();

            // Establecemos la NavigationPage como la MainPage. Esto es clave para 
            // que MainAppPage y todas las páginas que empujemos tengan navegación.
            MainPage = new NavigationPage(new MainAppPage())
            {
                BarBackgroundColor = Colors.Navy,
                BarTextColor = Colors.White
            };
        }
    }
}
