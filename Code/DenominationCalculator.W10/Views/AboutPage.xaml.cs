using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Views
{
    public sealed partial class AboutPage : PageBase
    {
        public AboutPage()
        {
            AboutThisAppModel = new AboutThisAppViewModel();
            this.InitializeComponent();
        }

        public AboutThisAppViewModel AboutThisAppModel { get; private set; }

        protected override void LoadState(object navParameter)
        {
        }
    }
}
