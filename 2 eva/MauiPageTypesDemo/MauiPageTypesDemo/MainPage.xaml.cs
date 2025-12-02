using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MauiPageTypesDemo
{
    // --- 1. ContentPage (Página de Contenido) ---
    // La página base, simple, que contiene una sola vista.
    public class SimpleContentPage : ContentPage
    {
        public SimpleContentPage(string title, Color backgroundColor)
        {
            Title = title;
            BackgroundColor = backgroundColor;

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 10,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = $"¡Hola desde {title}!",
                        FontSize = 24,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Colors.DarkSlateGray
                    },
                    new Label
                    {
                        Text = "Esta es una ContentPage simple, el bloque de construcción fundamental de MAUI.",
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        MaxLines = 3,
                        TextColor = Colors.DarkSlateGray
                    }
                }
            };
        }
    }

    // --- 4. TabbedPage (Página con Pestañas) ---
    // Contiene múltiples ContentPages a las que se accede a través de pestañas.
    public class MyTabbedPage : TabbedPage
    {
        public MyTabbedPage()
        {
            Title = "Página con Pestañas";
            BarBackgroundColor = Colors.DarkCyan;
            BarTextColor = Colors.White;

            // Agregamos ContentPages como hijos
            Children.Add(new SimpleContentPage("Datos", Color.FromArgb("#E0F7FA")));
            Children.Add(new SimpleContentPage("Configuración", Color.FromArgb("#E8F5E9")));
            Children.Add(new SimpleContentPage("Acerca de", Color.FromArgb("#FFF3E0")));
        }
    }

    // --- 3. FlyoutPage (Página Desplegable) - Menú Lateral (Flyout) ---
    public class MyFlyoutMenu : ContentPage
    {
        public MyFlyoutMenu(MyFlyoutPage parent)
        {
            Title = "Menú Lateral";
            BackgroundColor = Color.FromArgb("#4A148C"); // Púrpura oscuro

            var menuLayout = new StackLayout
            {
                Padding = new Thickness(10, 50, 10, 10),
                Spacing = 15
            };

            var header = new Label
            {
                Text = "NAVEGACIÓN PRINCIPAL",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            };

            var option1Button = new Button { Text = "Página Detalle 1", BackgroundColor = Color.FromArgb("#9C27B0"), TextColor = Colors.White };
            option1Button.Clicked += (s, e) =>
            {
                // Al hacer clic, se actualiza el Detail de la FlyoutPage
                parent.Detail = new NavigationPage(new SimpleContentPage("Detalle Opción 1", Color.FromArgb("#F3E5F5")));
                parent.IsPresented = false; // Cierra el menú
            };

            var option2Button = new Button { Text = "Página Detalle 2", BackgroundColor = Color.FromArgb("#8E24AA"), TextColor = Colors.White };
            option2Button.Clicked += (s, e) =>
            {
                // Al hacer clic, se actualiza el Detail de la FlyoutPage
                parent.Detail = new NavigationPage(new SimpleContentPage("Detalle Opción 2", Color.FromArgb("#E1BEE7")));
                parent.IsPresented = false; // Cierra el menú
            };

            menuLayout.Children.Add(header);
            menuLayout.Children.Add(option1Button);
            menuLayout.Children.Add(option2Button);

            Content = new ScrollView { Content = menuLayout };
        }
    }

    // --- 3. FlyoutPage (Página Desplegable) - Contenedor Principal
    public class MyFlyoutPage : FlyoutPage
    {
        public MyFlyoutPage()
        {
            Title = "Página Desplegable (Flyout)";

            // 3a. Flyout (El menú lateral, usando una ContentPage)
            Flyout = new MyFlyoutMenu(this);

            // 3b. Detail (El contenido principal, envuelto en una NavigationPage para la barra de título)
            Detail = new NavigationPage(new SimpleContentPage("Detalle Inicial", Color.FromArgb("#FFFFFF")))
            {
                BarBackgroundColor = Color.FromArgb("#512DA8"),
                BarTextColor = Colors.White
            };

            // Permite ver el menú al iniciar en pantallas grandes
            FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
        }
    }

    // --- 2. NavigationPage (Página de Navegación) y Página de Inicio ---
    // Esta página demuestra la navegación Push/Pop.
    public class MainAppPage : ContentPage
    {
        public MainAppPage()
        {
            Title = "Demo de Páginas MAUI";
            BackgroundColor = Color.FromArgb("#F5F5F5"); // Gris claro

            var navigateToContentButton = new Button
            {
                Text = "1. Navegar a ContentPage (Push)",
                BackgroundColor = Colors.OrangeRed,
                TextColor = Colors.White,
                CornerRadius = 8
            };
            navigateToContentButton.Clicked += async (s, e) =>
            {
                // PushAsync agrega una página a la pila de NavigationPage
                await Navigation.PushAsync(new SimpleContentPage("Página Empujada", Colors.Coral));
            };

            var navigateToTabbedButton = new Button
            {
                Text = "2. Mostrar TabbedPage",
                BackgroundColor = Colors.DarkCyan,
                TextColor = Colors.White,
                CornerRadius = 8
            };
            navigateToTabbedButton.Clicked += async (s, e) =>
            {
                // Navegamos a la TabbedPage
                await Navigation.PushAsync(new MyTabbedPage());
            };

            var navigateToFlyoutButton = new Button
            {
                Text = "3. Mostrar FlyoutPage",
                BackgroundColor = Colors.Indigo,
                TextColor = Colors.White,
                CornerRadius = 8
            };
            navigateToFlyoutButton.Clicked += async (s, e) =>
            {
                // Navegamos a la FlyoutPage
                await Navigation.PushAsync(new MyFlyoutPage());
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 20,
                    Spacing = 20,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            Text = ".NET MAUI 8 - Demostración de Tipos de Página",
                            FontSize = 28,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalOptions = LayoutOptions.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Colors.Navy
                        },
                        new Label
                        {
                            Text = "La página actual está envuelta en una NavigationPage, lo que habilita los botones 'Atrás' y la pila de navegación.",
                            HorizontalOptions = LayoutOptions.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 10),
                            TextColor = Colors.Gray
                        },
                        navigateToContentButton,
                        navigateToTabbedButton,
                        navigateToFlyoutButton
                    }
                }
            };
        }
    }


}
