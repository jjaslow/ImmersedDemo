using UnityEngine;
using UnityEngine.UI;

namespace VRUiKits.Utils
{
    public class MenuItem : Item
    {
        public GameObject subMenu;

        public override void Activate()
        {
            base.Activate();

            if (null != subMenu)
            {
                subMenu.SetActive(true);
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            DeactivateSubMenu();
        }

        public override void DeactivateSubMenu()
        {
            if (null != subMenu)
            {
                subMenu.SetActive(false);
            }
        }
    }
}