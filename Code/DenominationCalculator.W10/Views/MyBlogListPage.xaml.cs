using Windows.UI.Xaml.Navigation;
using AppStudio.Common;
using AppStudio.DataProviders.Rss;
using DenominationCalculator;
using DenominationCalculator.Sections;
using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Views
{
    public sealed partial class MyBlogListPage : PageBase     {
        public MyBlogListPage()
        {
            this.ViewModel = new ListViewModel<RssDataConfig, RssSchema>(new MyBlogConfig());
            this.InitializeComponent();
}

        public ListViewModel<RssDataConfig, RssSchema> ViewModel { get; set; }
        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
