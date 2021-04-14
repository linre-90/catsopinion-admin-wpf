using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Catsopinion_admin.Logic;


namespace Catsopinion_admin
{
    /// <summary>
    /// Interaction logic for NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {
        private readonly CollectionHandler collectionHandler = null;

        public NewsPage()
        {
            InitializeComponent();
            this.collectionHandler = new CollectionHandler();
            dateBox.Text = DateToday();
        }


        /// <summary>
        /// Navigation route to main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainMenu.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Opens remove view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenRemove(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewsPageDelete.xaml", UriKind.Relative));
        }



        /// <summary>
        /// Saves new news to database
        /// Controls mouse cursor, sets it to loading and when done to normal. TODO form validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveToDb(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Dictionary<string, object> formData = new Dictionary<string, object>
            {
                { "date", dateBox.Text },
                { "headline", headlineBox.Text },
                { "locale", localeBox.Text },
                { "message", messageBox.Text }
            };
            FirebaseHandler FH = new FirebaseHandler();
            string[] result = await FH.SaveToDb(formData, this.collectionHandler.GetNewsCollection(formData["locale"].ToString()));
            Mouse.OverrideCursor = null;
            if (result[0].Equals("200"))
            {
                _ = MessageBox.Show(result[1], "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(result[1], "Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Return current date for time stamp
        /// </summary>
        /// <returns></returns>
        private string DateToday()
        {
            DateTime thisDay = DateTime.Today;
            return thisDay.ToString("d");
        }
    }
}
