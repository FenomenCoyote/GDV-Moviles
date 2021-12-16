using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{
    public class EndImage : MonoBehaviour
    {
        [SerializeField]
        private Image checkImg;

        [SerializeField]
        private Image starImg;

        /// <summary>
        /// Enables or disables the check image 
        /// </summary>
        /// <param name="enable">if the image is enabled or not</param>
        public void enableCheck(bool enable)
        {
            checkImg.enabled = enable;
        }

        /// <summary>
        /// Enables or disables the start image 
        /// </summary>
        /// <param name="enable">if the image is enabled or not</param>
        public void enableStar(bool enable)
        {
            starImg.enabled = enable;
        }
    }
}
