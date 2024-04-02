using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;
using AResource = Android.Resource;
using View = Android.Views.View;
using Color = Microsoft.Maui.Graphics.Color;

namespace CustomizeHandlers.Handlers;

public partial class PickerRowExHandler : PickerHandler
{
    private AppCompatAlertDialog dialog;

    protected override void ConnectHandler(MauiPicker platformView)
    {
        platformView.FocusChange += OnFocusChange;
        platformView.Click += OnClick;
    }

    protected override void DisconnectHandler(MauiPicker platformView)
    {
        platformView.FocusChange -= OnFocusChange;
        platformView.Click -= OnClick;
        base.DisconnectHandler(platformView);
    }

    public void SetPadding(Thickness padding)
    {
    }
    public void SetBorderColor(Color color)
    {
        
    }

    /// <summary>
    /// Source code from PickerHandler. Added an adapter for controlling the rows.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnClick(object sender, EventArgs e)
    {
        if (dialog == null && VirtualView != null)
        {
            using (var builder = new AppCompatAlertDialog.Builder(Context))
            {
                if (VirtualView.TitleColor == null)
                {
                    builder.SetTitle(VirtualView.Title ?? string.Empty);
                }
                else
                {
                    var title = new SpannableString(VirtualView.Title ?? string.Empty);
                    title.SetSpan(new ForegroundColorSpan(VirtualView.TitleColor.ToPlatform()), 0, title.Length(), SpanTypes.ExclusiveExclusive);
                    builder.SetTitle(title);
                }

                string[] items = VirtualView.GetItemsAsArray();

                for (var i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    if (item == null)
                        items[i] = string.Empty;
                }

                var adapter = new ArrayAdapterEx(Context, Android.Resource.Layout.SimpleListItem1, items, VirtualView);

                dialog = builder.SetAdapter(adapter, (s, e) =>
                {
                    var selectedIndex = e.Which;
                    VirtualView.SelectedIndex = selectedIndex;
                    base.PlatformView.Hint = VirtualView.Title;
                    if (VirtualView.SelectedIndex == -1 || VirtualView.SelectedIndex >= VirtualView.GetCount())
                        base.PlatformView.Text = null;
                    else
                        base.PlatformView.Text = VirtualView.GetItem(VirtualView.SelectedIndex);
                })
                .SetNegativeButton(AResource.String.Cancel, (o, args) => { })
                .Create();
            }

            if (dialog == null)
                return;

            dialog.SetCanceledOnTouchOutside(true);

            dialog.DismissEvent += (sender, args) =>
            {
                dialog = null;
            };

            dialog.Show();
        }
    }

    private void OnFocusChange(object sender, global::Android.Views.View.FocusChangeEventArgs e)
    {
        if (PlatformView == null)
            return;

        if (e.HasFocus)
        {
            if (PlatformView.Clickable)
                PlatformView.CallOnClick();
            else
                OnClick(PlatformView, EventArgs.Empty);
        }
        else if (dialog != null)
        {
            dialog.Hide();
            dialog = null;
        }
    }

    private class ArrayAdapterEx : ArrayAdapter<string>
    {
        private readonly IPicker virtualView;
        private readonly Android.Graphics.Color[] colors;

        public ArrayAdapterEx(Context context, int textViewResourceId, string[] objects, IPicker virtualView) : base(context, textViewResourceId, objects)
        {
            this.virtualView = virtualView;
            colors = new Android.Graphics.Color[]
            {
                Android.Graphics.Color.Green,
                Android.Graphics.Color.Purple,
                Android.Graphics.Color.Blue,
                Android.Graphics.Color.Yellow,
                Android.Graphics.Color.Magenta
            };
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = base.GetView(position, convertView, parent);
            TextView textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);

            var colorIndex = position % 5;
            var color = colors[colorIndex];

            var item = GetItem(position); // Exampel of how to get current item
            if (position == virtualView.SelectedIndex)
            {
                textView.SetTextColor(Colors.Red.ToPlatform());
                textView.SetBackgroundColor(Colors.LightGray.ToPlatform());
                textView.Typeface = Typeface.DefaultBold;
            }
            else
            {
                textView.SetTextColor(color);
                textView.SetBackgroundColor(Colors.White.ToPlatform());
            }

            textView.Gravity = Android.Views.GravityFlags.Center;

            return view;
        }
    }
}