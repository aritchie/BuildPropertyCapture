namespace Sample.Maui
{
    public partial class App : Application
    {
        readonly IServiceProvider services;

        public App(IServiceProvider services)
        {
            InitializeComponent();
            this.services = services;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var page = this.services.GetRequiredService<MainPage>();
            return new Window(new NavigationPage(page));
        }
    }
}