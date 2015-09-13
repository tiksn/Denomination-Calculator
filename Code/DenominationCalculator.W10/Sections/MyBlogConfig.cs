using System;
using System.Collections.Generic;
using AppStudio.Common.Actions;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Rss;
using DenominationCalculator.Config;
using DenominationCalculator.ViewModels;

namespace DenominationCalculator.Sections
{
    public class MyBlogConfig : SectionConfigBase<RssDataConfig, RssSchema>
    {
        public override DataProviderBase<RssDataConfig, RssSchema> DataProvider
        {
            get
            {
                return new RssDataProvider();
            }
        }

        public override RssDataConfig Config
        {
            get
            {
                return new RssDataConfig
                {
                    Url = new Uri("http://lego.gizmodo.com/rss")
                };
            }
        }

        public override NavigationInfo ListNavigationInfo
        {
            get 
            {
                return NavigationInfo.FromPage("MyBlogListPage");
            }
        }

        public override ListPageConfig<RssSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<RssSchema>
                {
                    Title = "My Blog",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.Description = item.Summary.ToSafeString();
                        viewModel.Image = item.ImageUrl.ToSafeString();

                    },
                    NavigationInfo = (item) =>
                    {
                        return NavigationInfo.FromPage("MyBlogDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<RssSchema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, RssSchema>>();

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Title.ToSafeString();
                    viewModel.Title = "";
                    viewModel.Description = item.Content.ToSafeString();
                    viewModel.Image = "";
                    viewModel.Content = null;
                });

				var actions = new List<ActionConfig<RssSchema>>
				{
                    ActionConfig<RssSchema>.Link("Go To Source", (item) => item.FeedUrl.ToSafeString()),
				};

                return new DetailPageConfig<RssSchema>
                {
                    Title = "My Blog",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }

        public override string PageTitle
        {
            get { return "My Blog"; }
        }

    }
}
