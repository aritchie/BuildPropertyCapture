namespace Sample.Maui;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }
}