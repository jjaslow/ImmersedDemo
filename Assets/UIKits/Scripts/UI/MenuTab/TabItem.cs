using UnityEngine;
using UnityEngine.UI;

namespace VRUiKits.Utils
{
    public class TabItem : Item
    {
        public GameObject relatedPanel;

        public override void Activate()
        {
            base.Activate();

            if (null != relatedPanel)
            {
                if (!relatedPanel.activeSelf)
                {
                    relatedPanel.SetActive(true);
                }

                Canvas canvas = relatedPanel.GetComponent<Canvas>();
                if (null != canvas)
                {
                    canvas.enabled = true;
                }
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            if (null != relatedPanel)
            {
                Canvas canvas = relatedPanel.GetComponent<Canvas>();

                if (null != canvas)
                {
                    canvas.enabled = false;
                }
                else
                {
                    relatedPanel.SetActive(false);
                }
            }
        }
    }
}