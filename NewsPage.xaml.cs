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

        private void BackBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainMenu.xaml", UriKind.Relative));
        }

        private void OpenRemove(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewsPageDelete.xaml", UriKind.Relative));
        }

        private async void SaveToDb(object sender, RoutedEventArgs e)
        {
            // show that we are loading and not freezing
            Mouse.OverrideCursor = Cursors.Wait;
            Dictionary<string, object> formData = new Dictionary<string, object>
            {
                { "date", dateBox.Text },
                { "headline", headlineBox.Text },
                { "locale", localeBox.Text },
                { "message", messageBox.Text }
            };

            // TODO form validation with dictionary

            FirebaseHandler FH = new FirebaseHandler();
            string[] result = await FH.SaveToDb(formData, this.collectionHandler.GetNewsCollection(formData["locale"].ToString()));
            // back to normal
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

        private string DateToday()
        {
            DateTime thisDay = DateTime.Today;
            return thisDay.ToString("d");
        }
    }
}
