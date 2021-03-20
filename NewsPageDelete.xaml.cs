using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Catsopinion_admin.Logic;
using System.Collections.ObjectModel;

namespace Catsopinion_admin
{
    /// <summary>
    /// Interaction logic for NewsPageDelete.xaml
    /// </summary>
    public partial class NewsPageDelete : Page
    {
        List<Dictionary<string, object>> news = null;
        ObservableCollection<dataModel> dataCollection = null;
        

        public NewsPageDelete()
        {
            InitializeComponent();
            this.news = new List<Dictionary<string, object>>();
            this.dataCollection = new ObservableCollection<dataModel>();
            dataListGrid.ItemsSource = this.dataCollection;
            this.GetData();
        }

        private void BackToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainMenu.xaml", UriKind.Relative));
        }

        private async void DeleteMessage(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete?", "Deleting", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes && idToDelete.Text.Length > 0)
            {
                FirebaseHandler firebaseHandler = new FirebaseHandler();
                CollectionHandler collection = new CollectionHandler();
                string locale = "";
                int index = 0;
                foreach (dataModel item in this.dataCollection)
                {
                    if (item.id.Equals(idToDelete.Text))
                    {
                        locale = item.locale;
                        index = this.dataCollection.IndexOf(item);
                    }
                }
                if(locale.Equals("fi") || locale.Equals("en"))
                {
                    string[] response = await firebaseHandler.DeleteFromDb(idToDelete.Text, collection.GetNewsCollection(locale));
                    if (response[0].Equals("200"))
                    {
                        this.dataCollection.RemoveAt(index);
                        _ = MessageBox.Show("Deleted succesfully", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        _ = MessageBox.Show(response[1], "Error happened", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    _ = MessageBox.Show("Error cannot get locale from news delete. Are you sure id is valid?", "Error happened", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (idToDelete.Text.Length == 0)
            {
                _ = MessageBox.Show("Id to delete cannot be empty!", "Id is empty", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _ = MessageBox.Show("Nothing deleted!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void GetData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                FirebaseHandler fbh = new FirebaseHandler();
                CollectionHandler collectionHandler = new CollectionHandler();
                List<Dictionary<string, object>> list1 = await fbh.GetCollection(collectionHandler.GetNewsCollection("fi"));
                List<Dictionary<string, object>> list2 = await fbh.GetCollection(collectionHandler.GetNewsCollection("en"));
                this.news.AddRange(list1);
                this.news.AddRange(list2);
                Mouse.OverrideCursor = null;
                this.FillDataCollection();
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message, "There was an error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FillDataCollection()
        {
            foreach (Dictionary<string,object> item in this.news)
            {
                dataModel temp = new dataModel
                {
                    id = item["id"].ToString(),
                    date = item["date"].ToString(),
                    headline = item["headline"].ToString(),
                    locale = item["locale"].ToString(),
                    message = item["message"].ToString()
                };
                this.dataCollection.Add(temp);
                
            }
        }
    }

    public class dataModel
    {
        public string id { get; set; }
        public string date { get; set; }
        public string headline { get; set; }
        public string locale { get; set; }
        public string message { get; set; }
    }
}
