namespace ShellTabs;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("NewPage11");
    }
}