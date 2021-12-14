using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.Logic
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField]
        private Text hintNumberText;

        void Start()
        {
            hintNumberText.text = GameManager.Instance.getState().nHints.ToString()+" x";
        }

        public void applyHint()
        {
            GameManager.Instance.getState().nHints--;
            int nHints = (int)GameManager.Instance.getState().nHints;
            if (nHints <= 0)
            {
                GameManager.Instance.getState().nHints = 0;
                hintNumberText.text = "+";
            }
            else if (nHints > 0) 
            { 
                hintNumberText.text = GameManager.Instance.getState().nHints.ToString() + " x"; 
            }

           
            //aplicar la pista, llamar aqui a un metodo del board
        }

        public void getHint()
        {
            GameManager.Instance.getState().nHints++;
            hintNumberText.text = GameManager.Instance.getState().nHints.ToString() + " x";
        }
    }
}
