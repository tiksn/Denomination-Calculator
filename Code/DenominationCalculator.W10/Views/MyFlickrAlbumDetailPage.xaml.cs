using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppStudio.DataProviders.Flickr;
using DenominationCalculator.Sections;
using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Views
{
    public sealed partial class MyFlickrAlbumDetailPage : PageBase
    {
        private DataTransferManager _dataTransferManager;

        public MyFlickrAlbumDetailPage()
        {
            this.ViewModel = new DetailViewModel<FlickrDataConfig, FlickrSchema>(new MyFlickrAlbumConfig());            
            this.InitializeComponent();
        }

        public DetailViewModel<FlickrDataConfig, FlickrSchema> ViewModel { get; set; }        

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync(navParameter as ItemViewModel);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataTransferManager.DataRequested -= OnDataRequested;

            base.OnNavigatedFrom(e);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ViewModel.ShareContent(args.Request);
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AppBarButton button = sender as AppBarButton;
            int newFontSize = Int32.Parse(button.Tag.ToString());
            mainPanel.BodyFontSize = newFontSize;
            mainPanel.UpdateFontSize();
        }
    }
}
