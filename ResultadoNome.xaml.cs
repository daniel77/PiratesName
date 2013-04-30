using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.IO.IsolatedStorage;

namespace PiratesName
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";


            List<string> nomes = new List<string>();

            nomes.Add("Capitao ");
            nomes.Add("Imediato ");
            nomes.Add("Marujo ");
            nomes.Add("Bucaneiro ");
            nomes.Add("Corsario ");
            nomes.Add("Cachorro Loco ");
            nomes.Add("Almirante ");
            nomes.Add("Contra Mestre ");
            nomes.Add("Navegador ");

            Random rnd = new Random();
            int r = rnd.Next(nomes.Count);


            List<string> sobre = new List<string>();

            sobre.Add("Morgan");
            sobre.Add("Henry");
            sobre.Add("Barba Negra");
            sobre.Add("Barba Ruiva");
            sobre.Add("Jack Daniels");
            sobre.Add("Jack Sparrow");
            sobre.Add("Ursinho Dourado");
            sobre.Add("Magneto");
            sobre.Add("Wolverine");
            sobre.Add("Toddy");
            sobre.Add("Teddy");



            Random rnd2 = new Random();
            int num = rnd2.Next(sobre.Count);



            string nome = nomes.ElementAt(r) + sobre.ElementAt(num);


            if (NavigationContext.QueryString.TryGetValue("nome", out msg))
            {

                nomeOriginal.Text = (msg + ", seu nome eh ");
                

                var settings = IsolatedStorageSettings.ApplicationSettings;

                if (!settings.Contains("nomepirata"))
                {

                    resultado.Text = nome;
                    settings.Add("nomepirata", nome);
                }

                

            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FacebookClient.Instance.PostMessageOnWall("My Pirate name is " + resultado.Text, new UploadStringCompletedEventHandler(PostMessageOnWallCompleted));
        }

        void PostMessageOnWallCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (e.Error != null)
            {
                MessageBox.Show("Error Occurred: " + e.Error.Message);
                return;
            }

            System.Diagnostics.Debug.WriteLine(e.Result);

            string result = e.Result;
            byte[] data = Encoding.UTF8.GetBytes(result);
            MemoryStream memStream = new MemoryStream(data);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ResponseData));
            ResponseData responseData = (ResponseData)serializer.ReadObject(memStream);

            if (responseData.id != null && !responseData.id.Equals(""))
            {
                // Success
                MessageBox.Show("Message Posted!");
            }
            else if (responseData.error != null && responseData.error.code == 190)
            {
                if (responseData.error.error_subcode == 463)
                {
                    // Access Token Expired, need to get new token
                    FacebookClient.Instance.ExchangeAccessToken(new UploadStringCompletedEventHandler(ExchangeAccessTokenCompleted));
                }
                else
                {
                    // Another Error with Access Token, need to clear the Access Token
                    FacebookClient.Instance.AccessToken = "";
                    ///SetLoggedInState(false);
                }
            }
            else
            {
                // Error
            }
        }

        void ExchangeAccessTokenCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            // Acquire access_token and expires timestamp
            IEnumerable<KeyValuePair<string, string>> pairs = UriToolKits.ParseQueryString(e.Result);
            string accessToken = KeyValuePairUtils.GetValue(pairs, "access_token");

            if (accessToken != null && !accessToken.Equals(""))
            {
                MessageBox.Show("Access Token Exchange Failed");
                return;
            }

            // Save access_token
            FacebookClient.Instance.AccessToken = accessToken;
            //FacebookClient.Instance.PostMessageOnWall("meu nome pirata", new UploadStringCompletedEventHandler(PostMessageOnWallCompleted));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ConnectPage.xaml?", UriKind.Relative));
            botaoFace.IsEnabled = false;
            botaoPostar.IsEnabled = true;

        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sobre.xaml", UriKind.Relative));
        }

        private void Inicio(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


    }
}