using Microsoft.UI.Xaml;

namespace Probo.EasyLoader
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            new MainWindow().Activate();
        }
    }
}
