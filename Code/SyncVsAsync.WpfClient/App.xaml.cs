using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using LightInject;

namespace SyncVsAsync.WpfClient
{
    public partial class App
    {
        private readonly ServiceContainer _container = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _container.Register<MainWindow>()
                      .Register<MainWindowViewModel>()
                      .Register<WebApiPerformanceManager>()
                      .RegisterInstance(httpClient);

            MainWindow = _container.GetInstance<MainWindow>();
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container.Dispose();
            base.OnExit(e);
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            _container.Dispose();
            base.OnSessionEnding(e);
        }
    }
}