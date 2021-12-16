using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField]
        private Text hintNumberText;

        void Start()
        {
            hintsChanged((int)GameManager.Instance.getState().nHints);
        }

        public void hintsChanged(int nHints)
        {
            if (hintNumberText.IsDestroyed())
                return;
            if (nHints <= 0)
            {
                hintNumberText.text = "+";
            }
            else if (nHints > 0)
            {
                hintNumberText.text = nHints.ToString() + " x";
            }
        }
    }
}
