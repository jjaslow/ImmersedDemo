/***
 * Author: Yunhan Li
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace VRUiKits.Utils
{
    public class LaserPointer : MonoBehaviour
    {
        LineRenderer lr;

        #region MonoBehaviour Callbacks
        void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }

        void LateUpdate()
        {
            lr.SetPosition(0, transform.position + (transform.forward * .05f));
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position, fwd, out hit))
            {
                if (hit.collider)
                {
                    lr.SetPosition(1, hit.point);
                }
            }
            else
            {
                lr.SetPosition(1, transform.position + (transform.forward * 5));
            }
        }

        void OnDisable()
        {
            if (null != lr)
            {
                // Reset position for smooth transtation when enbale laser pointer
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, Vector3.zero);
            }
        }
        #endregion
    }
}
