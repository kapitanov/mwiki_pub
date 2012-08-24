using System.Collections.Generic;
using System.Windows;

namespace MediaWikiPublisher.UI
{
    public partial class ImportErrorsWindow
    {
        private ImportErrorsWindow(IEnumerable<string> errors)
        {
            InitializeComponent();
            ErrorsList.ItemsSource = errors;
        }

        public static void Show(Window parent, IEnumerable<string> errors)
        {
            new ImportErrorsWindow(errors) { Owner = parent }.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
