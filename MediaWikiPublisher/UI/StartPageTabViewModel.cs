namespace MediaWikiPublisher.UI
{
    public class StartPageTabViewModel : TabViewModel
    {
        public StartPageTabViewModel()
        {
            CloseCommand.IsEnabled = false;
        }

        #region Overrides of TabViewModel

        public override string Title
        {
            get { return "start page"; }
        }

        #endregion
    }
}
