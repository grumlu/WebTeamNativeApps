using BackgroundAudioShared;
using BackgroundAudioShared.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Resources;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

namespace WebTeamWindows10Universal.Model
{
    class FUSEModel
    {
        private AutoResetEvent backgroundAudioTaskStarted;
        private bool isMyBackgroundTaskRunning = false;

        /// <summary>
        /// Gets the information about background task is running or not by reading the setting saved by background task.
        /// This is used to determine when to start the task and also when to avoid sending messages.
        /// </summary>
        private bool IsMyBackgroundTaskRunning
        {
            get
            {
                if (isMyBackgroundTaskRunning)
                    return true;

                string value = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.BackgroundTaskState) as string;
                if (value == null)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        isMyBackgroundTaskRunning = EnumHelper.Parse<BackgroundTaskState>(value) == BackgroundTaskState.Running;
                    }
                    catch (ArgumentException)
                    {
                        isMyBackgroundTaskRunning = false;
                    }
                    return isMyBackgroundTaskRunning;
                }
            }
        }
        
        private static string fuseUrl ="http://streaming.radionomy.com/RadioFuse";

        public FUSEModel()
        {
            // Setup the initialization lock
            backgroundAudioTaskStarted = new AutoResetEvent(false);
        }

        public void setMediaElement(MediaElement meElement)
        {
        }

        #region Buttons commands
        /// <summary>
        /// User clicked on the play/pause button
        /// </summary>
        public void Play()
        {
            Debug.WriteLine("Play button pressed from App");

            StartBackgroundAudioTask();
        }

        public void Pause()
        {
        }

        public void Stop()
        {
        }
        #endregion

        #region Background MediaPlayer Event handlers
        /// <summary>
        /// MediaPlayer state changed event handlers. 
        /// Note that we can subscribe to events even if Media Player is playing media in background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            var currentState = sender.CurrentState; // cache outside of completion or you might get a different value
            //await uiFactory.StartNew(() =>
            //{
            //    //Il est possible ici de changer quelque chose dans l'interface si on passe de lecture à pause par ex
            //    //// Update state label
            //    //txtCurrentState.Text = currentState.ToString();

            //    //// Update controls
            //    //UpdateTransportControls(currentState);
            //});
        }

        /// <summary>
        /// This event is raised when a message is recieved from BackgroundAudioTask
        /// </summary>
        void BackgroundMediaPlayer_MessageReceivedFromBackground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            BackgroundAudioTaskStartedMessage backgroundAudioTaskStartedMessage;
            if (MessageService.TryParseMessage(e.Data, out backgroundAudioTaskStartedMessage))
            {
                // StartBackgroundAudioTask is waiting for this signal to know when the task is up and running
                // and ready to receive messages
                Debug.WriteLine("BackgroundAudioTask started");
                backgroundAudioTaskStarted.Set();
                return;
            }
        }
        #endregion

        #region Media Playback Helper Methods
        /// <summary>
        /// Subscribes to MediaPlayer events
        /// </summary>
        private void AddMediaPlayerEventHandlers()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged += this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground += this.BackgroundMediaPlayer_MessageReceivedFromBackground;
        }

        /// <summary>
        /// Initialize Background Media Player Handlers and starts playback
        /// </summary>
        private void StartBackgroundAudioTask()
        {
            AddMediaPlayerEventHandlers();

            var startResult = DispatchService.Invoke(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                bool result = backgroundAudioTaskStarted.WaitOne(10000);
                //Send message to initiate playback
                if (result == true)
                {
                    MessageService.SendMessageToBackground(new UpdateStreamURLMessage(fuseUrl));
                    MessageService.SendMessageToBackground(new StartPlaybackMessage());
                }
                else
                {
                    throw new Exception("Background Audio Task didn't start in expected time");
                }
            });
        }
        #endregion
    }
}
