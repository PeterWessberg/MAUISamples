using CoreFoundation;
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