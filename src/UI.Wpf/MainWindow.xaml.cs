using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Palermo.BlazorMvc;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;

namespace UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
#if DEBUG
            serviceCollection.AddBlazorWebViewDeveloperTools();
            serviceCollection.AddLogging();
#endif
            serviceCollection.AddScoped<IUiBus>(provider => new MvcBus(NullLogger<MvcBus>.Instance));
            serviceCollection.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7174") });
            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }
    }
}