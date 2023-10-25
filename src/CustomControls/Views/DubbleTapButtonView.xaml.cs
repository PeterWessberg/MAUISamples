using CommunityToolkit.Maui.Alerts;
using CustomControls.ViewModel;
using System.Collections.ObjectModel;

namespace CustomControls.Views;

public partial class DubbleTapButtonView : ContentPage
{
    public DubbleTapButtonView()
    {
        InitializeComponent();
        BindingContext = new DoubleTapViewModel();
    }
}