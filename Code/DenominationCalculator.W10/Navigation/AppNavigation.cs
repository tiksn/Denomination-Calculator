using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppStudio.Common.Navigation;
using Windows.UI.Xaml;

namespace DenominationCalculator.Navigation
{
    public class AppNavigation
    {
        private NavigationNode _active;

        static AppNavigation()
        {

        }

        public NavigationNode Active
        {
            get
            {
                return _active;
            }
            set
            {
                if (_active != null)
                {
                    _active.IsSelected = false;
                }
                _active = value;
                if (_active != null)
                {
                    _active.IsSelected = true;
                }
            }
        }


        public ObservableCollection<NavigationNode> Nodes { get; private set; }

        public void LoadNavigation()
        {
            Nodes = new ObservableCollection<NavigationNode>();

            Nodes.Add(new ItemNavigationNode
            {
                Title = @"Denomination Calculator",
                Label = "Home",
                IsSelected = true,
                NavigationInfo = NavigationInfo.FromPage("HomePage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "My Flickr Album",
                NavigationInfo = NavigationInfo.FromPage("MyFlickrAlbumListPage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "My Blog",
                NavigationInfo = NavigationInfo.FromPage("MyBlogListPage")
            });

            Nodes.Add(new GroupNavigationNode
            {
                Label = "My Favorite Music",
                Visibility = Visibility.Visible,
                Nodes = new ObservableCollection<NavigationNode>()
                {
                    new ItemNavigationNode
                    {
                        Label = "DJ Nano",
                        NavigationInfo = new NavigationInfo { NavigationType = NavigationType.DeepLink, TargetUri = new Uri("https://music.xbox.com/artist/dj-nano/az.1E990900-0200-11DB-89CA-0019B92A3933") }
                    },
                    new ItemNavigationNode
                    {
                        Label = "Jamestown Revival",
                        NavigationInfo = new NavigationInfo { NavigationType = NavigationType.DeepLink, TargetUri = new Uri("https://music.xbox.com/artist/jamestown-revival/az.B6E01900-0200-11DB-89CA-0019B92A3933") }
                    },
                    new ItemNavigationNode
                    {
                        Label = "Sonnöv",
                        NavigationInfo = new NavigationInfo { NavigationType = NavigationType.DeepLink, TargetUri = new Uri("https://music.xbox.com/artist/sonnov/az.D7095900-0200-11DB-89CA-0019B92A3933") }
                    },
                    new ItemNavigationNode
                    {
                        Label = "StandStill",
                        NavigationInfo = new NavigationInfo { NavigationType = NavigationType.DeepLink, TargetUri = new Uri("https://music.xbox.com/artist/standstill/az.CAB20000-0200-11DB-89CA-0019B92A3933") }
                    },
                    new ItemNavigationNode
                    {
                        Label = "Havalina",
                        NavigationInfo = new NavigationInfo { NavigationType = NavigationType.DeepLink, TargetUri = new Uri("https://music.xbox.com/artist/havalina-rail-co/az.8A571300-0200-11DB-89CA-0019B92A3933") }
                    },
                    new ItemNavigationNode
                    {
                        Label = "V3ctors",
                        NavigationInfo = new NavigationInfo { NavigationType = NavigationType.DeepLink, TargetUri = new Uri("https://music.xbox.com/artist/v3ctors/az.301B5200-0200-11DB-89CA-0019B92A3933") }
                    },
                }
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "About Me",
                NavigationInfo = NavigationInfo.FromPage("AboutMeListPage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "About",
                NavigationInfo = NavigationInfo.FromPage("AboutPage")
            });
            Nodes.Add(new ItemNavigationNode
            {
                Label = "Privacy terms",
                NavigationInfo = new NavigationInfo()
                {
                    NavigationType = NavigationType.DeepLink,
                    TargetUri = new Uri("http://appstudio.windows.com/home/appprivacyterms", UriKind.Absolute)
                }
            });
        }

        public NavigationNode FindPage(Type pageType)
        {
            return GetAllItemNodes(Nodes).FirstOrDefault(n => n.NavigationInfo.NavigationType == NavigationType.Page && n.NavigationInfo.TargetPage == pageType.Name);
        }

        private IEnumerable<ItemNavigationNode> GetAllItemNodes(IEnumerable<NavigationNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is ItemNavigationNode)
                {
                    yield return node as ItemNavigationNode;
                }
                else if(node is GroupNavigationNode)
                {
                    var gNode = node as GroupNavigationNode;

                    foreach (var innerNode in GetAllItemNodes(gNode.Nodes))
                    {
                        yield return innerNode;
                    }
                }
            }
        }
    }
}
