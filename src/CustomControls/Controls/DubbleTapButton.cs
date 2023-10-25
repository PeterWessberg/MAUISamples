using CustomControls.Resources.Fonts;
using System.Windows.Input;

namespace CustomControls.Controls
{
    public class DubbleTapButton : Button
    {
        private bool _canExecutCommand = false;
        private ICommand _toggleCommand;
        private string _fontFamily;
        private int width = 30;

        public new event EventHandler Clicked;

        public ICommand ToogleCommand =>
                _toggleCommand ??= new Command(() =>
                {
                    if (!_canExecutCommand)
                    {
                        TextColor = Colors.Transparent;
                        Text = "Clear";
                        new Animation(v => WidthRequest = v, width, 70).Commit(this, "Animate", 16, 200, Easing.SinOut, finished: (v, c) =>
                        {
                            FontFamily = _fontFamily;
                            TextColor = Colors.Black;
                            FontSize = 14;
                            CornerRadius = 18;
                            _canExecutCommand = true;
                        });
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
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ToogleCommand
            });

            _fontFamily = FontFamily;
            CornerRadius = 18;
            Text = FontAwesomeIcons.Xmark;
            Padding = 0;
            FontSize = 16;
            TextColor = Colors.Black;
            WidthRequest = width;
            HeightRequest = width;
            BackgroundColor = Colors.LightGray;
            FontFamily = "FontAwesome-Solid";
        }

        private void ReSet()
        {
            TextColor = Colors.Transparent;
            Text = FontAwesomeIcons.Xmark;
            new Animation(v => WidthRequest = v, 70, width).Commit(this, "AnimateReset", 16, 200, Easing.SinOut, finished: (v, c) =>
            {
                FontFamily = "FontAwesome-Solid";
                TextColor = Colors.Black;
                CornerRadius = 18;
                Padding = 0;
                FontSize = 16;
                _canExecutCommand = false;
            });
        }
    }
}