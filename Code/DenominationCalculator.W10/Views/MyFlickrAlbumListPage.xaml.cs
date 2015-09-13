using Windows.UI.Xaml.Navigation;
using AppStudio.Common;
using AppStudio.DataProviders.Flickr;
using DenominationCalculator;
using DenominationCalculator.Sections;
using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Views
{
    public sealed partial class MyFlickrAlbumListPage : PageBase     {
        public MyFlickrAlbumListPage()
        {
            this.ViewModel = new ListViewModel<FlickrDataConfig, FlickrSchema>(new MyFlickrAlbumConfig());
            this.InitializeComponent();
}

        public ListViewModel<FlickrDataConfig, FlickrSchema> ViewModel { get; set; }
        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
