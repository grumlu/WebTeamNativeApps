﻿using System;
using WebTeamWindows.Resources;
using WebTeamWindows.ViewModel;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebTeamWindows.View
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
            DataContext = new LoginViewModel();
        }


        #region Boutons
        private async void Connexion_Click(object sender, RoutedEventArgs e)
        {
            progressRingWebTeam.IsActive = true;

            try
            {
                ERROR err = await APIWebTeam.CheckTokenAsync();

                if (err != ERROR.NO_ERR)
                {

                    progressRingWebTeam.IsActive = false;
                    return;
                }

                Utilisateur appUser = await APIWebTeam.GetUserAsync();

                this.Frame.Navigate(typeof(WebTeamView), appUser);
            }
            catch (Exception excep)
            {
                string errMsg = "Une erreur est survenue :\n" + excep.StackTrace.ToString();
                MessageDialog dialog = new MessageDialog(errMsg);
                dialog.ShowAsync();
                progressRingWebTeam.IsActive = false;
            }

        }
        #endregion


        private void SettingsAbout_ApplicationBarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void changeUser_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).Disconnect();

            //Connexion_Click(null, null);
        }

    }


}
