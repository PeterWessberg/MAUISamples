using CustomControls.Resources.Fonts;
using System.Windows.Input;

namespace CustomControls.Controls
{
    public class DubbleTapButton : Button
    {
        private bool _canExecutCommand = false;
        private ICommand _toggleCommand;
        private string _fontFamily;

        public new event EventHandler Clicked;

        public ICommand ToogleCommand =>
                _toggleCommand ??= new Command(() =>
                {
                    if (!_canExecutCommand)
                    {
                        new Animation(v => WidthRequest = v, 30, 70).Commit(this, "Animate", 16, 300, Easing.SinOut, finished: (v, c) =>
                        {
                            FontFamily = _fontFamily;
                            FontSize = 14;
                            CornerRadius = 20;
                            Text = "Clear";
                        });

                        _canExecutCommand = true;
                    }
                    else
                    {
                        if (Command != null)
                        {
                            if (Command.CanExecute(CommandParameter))
                            {
                                Command.Execute(CommandParameter);
                            }
                        }

                        Clicked?.Invoke(this, EventArgs.Empty);

                        ReSet();
                    }
                });

        public DubbleTapButton()
        {
            _fontFamily = FontFamily;

            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ToogleCommand
            });

            CornerRadius = 20;
            FontFamily = "FontAwesome-Solid";
            Text = FontAwesomeIcons.Xmark;
            Padding = 0;
            FontSize = 16;
            TextColor = Colors.Black;
            WidthRequest = 30;
            HeightRequest = 30;
            BackgroundColor = Colors.LightGray;
        }

        private void ReSet()
        {
            new Animation(v => WidthRequest = v, 70, 30).Commit(this, "Animate", 16, 300, Easing.SinOut, finished: (v, c) =>
            {
                CornerRadius = 20;
                FontFamily = "FontAwesome-Solid";
                Text = FontAwesomeIcons.Xmark;
                Padding = 0;
                FontSize = 16;
                TextColor = Colors.Black;
            });

            _canExecutCommand = false;
        }
    }
}