namespace ShellTabs
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));
            Routing.RegisterRoute(nameof(NewPage2), typeof(NewPage2));
            Routing.RegisterRoute(nameof(NewPage3), typeof(NewPage3));
            Routing.RegisterRoute(nameof(NewPage11), typeof(NewPage11));

        }
    }
}
