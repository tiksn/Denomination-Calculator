using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.Common;
using AppStudio.Common.Actions;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Flickr;
using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.Menu;
using AppStudio.DataProviders.Html;
using AppStudio.DataProviders.LocalStorage;
using DenominationCalculator.Sections;


namespace DenominationCalculator.ViewModels
{
    public class MainViewModel : ObservableBase
    {
        public MainViewModel(int visibleItems)
        {
            PageTitle = "Denomination Calculator";
            MyFlickrAlbum = new ListViewModel<FlickrDataConfig, FlickrSchema>(new MyFlickrAlbumConfig(), visibleItems);
            MyBlog = new ListViewModel<RssDataConfig, RssSchema>(new MyBlogConfig(), visibleItems);
            MyFavoriteMusic = new ListViewModel<LocalStorageDataConfig, MenuSchema>(new MyFavoriteMusicConfig());
            AboutMe = new ListViewModel<LocalStorageDataConfig, HtmlSchema>(new AboutMeConfig(), visibleItems);
            Actions = new List<ActionInfo>();

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = new RelayCommand(Refresh),
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    ActionType = ActionType.Primary
                });
            }
        }

        public string PageTitle { get; set; }
        public ListViewModel<FlickrDataConfig, FlickrSchema> MyFlickrAlbum { get; private set; }
        public ListViewModel<RssDataConfig, RssSchema> MyBlog { get; private set; }
        public ListViewModel<LocalStorageDataConfig, MenuSchema> MyFavoriteMusic { get; private set; }
        public ListViewModel<LocalStorageDataConfig, HtmlSchema> AboutMe { get; private set; }

        public RelayCommand<INavigable> SectionHeaderClickCommand
        {
            get
            {
                return new RelayCommand<INavigable>(item =>
                    {
                        NavigationService.NavigateTo(item);
                    });
            }
        }

        public DateTime? LastUpdated
        {
            get
            {
                return GetViewModels().Select(vm => vm.LastUpdated)
                            .OrderByDescending(d => d).FirstOrDefault();
            }
        }

        public List<ActionInfo> Actions { get; private set; }

        public bool HasActions
        {
            get
            {
                return Actions != null && Actions.Count > 0;
            }
        }

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private async void Refresh()
        {
            var refreshDataTasks = GetViewModels()
                                        .Where(vm => !vm.HasLocalData)
                                        .Select(vm => vm.LoadDataAsync(true));

            await Task.WhenAll(refreshDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<DataViewModelBase> GetViewModels()
        {
            yield return MyFlickrAlbum;
            yield return MyBlog;
            yield return MyFavoriteMusic;
            yield return AboutMe;
        }
    }
}
