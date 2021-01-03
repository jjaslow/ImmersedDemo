using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace VRUiKits.Utils
{
    public class ImageHoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image targetImage;
        public Color outlineColor = Color.black;
        public float outlineWidth = 1f;
        UnityEngine.UI.Outline outline;
        // Lazy Evaluation
        UnityEngine.UI.Outline _Outline
        {
            get
            {
                if (null == outline)
                {
                    if (null == targetImage)
                    {
                        outline = null;
                        return outline;
                    }

                    if (null == targetImage.GetComponent<UnityEngine.UI.Outline>())
                    {
                        targetImage.gameObject.AddComponent<UnityEngine.UI.Outline>();
                    }

                    outline = targetImage.GetComponent<UnityEngine.UI.Outline>();
                    outline.effectColor = outlineColor;
                    outline.effectDistance = new Vector2(outlineWidth, -outlineWidth);
                    outline.enabled = false;
                }

                return outline;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (null != _Outline)
            {
                _Outline.enabled = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (null != _Outline)
            {
                _Outline.enabled = false;
            }
        }
    }
}