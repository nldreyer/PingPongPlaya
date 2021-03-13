

using Microsoft.Xna.Framework.Audio;

namespace PingPongPlaya.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {

        private readonly MenuEntry _volumeMenuEntry;

        private static int _volume = 100;

        public OptionsMenuScreen() : base("Options")
        {
            _volumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _volumeMenuEntry.Selected += VolumeMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(_volumeMenuEntry);
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _volumeMenuEntry.Text = $"Volume: {_volume.ToString()}";
        }

        private void VolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_volume == 100) _volume = 0;
            else _volume += 10;
            SoundEffect.MasterVolume = (float)(_volume * .01);
            SetMenuEntryText();
        }
    }
}
