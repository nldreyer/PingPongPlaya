using Microsoft.Xna.Framework;
using PingPongPlaya.StateManagement;
using System;

namespace PingPongPlaya.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class LostMenuScreen : MenuScreen
    {
        private TimeSpan highScoreTime;

        public LostMenuScreen(TimeSpan highScoreTime) : base("You lost!")
        {
            this.highScoreTime = highScoreTime;
            var playGameMenuEntry = new MenuEntry("Play again");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(highScoreTime));
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
