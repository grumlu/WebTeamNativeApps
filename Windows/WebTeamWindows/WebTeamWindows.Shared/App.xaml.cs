using System;
using WebTeamWindows.Common;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#if WINDOWS_PHONE_APP
using WebTeamWindows.Resources;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Navigation;
#endif
// Pour plus d'informations sur le modèle Application vide, consultez la page http://go.microsoft.com/fwlink/?LinkId=234227

namespace WebTeamWindows
{

	/// <summary>
	/// Fournit un comportement spécifique à l'application afin de compléter la classe Application par défaut.
	/// </summary>
	public sealed partial class App : Application
	{
#if WINDOWS_PHONE_APP
		public static ContinuationManager continuationManager { get; private set; }
#endif
		public static Frame rootFrame { get; set; }

		/// <summary>
		/// Initialise l'objet d'application de singleton.  Il s'agit de la première ligne du code créé
		/// à être exécutée. Elle correspond donc à l'équivalent logique de main() ou WinMain().
		/// </summary>
		public App()
		{
#if WINDOWS_PHONE_APP
			if(continuationManager == null)
				continuationManager = new ContinuationManager();
#endif
			this.InitializeComponent();
			this.Suspending += this.OnSuspending;
		}

		/// <summary>
		/// Invoqué lorsque l'application est lancée normalement par l'utilisateur final.  D'autres points d'entrée
		/// sont utilisés lorsque l'application est lancée pour ouvrir un fichier spécifique, pour afficher
		/// des résultats de recherche, etc.
		/// </summary>
		/// <param name="e">Détails concernant la requête et le processus de lancement.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			rootFrame = Window.Current.Content as Frame;

			// Ne répétez pas l'initialisation de l'application lorsque la fenêtre comporte déjà du contenu,
			// assurez-vous juste que la fenêtre est active
			if (rootFrame == null)
			{
				// Créez un Frame utilisable comme contexte de navigation et naviguez jusqu'à la première page
				rootFrame = new Frame();

				// TODO: modifier cette valeur à une taille de cache qui contient à votre application
				rootFrame.CacheSize = 1;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: chargez l'état de l'application précédemment suspendue
				}

				// Placez le frame dans la fenêtre active
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// Quand la pile de navigation n'est pas restaurée, accédez à la première page,
				// puis configurez la nouvelle page en transmettant les informations requises en tant que
				// paramètre
				if (!rootFrame.Navigate(typeof(View.LoginView), e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}

			// Vérifiez que la fenêtre actuelle est active
			Window.Current.Activate();
		}


		/// <summary>
		/// Appelé lorsque l'exécution de l'application est suspendue.  L'état de l'application est enregistré
		/// sans savoir si l'application pourra se fermer ou reprendre sans endommager
		/// le contenu de la mémoire.
		/// </summary>
		/// <param name="sender">Source de la requête de suspension.</param>
		/// <param name="e">Détails de la requête de suspension.</param>
		private async void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			
			await SuspensionManager.SaveAsync();
#if WINDOWS_PHONE_APP
			continuationManager.MarkAsStale();
#endif
			deferral.Complete();
		}

#if WINDOWS_PHONE_APP
		protected async override void OnActivated(IActivatedEventArgs e)
		{
			CreateRootFrame();

			// Restore the saved session state only when appropriate

			if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)

			{
				try
				{
					await SuspensionManager.RestoreAsync();
				}
				catch (SuspensionManagerException)
				{					//Something went wrong restoring state.
									//Assume there is no state and continue
				}
			}
			//Check if this is a continuation
			var continuationEventArgs = e as IContinuationActivatedEventArgs;
			if (continuationEventArgs != null)
			{
				continuationManager.Continue(continuationEventArgs);
			}
			continuationManager.MarkAsStale();
			Window.Current.Activate();
		}

		private void CreateRootFrame()
		{
			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame != null)
				return;

			// Create a Frame to act as the navigation context and navigate to the first page
			rootFrame = new Frame();

			//Associate the frame with a SuspensionManager key                                
			SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

			// Set the default language
			rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

			rootFrame.NavigationFailed += OnNavigationFailed;

			// Place the frame in the current Window
			Window.Current.Content = rootFrame;
		}

		private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			Frame frame = Window.Current.Content as Frame;
			if (frame == null)
			{
				return;
			}

			if (frame.CanGoBack)
			{
				frame.GoBack();
				e.Handled = true;
			}
		}

		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{

			//change this code for your needs
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}
#endif
	}
}