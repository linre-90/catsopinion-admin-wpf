using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Catsopinion_admin
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        // contains only button navigation
        private void NavigateToNews(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("NewsPage.xaml", UriKind.Relative));
        }

        private void NavigateToMessages(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("messages.xaml", UriKind.Relative));
        }

        private void NavigateToBlog(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("InsertBlogPost.xaml", UriKind.Relative));
        }
    }
}
