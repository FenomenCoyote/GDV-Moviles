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
       
        public void setlvlButton(Color c, string lvl, int number, int nPanel, Logic.Level logicLevel)
        {
            LevelButton button = lvlButton[number];

            button.setButtonLevelColor(c);
            button.setLevelInfo(lvl, nPanel);

            if (logicLevel.locked)
            {
                button.enableLock(true);
            }
            else if(logicLevel.completed)
            {
                int nPipes = int.Parse(lvl.ToString().Split(',')[3]);
                if (logicLevel.record == nPipes) button.enableStar(true);
                else button.enableCheck(true);
            }
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