using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRUiKits.Utils
{
    public class OptionsManager : MonoBehaviour
    {
        [Header("Template")]
        public GameObject optionTemplate;
        [HideInInspector]
        public List<Option> optionsList = new List<Option>(); // Used to populate the options list
        public delegate void OnOptionSelectedHandler(int index);
        public event OnOptionSelectedHandler OnOptionSelected;
        [HideInInspector]
        public string selectedValue;
        public int firstSelectedIndex = 0;
        // The index of the selected <option> element in the options list (starts at 0)
        int selectedIdx = 0;
        List<OptionItem> optionItems = new List<OptionItem>();

        void Awake()
        {
            optionTemplate.SetActive(false);
            PopulateOptions();
        }

        void Start()
        {
            selectedIdx = Mathf.Clamp(firstSelectedIndex, 0, optionItems.Count - 1);
            ActivateOption(selectedIdx);
            OnOptionSelected += ActivateOption;
        }

        void PopulateOptions()
        {
            Transform _parent = optionTemplate.transform.parent;

            for (int i = 0; i < optionsList.Count; i++)
            {
                OptionItem _item = Instantiate(optionTemplate, _parent).GetComponent<OptionItem>();
                _item.Option = optionsList[i];
                _item.Deactivate();
                optionItems.Add(_item);
            }
        }

        public void PrevOption()
        {
            if (0 == selectedIdx)
            {
                return;
            }

            DeactivateOption(selectedIdx);
            selectedIdx -= 1;
            OnOptionSelected(selectedIdx);
        }

        public void NextOption()
        {
            if (selectedIdx >= optionItems.Count - 1)
            {
                return;
            }
            DeactivateOption(selectedIdx);
            selectedIdx += 1;
            OnOptionSelected(selectedIdx);
        }

        void ActivateOption(int i)
        {
            if (i >= 0 && i < optionItems.Count)
            {
                optionItems[i].Activate();
                // Assign new selected value
                selectedValue = optionItems[i].Value;
            }
        }

        void DeactivateOption(int i)
        {
            if (i >= 0 && i < optionItems.Count)
            {
                optionItems[i].Deactivate();
            }
        }
    }
}