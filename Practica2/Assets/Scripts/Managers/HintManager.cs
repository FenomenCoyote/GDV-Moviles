using UnityEngine;
using UnityEngine.UI;

namespace flow
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField]
        private Text hintNumberText;    //The text that shows the number of hints

        void Start()
        {
            //We set the hintNumberText
            hintsChanged((int)GameManager.Instance.getState().nHints);
        }

        /// <summary>
        /// Sets the hintNumberText
        /// If we have <= 0 hints, we set the hintNumberText to "+"
        /// else if we have > 0 hints, we set the hintNumberText to the current hints
        /// </summary>
        /// <param name="nHints">Current hints</param>
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
