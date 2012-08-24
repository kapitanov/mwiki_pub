namespace MediaWikiPublisher.UI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(this);
        }
    }
}
