namespace MediaWikiPublisher.UI
{
    public abstract class TabViewModel
    {
        public TabViewModel()
        {
            CloseCommand = new RelayCommand(CloseCommandExecuted);
        }

        public abstract string Title { get; }

        public RelayCommand CloseCommand { get; private set; }

        protected virtual void CloseCommandExecuted()
        {
            
        }
    }

}
