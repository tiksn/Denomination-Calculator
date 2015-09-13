using System;
using System.Collections.Generic;
using AppStudio.Common.Actions;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Flickr;
using DenominationCalculator.Config;
using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Sections
{
    public class MyFlickrAlbumConfig : SectionConfigBase<FlickrDataConfig, FlickrSchema>
    {
        public override DataProviderBase<FlickrDataConfig, FlickrSchema> DataProvider
        {
            get
            {
                return new FlickrDataProvider();
            }
        }

        public override FlickrDataConfig Config
        {
            get
            {
                return new FlickrDataConfig
                {
                    QueryType = FlickrQueryType.Id,
                    Query = @"100292344@N05"
                };
            }
        }

        public override NavigationInfo ListNavigationInfo
        {
            get 
            {
                return NavigationInfo.FromPage("MyFlickrAlbumListPage");
            }
        }

        public override ListPageConfig<FlickrSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<FlickrSchema>
                {
                    Title = "My Flickr Album",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.Description = null;
                        viewModel.Image = item.ImageUrl.ToSafeString();

                    },
                    NavigationInfo = (item) =>
                    {
                        return NavigationInfo.FromPage("MyFlickrAlbumDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<FlickrSchema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, FlickrSchema>>();

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Title.ToSafeString();
                    viewModel.Title = "";
                    viewModel.Description = item.Summary.ToSafeString();
                    viewModel.Image = item.ImageUrl.ToSafeString();
                    viewModel.Content = null;
                });

				var actions = new List<ActionConfig<FlickrSchema>>
				{
                    ActionConfig<FlickrSchema>.Link("Go To Source", (item) => item.FeedUrl.ToSafeString()),
				};

                return new DetailPageConfig<FlickrSchema>
                {
                    Title = "My Flickr Album",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }

        public override string PageTitle
        {
            get { return "My Flickr Album"; }
        }

    }
}
