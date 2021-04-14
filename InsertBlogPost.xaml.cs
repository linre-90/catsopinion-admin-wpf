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
    /// Interaction logic for InsertBlogPost.xaml
    /// </summary>
    public partial class InsertBlogPost : Page
    {
        public InsertBlogPost()
        {
            InitializeComponent();
            BuildData();
            this.ReadSeries();
            this.FillDropDown(localesDrobdown, this.ReadLocales());
        }

        /// <summary>
        /// Function for navigation.
        /// Changes view to MainMenu.xaml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackToMain(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("MainMenu.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Save button functionality.
        /// Calls methods ValidatePost() and BuildData() 
        /// Asks user confirmation to validate saving or chance to back out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveNewPost(object sender, RoutedEventArgs e)
        {
            if (this.ValidatePost())
            {
                MessageBoxResult wannaInsert = MessageBox.Show("Are you sure you want to add post?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (wannaInsert == MessageBoxResult.Yes)
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

        /// <summary>method <c>ReadSeries</c> Reads series information from firestore. Sets cursor to wait and releases it after data is fetched</summary>
        private async void ReadSeries()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            FirebaseHandler firebaseHandler = new FirebaseHandler();
            List<Dictionary<string, object>> data = await firebaseHandler.GetCollection("series");
            List<string> temp = new List<string>();
            foreach (Dictionary<string, object> x in data)
            {
                x.TryGetValue("name", out object name);
                temp.Add(name.ToString());
            }
            this.FillDropDown(seriesDropdown, temp.ToArray());
            Mouse.OverrideCursor = null;
        }

        /// <summary>method <c>ReadLocales</c> Reads locales from app.config file.</summary><returns>Nothing</returns>
        private string[] ReadLocales()
        {
            CollectionHandler collectionHandler = new CollectionHandler();
            string series = collectionHandler.GetLocales();
            return series.Split(",");
        }

        /// <summary>
        /// <c>FillDropDown</c>
        /// Fills dropdown with given data
        /// </summary>
        /// <param name="box">Combobox item</param>
        /// <param name="data">string array</param>
        /// <returns>Nothing</returns>
        /// 
        private void FillDropDown(ComboBox box, string[] data)
        {
            ObservableCollection<string> observableData = new ObservableCollection<string>();
            foreach (string item in data)
            {
                observableData.Add(item);
            }
            box.ItemsSource = observableData;
        }

        /// <summary>
        /// Validates form data. Checks only that everyfield has something in it.
        /// Is stupid, make better function at some point.
        /// </summary>
        /// <returns>True on valid form othervise False</returns>
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

        /// <summary>
        /// Builds data to dictionary.
        /// Returned data is ready to be send to FirebaseHandler for saving.
        /// </summary>
        /// <returns> Dictionary</returns>
        private Dictionary<string, object> BuildData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            DateTime thisDay = DateTime.Today;
            data["date"] = thisDay.Month + "/" + thisDay.Day.ToString("00") + "/" + thisDay.Year;
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
