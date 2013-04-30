using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PiratesName.Resources;
using System.IO.IsolatedStorage;

namespace PiratesName
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();


            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains("nomepirata"))
            {
                settings.Remove("nomepirata");
            }

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ResultadoNome.xaml?nome=" + nomeUsuario.Text, UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sobre.xaml?nome=" + nomeUsuario.Text, UriKind.Relative));
        }
    }
}