using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRUiKits.Utils
{
    public class ListSelectionHelper : MonoBehaviour
    {
        public Transform list;
        public Item initialActivatedItem;
        [HideInInspector]
        public Item currentSelectedItem;
        Item[] items;

        void Awake()
        {
            items = list.GetComponentsInChildren<Item>();
        }

        void Start()
        {
            foreach (var item in items)
            {
                item.OnItemSelected += SelectionHelper;
                if (null != initialActivatedItem)
                {
                    initialActivatedItem.button.onClick.Invoke();
                    currentSelectedItem = initialActivatedItem;
                }
            }
        }

        void OnDisable()
        {
            foreach (var item in items)
            {
                if (item is MenuItem)
                {
                    item.DeactivateSubMenu();
                }
            }

            if (currentSelectedItem is MenuItem)
            {
                DeselectCurrentItem();
            }
        }

        void SelectionHelper(Item item)
        {
            DeselectCurrentItem();

            item.Activate();
            currentSelectedItem = item;
        }

        public void DeselectCurrentItem()
        {
            if (null != currentSelectedItem)
            {
                currentSelectedItem.Deactivate();
            }
            currentSelectedItem = null;
        }
    }
}