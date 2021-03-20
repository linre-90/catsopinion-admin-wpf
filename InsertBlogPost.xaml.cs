using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Catsopinion_admin.Logic;
using System.Collections.ObjectModel;

namespace Catsopinion_admin
{
    /// <summary>
    /// Interaction logic for InsertBlogPost.xaml
    /// </summary>
    public partial class InsertBlogPost : Page
    {
        public InsertBlogPost()
        {
            InitializeComponent();
            BuildData();
            this.FillDropDown(seriesDropdown, this.ReadSeries());
            this.FillDropDown(localesDrobdown, this.ReadLocales());
        }

        /* Buttons back to main */
        private void GoBackToMain(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("MainMenu.xaml", UriKind.Relative));
        }
        private async void SaveNewPost(object sender, RoutedEventArgs e)
        {
            if (this.ValidatePost())
            {
                MessageBoxResult wannaInsert = MessageBox.Show("Are you sure you want to add post?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(wannaInsert == MessageBoxResult.Yes)
                {
                    CollectionHandler collectionHandler = new CollectionHandler();
                    FirebaseHandler firebaseHandler = new FirebaseHandler();
                    string[] response = await firebaseHandler.SaveToDb(this.BuildData(), collectionHandler.GetBlogCollection(localesDrobdown.Text));
                    if (response[0].Equals("200"))
                    {
                        _ = MessageBox.Show("Blog post added", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        _ = MessageBox.Show(response[1], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    _ = MessageBox.Show("Inserting cancelled!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                _ = MessageBox.Show("Some fields are empty", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }

        private string[] ReadSeries()
        {
            CollectionHandler collectionHandler = new CollectionHandler();
            string series = collectionHandler.GetSeries();
            return series.Split(",");
        }
        private string[] ReadLocales()
        {
            CollectionHandler collectionHandler = new CollectionHandler();
            string series = collectionHandler.GetLocales();
            return series.Split(",");
        }


        private void FillDropDown(ComboBox box, string[] data)
        {
            ObservableCollection<string> observableData = new ObservableCollection<string>();
            foreach (string item in data)
            {
                observableData.Add(item);
            }
            box.ItemsSource = observableData;
        }

        // stupid but fast to write
        private bool ValidatePost()
        {
            if(seriesDropdown.Text.Length < 2 || localesDrobdown.Text.Length < 2)
            {
                return false;
            }
            if (title.Text.Length < 1)
            {
                return false;
            }
            if (headerIMG.Text.Length < 1)
            {
                return false;
            }
            if(description.Text.Length < 1 || view.Text.Length < 1)
            {
                return false;
            }

            return true;
        }


        private Dictionary<string, object> BuildData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            DateTime thisDay = DateTime.Today;
            data["date"] = thisDay.Month + "/" + thisDay.Day + "/" + thisDay.Year;
            data["dislikes"] = 0;
            data["likes"] = 0;
            //comes from form
            data["series"] = seriesDropdown.Text;
            data["description"] = description.Text;
            data["locale"] = localesDrobdown.Text;
            data["headerIMG"] = headerIMG.Text;
            data["title"] = title.Text;
            data["view"] = view.Text;
            return data;
        }
    }
}
