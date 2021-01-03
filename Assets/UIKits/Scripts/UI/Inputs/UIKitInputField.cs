using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace VRUiKits.Utils
{
    public class UIKitInputField : MonoBehaviour, ISelectHandler, IPointerClickHandler, IDeselectHandler
    {
        public enum ContentType
        {
            Standard,
            Password
        }

        public RectTransform wrapper;
        public Text textComponent;
        public string text;
        public int characterLimit = 0;
        public ContentType contentType;
        public Text placeholder;
        public RectTransform caretTransform;
        [Range(0f, 4f)]
        public float caretBlinkRate = 0.85f;
        private float maxW_textComponent;
        private string displayedText;
        private string prevText = "";
        private Text caretText;

        void Awake()
        {
            // 5.0f is the min width set to caret.
            maxW_textComponent = wrapper.rect.width - 5.0f;
            caretText = caretTransform.GetComponent<Text>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            /*
            Set keyboard target explicitly for some 3rd party packages which lost focus when
            user click on keyboard.
            */
            MobileKeyboardManager.Target = this;
            caretTransform.gameObject.SetActive(true);
            StartCoroutine("BlinkCaret");
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            caretTransform.gameObject.SetActive(false);
            StopCoroutine("BlinkCaret");
        }

        // Trick: Set caret text to empty to update the next caret position
        public void ForceCaretUpdate()
        {
            caretText.text = "";
        }

        void LateUpdate()
        {
            // Limit the character length
            if (characterLimit != 0 && text.Length > characterLimit)
            {
                text = text.Substring(0, characterLimit);
            }

            // Check if character is empty
            if (text == "")
            {
                placeholder.gameObject.SetActive(true);
            }
            else
            {
                placeholder.gameObject.SetActive(false);
            }

            if (prevText == text)
            {
                return;
            }

            if (contentType == ContentType.Standard)
            {
                CalculateLengthOfText(text);
            }
            else if (contentType == ContentType.Password)
            {
                displayedText = new string('*', text.Length);
            }

            // Update text field to displayed text
            textComponent.text = displayedText;
            prevText = text;
        }


        void CalculateLengthOfText(string text)
        {
            int totalWidth = 0;
            displayedText = "";

            if (text != "")
            {
                Font font = textComponent.font;
                CharacterInfo characterInfo = new CharacterInfo();
                font.RequestCharactersInTexture(text, textComponent.fontSize, textComponent.fontStyle);

                char[] arr = text.ToCharArray();
                for (int i = text.Length - 1; i >= 0; i--)
                {
                    font.GetCharacterInfo(arr[i], out characterInfo, textComponent.fontSize, textComponent.fontStyle);

                    totalWidth += characterInfo.advance;
                    if (totalWidth <= maxW_textComponent)
                    {
                        displayedText = arr[i] + displayedText;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        IEnumerator BlinkCaret()
        {
            while (true)
            {
                yield return new WaitForSeconds(caretBlinkRate);
                if (caretText.text == "")
                {
                    caretText.text = "|";
                }
                else
                {
                    caretText.text = "";
                }
            }
        }
    }
}