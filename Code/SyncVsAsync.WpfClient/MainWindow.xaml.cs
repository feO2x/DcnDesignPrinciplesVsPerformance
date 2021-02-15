using Light.GuardClauses;

namespace SyncVsAsync.WpfClient
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel viewModel) : this()
        {
            DataContext = viewModel.MustNotBeNull(nameof(viewModel));
        }
    }
}