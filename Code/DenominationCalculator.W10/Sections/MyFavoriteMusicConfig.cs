using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders.Menu;
using DenominationCalculator.Config;
using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Sections
{
    public class MyFavoriteMusicConfig : SectionConfigBase<LocalStorageDataConfig, MenuSchema>
    {
        public override DataProviderBase<LocalStorageDataConfig, MenuSchema> DataProvider
        {
            get
            {
                return new LocalStorageDataProvider<MenuSchema>();
            }
        }

        public override LocalStorageDataConfig Config
        {
            get
            {
                return new LocalStorageDataConfig
                {
                    FilePath = "/Assets/Data/MyFavoriteMusic.json"
                };
            }
        }

        public override NavigationInfo ListNavigationInfo
        {
            get 
            {
                return NavigationInfo.FromPage("MyFavoriteMusicListPage");
            }
        }


        public override ListPageConfig<MenuSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<MenuSchema>
                {
                    Title = "My Favorite Music",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title;
                        viewModel.Image = item.Icon;
                    },
                    NavigationInfo = (item) =>
                    {
                        return item.ToNavigationInfo();
                    }
                };
            }
        }

        public override DetailPageConfig<MenuSchema> DetailPage
        {
            get { return null; }
        }

        public override string PageTitle
        {
            get { return "My Favorite Music"; }
        }

    }
}
