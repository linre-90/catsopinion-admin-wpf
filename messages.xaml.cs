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
    /// Interaction logic for messages.xaml
    /// DEPRECATED. WILL BE REMOVED!!
    /// </summary>
    public partial class messages : Page
    {
        private List<Dictionary<string, object>> data = null;
        private string currentMessage = null;

        public messages()
        {
            InitializeComponent();
            this.SetData();
        }
        /* Buttons back to main */
        private void GoBackToMain(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("MainMenu.xaml", UriKind.Relative));
        }
        // message scroll buttons
        private void MessageScrollerButtons(object sender, RoutedEventArgs e)
        {
            Button invoker = sender as Button;
            int buttonType = -1;
            if (invoker.Name.Equals("next"))
            {
                buttonType = 1;
            }
            int i = this.FindIndex(this.currentMessage);
            if(i >= 0)
            {
                if (i + buttonType > this.data.Count - 1 || i + buttonType < 0)
                {
                    this.InsertToDom(this.data[0]);
                }
                else
                {
                    this.InsertToDom(this.data[i+buttonType]);
                }
            }
        }
        // remove button
        private async void RemoveMessage(object sender, RoutedEventArgs e)
        {
            MessageBoxResult wannaRemove = MessageBox.Show("Are you sure you want to remove?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(wannaRemove == MessageBoxResult.Yes)
            {
                // collection does get removed if empty, prevent that
                if(this.data.Count > 1)
                {
                    FirebaseHandler firebaseHandler = new FirebaseHandler();
                    CollectionHandler collectionH = new CollectionHandler();
                    string[] response = await firebaseHandler.DeleteFromDb(this.currentMessage, collectionH.GetMessageCollection());
                    if (response[0].Equals("200"))
                    {
                        int index = this.FindIndex(this.currentMessage);
                        try
                        {
                            this.data.RemoveAt(index);
                            this.InsertToDom(this.data[0]);
                            _ = MessageBox.Show("Document deleted!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ed)
                        {
                            _ = MessageBox.Show(ed.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    //error from firebase
                    else
                    {
                        _ = MessageBox.Show(response[1], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                // cannot remove only document
                else
                {
                    _ = MessageBox.Show("Nothing removed! Collection length must be at least one.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            // cancelled remove
            else
            {
                _ = MessageBox.Show("Nothing removed!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /* class functions */
        // gets data from db List<Dic<string, object>> and sets it to private variable
        private async void SetData()
        {
            // dummy text for user
            messageHeader.Text = "Loading messages, please wait!";
            // change mouse cursor to load
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                FirebaseHandler fbh = new FirebaseHandler();
                CollectionHandler collectionHandler = new CollectionHandler();
                this.data = await fbh.GetCollection(collectionHandler.GetMessageCollection());
                //release mouse cursor to normal
                Mouse.OverrideCursor = null;
                if (this.data[0] != null)
                {
                    // insert messages to "DOM" or ui
                    this.InsertToDom(this.data[0]);
                }
            }
            catch (Exception e)
            {
                messageHeader.Text = "Error occured: " + e.Message;
            }
        }
        
        // insert message to "DOM", read values from dictionary 
        private void InsertToDom(Dictionary<string, object> item)
        {
            this.currentMessage = item["id"].ToString();
            messageHeader.Text = "Headline: " + item["headline"].ToString();
            messageType.Text = "Type: " + item["type"].ToString();
            messageMessage.Text = "Message: " + item["message"].ToString();
            messageDate.Text = "Date: " + item["date"].ToString();
        }
        //get current messages index in local list
        private int FindIndex(string idNow)
        {
            int i = 0;
            // loop trough dictionary list and when key==current id brake and we have index. Data amount is only few peaces, if evolves to larger figure out something else
            foreach (Dictionary<string, object> message in this.data)
            {
                if (message["id"].Equals(idNow))
                {
                    return i;
                }
                else
                {
                    i++;
                }
            }
            return -1;
        }
    }
}
