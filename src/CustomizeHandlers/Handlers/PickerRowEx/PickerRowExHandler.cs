using CustomizeHandlers.Controls;
using Microsoft.Maui.Handlers;

namespace CustomizeHandlers.Handlers;

public partial class PickerRowExHandler
{
    public static new PropertyMapper<IPicker, PickerHandler> Mapper = new PropertyMapper<IPicker, PickerHandler>(PickerHandler.Mapper)
    {
        ["Padding"] = MapPadding,
        ["BorderColor"] = MapBorderColor,
    };
    public PickerRowExHandler() : base(Mapper)
    {
    }

    public PickerRowExHandler(PropertyMapper mapper) : base(mapper)
    {
    }

    public override void UpdateValue(string propertyName)
    {
        base.UpdateValue(propertyName);
        if (propertyName == PickerRowEx.PaddingProperty.PropertyName)
        {
            if(VirtualView is PickerRowEx pickerRowEx)
            {
                SetPadding(pickerRowEx.Padding);
            }
        }

        if (propertyName == PickerRowEx.BorderColorProperty.PropertyName)
        {
            if (VirtualView is PickerRowEx pickerRowEx)
            {
                SetBorderColor(pickerRowEx.BorderColor);
            }
        }
    }

    public static void MapPadding(PickerHandler handler, IPicker picker)
    {
        if (handler is PickerRowExHandler pickerHandler && pickerHandler.VirtualView is PickerRowEx pickerRowEx)
        {
            pickerHandler.SetPadding(pickerRowEx.Padding);
        }
    }

    public static void MapBorderColor(PickerHandler handler, IPicker picker)
    {
        if (handler is PickerRowExHandler pickerHandler && pickerHandler.VirtualView is PickerRowEx pickerRowEx)
        {
            pickerHandler.SetBorderColor(pickerRowEx.BorderColor);
        }
    }
}
