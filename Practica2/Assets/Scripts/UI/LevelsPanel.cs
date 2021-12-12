using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI {
    public class LevelsPanel : MonoBehaviour
    {

        [SerializeField] Text dimensionsText;
        [SerializeField] LevelButton[] lvlButton;
        [SerializeField] int rows = 6;
        [SerializeField] int cols = 5;
        [SerializeField] HorizontalLayoutGroup hLayout;

        public void setDimensionsText(string s)
        {
            dimensionsText.text = s;
        }
       
        public void setlvlButton(Color c, string lvl, int number, int nPanel,int lvlIndex)
        {
            lvlButton[number].setButtonLevelColor(c);
            lvlButton[number].setLevelInfo(lvl, nPanel,lvlIndex);
        }

        public float getWidth() 
        {
            float aux = 0;
            for(int i=0;i<cols;i++)
            {
                aux += lvlButton[0].getWidth() + hLayout.spacing;
            }
            return aux;
        }
    }
}