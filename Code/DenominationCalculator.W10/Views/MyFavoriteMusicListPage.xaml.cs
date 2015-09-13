using Windows.UI.Xaml.Navigation;
using AppStudio.Common;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders.Menu;
using DenominationCalculator;
using DenominationCalculator.Sections;
using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Views
{
    public sealed partial class MyFavoriteMusicListPage : PageBase     {
        public MyFavoriteMusicListPage()
        {
            this.ViewModel = new ListViewModel<LocalStorageDataConfig, MenuSchema>(new MyFavoriteMusicConfig());
            this.InitializeComponent();
}

        public ListViewModel<LocalStorageDataConfig, MenuSchema> ViewModel { get; set; }
        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
