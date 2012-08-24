using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

using MediaWikiPublisher.Model;

using Microsoft.Win32;

namespace MediaWikiPublisher.UI
{
    public class MainViewModel : DependencyObject
    {
        private readonly Window view;
        private readonly RelayCommand importCommand;

        public MainViewModel(Window view)
        {
            this.view = view;
            importCommand = new RelayCommand(ImportCommandExecuted);

            Tabs = new ObservableCollection<TabViewModel> { new StartPageTabViewModel() };
            SelectedTab = Tabs[0];
            Documents = new ObservableCollection<WikiContent>();
        }

        public RelayCommand ImportCommand { get { return importCommand; } }

        #region TabsProperty

        public ObservableCollection<TabViewModel> Tabs
        {
            get { return (ObservableCollection<TabViewModel>)GetValue(TabsProperty); }
            set { SetValue(TabsProperty, value); }
        }

        public static readonly DependencyProperty TabsProperty =
            DependencyProperty.Register(
                "Tabs",
                typeof(ObservableCollection<TabViewModel>),
                typeof(MainViewModel),
                new PropertyMetadata(default(ObservableCollection<TabViewModel>), OnTabsDependencyPropertyChanged));

        private static void OnTabsDependencyPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var sender = obj as MainViewModel;

            if (sender != null)
            {
                sender.OnTabsPropertyChanged((ObservableCollection<TabViewModel>)args.OldValue, (ObservableCollection<TabViewModel>)args.NewValue);
            }
        }

        protected virtual void OnTabsPropertyChanged(ObservableCollection<TabViewModel> oldValue, ObservableCollection<TabViewModel> newValue)
        {
        }

        #endregion

        #region SelectedTabProperty

        public TabViewModel SelectedTab
        {
            get { return (TabViewModel)GetValue(SelectedTabProperty); }
            set { SetValue(SelectedTabProperty, value); }
        }

        public static readonly DependencyProperty SelectedTabProperty =
            DependencyProperty.Register(
                "SelectedTab",
                typeof(TabViewModel),
                typeof(MainViewModel),
                new PropertyMetadata(default(TabViewModel), OnSelectedTabDependencyPropertyChanged));

        private static void OnSelectedTabDependencyPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var sender = obj as MainViewModel;

            if (sender != null)
            {
                sender.OnSelectedTabPropertyChanged((TabViewModel)args.OldValue, (TabViewModel)args.NewValue);
            }
        }

        protected virtual void OnSelectedTabPropertyChanged(TabViewModel oldValue, TabViewModel newValue)
        {
            var t = newValue as DocumentTabViewModel;
            if (t != null)
            {
                SelectedDocument = t.Content;
            }
        }

        #endregion

        #region DocumentsProperty

        public ObservableCollection<WikiContent> Documents
        {
            get { return (ObservableCollection<WikiContent>)GetValue(DocumentsProperty); }
            set { SetValue(DocumentsProperty, value); }
        }

        public static readonly DependencyProperty DocumentsProperty =
            DependencyProperty.Register(
                "Documents",
                typeof(ObservableCollection<WikiContent>),
                typeof(MainViewModel),
                new PropertyMetadata(default(ObservableCollection<WikiContent>), OnDocumentsDependencyPropertyChanged));

        private static void OnDocumentsDependencyPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var sender = obj as MainViewModel;

            if (sender != null)
            {
                sender.OnDocumentsPropertyChanged((ObservableCollection<WikiContent>)args.OldValue, (ObservableCollection<WikiContent>)args.NewValue);
            }
        }

        protected virtual void OnDocumentsPropertyChanged(ObservableCollection<WikiContent> oldValue, ObservableCollection<WikiContent> newValue)
        {
        }

        #endregion

        #region ActiveDocumentProperty

        public WikiContent SelectedDocument
        {
            get { return (WikiContent)GetValue(SelectedDocumentProperty); }
            set { SetValue(SelectedDocumentProperty, value); }
        }

        public static readonly DependencyProperty SelectedDocumentProperty =
            DependencyProperty.Register(
                "SelectedDocument",
                typeof(WikiContent),
                typeof(MainViewModel),
                new PropertyMetadata(default(WikiContent), OnSelectedDocumentDependencyPropertyChanged));

        private static void OnSelectedDocumentDependencyPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var sender = obj as MainViewModel;

            if (sender != null)
            {
                sender.OnSelectedDocumentPropertyChanged((WikiContent)args.OldValue, (WikiContent)args.NewValue);
            }
        }

        protected virtual void OnSelectedDocumentPropertyChanged(WikiContent oldValue, WikiContent newValue)
        {
            SelectedTab = Tabs.OfType<DocumentTabViewModel>().FirstOrDefault(_ => _.Content == newValue);
        }

        #endregion

        #region IsBusyProperty

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(
                "IsBusy",
                typeof(bool),
                typeof(MainViewModel),
                new PropertyMetadata(default(bool)));


        #endregion

        private void ImportCommandExecuted()
        {
            IsBusy = true;
            SelectFileForOpening(
                "Import Wiki data",
                WikiImporter.Filter,
                files => new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext())
                             .ContinueWhenAll(
                                 files.Select(WikiImporter.ImportAsync).ToArray(),
                                 tasks =>
                                 {
                                     var errors = (from t in tasks
                                                   where t.Status == TaskStatus.Faulted
                                                   let exception = t.Exception
                                                   where exception != null
                                                   from e in exception.InnerExceptions
                                                   select e.Message)
                                         .ToList();

                                     if (errors.Any())
                                     {
                                         ImportErrorsWindow.Show(view, errors);
                                     }

                                     foreach (var wikiContent in
                                         from t in tasks
                                         where t.Status == TaskStatus.RanToCompletion
                                         select t.Result)
                                     {
                                         Documents.Add(wikiContent);
                                         Tabs.Add(new DocumentTabViewModel(wikiContent));
                                         SelectedDocument = wikiContent;
                                     }

                                     IsBusy = false;
                                 }));
        }

        private void SelectFileForOpening(
            string title,
            string filter,
            Action<IEnumerable<string>> handler)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog
            {
                Title = title,
                Filter = filter,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                Multiselect = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                handler(dialog.FileNames);
            }
        }
    }
}
