using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Catsopinion_admin
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// Privides only navigation options
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        

        /// <summary>
        /// Navigation route to NewsPage.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToNews(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("NewsPage.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Navigation route to messages.xaml. DEPRECATED. WILL BE REMOVED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToMessages(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("messages.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Navigation route to blog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToBlog(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("InsertBlogPost.xaml", UriKind.Relative));
        }
    }
}
