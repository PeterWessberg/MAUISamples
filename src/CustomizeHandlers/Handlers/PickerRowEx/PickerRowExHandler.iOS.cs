using CoreFoundation;
using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace CustomizeHandlers.Handlers;

    public partial class PickerRowExHandler : PickerHandler
    {
        protected override void ConnectHandler(MauiPicker picker)
        {
            base.ConnectHandler(picker);

            if (picker is not null)
            {
                var pickerView = picker.UIPickerView;
                pickerView.WeakDelegate = new UIPickerViewDelegateEx(VirtualView);
            }
        }

        protected override void DisconnectHandler(MauiPicker platformView)
        {
            if (platformView.UIPickerView is UIPickerView pickerView)
            {
                pickerView.WeakDelegate = null;
            }

            base.DisconnectHandler(platformView);
        }

        public void SetPadding(Thickness padding)
        {
            var margin = 5;

            if (PlatformView is not UITextField textField) return;

            textField.LeftView = new UIView(new CGRect(0, 0, padding.Left, textField.Frame.Height));
            textField.LeftViewMode = UITextFieldViewMode.Always;

            var fontAwesomeLabel = new UILabel
            {
                Text = "\uf0d7",
                Font = UIFont.FromName("Font Awesome 5 Free", 20),
                TextAlignment = UITextAlignment.Left,
            };
            fontAwesomeLabel.SizeToFit();

            var originalLabelWidth = fontAwesomeLabel.Frame.Width + margin;
            var newContainerWidth = originalLabelWidth + (float)padding.Right;

            var labelYPosition = (textField.Frame.Height - fontAwesomeLabel.Frame.Height) / 2;

            var containerView = new UIView(new CGRect(0, 0, newContainerWidth, textField.Frame.Height));

            fontAwesomeLabel.Frame = new CGRect((float)padding.Right, labelYPosition, originalLabelWidth, fontAwesomeLabel.Frame.Height);

            containerView.AddSubview(fontAwesomeLabel);

            textField.RightView = containerView;
            textField.RightViewMode = UITextFieldViewMode.Always;
        }

        public void SetBorderColor(Color color)
        {
            PlatformView.Layer.CornerRadius = 10;
            PlatformView.Layer.BorderWidth = 1;
            PlatformView.Layer.BorderColor = color.ToCGColor();
        }


    private class UIPickerViewDelegateEx : UIPickerViewDelegate
    {
        private readonly IPicker _picker;
        private readonly UIColor[] colors;

        public UIPickerViewDelegateEx(IPicker picker)
        {
            _picker = picker;
            colors = new UIColor[]
            {
                UIColor.Green,
                UIColor.Purple,
                UIColor.Blue,
                UIColor.Yellow,
                UIColor.Magenta
            };
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            var colorIndex = row % 5;
            var color = colors[colorIndex];

            UILabel label = new UILabel
            {
                Text = _picker.GetItem((int)row),
                TextColor = color,
                BackgroundColor = UIColor.White,
                TextAlignment = UITextAlignment.Center
            };

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                if (pickerView.ViewFor(row, component) is UILabel updatedLabel)
                {
                    updatedLabel.TextColor = UIColor.Red;
                    updatedLabel.BackgroundColor = UIColor.LightGray;
                    updatedLabel.Font = UIFont.BoldSystemFontOfSize(label.Font.PointSize);
                }
            });

            return label;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            _picker.SelectedIndex = (int)row;
        }
    }
}