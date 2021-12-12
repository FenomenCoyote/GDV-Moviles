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

        public void enableCheck(bool enable)
        {
            checkImg.enabled = enable;
        }

        public void enableStar(bool enable)
        {
            starImg.enabled = enable;
        }
    }
}
