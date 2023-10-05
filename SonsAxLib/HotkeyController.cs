using Sons.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Items.Inventory;
using TheForest.Utils;

namespace SonsAxLib;

public class HotkeyController
{
    static ItemHotkeyController _itemHotkeyController;

    public static ItemHotkeyController ItemHotkeyController
    {
        get
        {
            _itemHotkeyController ??= GetHotkeyController();
            return _itemHotkeyController;
        }
    }

    internal static ItemHotkeyController GetHotkeyController()
    {
        return LocalPlayer.Transform.Find("ControllerObjects/ItemHotkeyController").GetComponent<ItemHotkeyController>();
    }
}
