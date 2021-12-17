using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI {
    public class LevelsPanel : MonoBehaviour
    {

        [SerializeField] Text dimensionsText;               //Board dimensions text
        [SerializeField] LevelButton[] lvlButton;           //Level button array
        [SerializeField] HorizontalLayoutGroup hLayout;     //Horizontal layout group of the rows
        [SerializeField] int rows = 6;                      //number of the panel rows
        [SerializeField] int cols = 5;                      //number of the panel colums

        /// <summary>
        /// Sets the board dimensions text
        /// </summary>
        /// <param name="s">Text</param>
        public void setDimensionsText(string s)
        {
            dimensionsText.text = s;
        }

        /// <summary>
        /// Initializes a level button
        /// </summary>
        /// <param name="c">Color</param>
        /// <param name="lvl">Level configuration</param>
        /// <param name="number">Index of the level buttons array</param>
        /// <param name="nPanel">Panel number</param>
        /// <param name="lvlIndex">Index of the levels array</param>
        /// <param name="logicLevel">Level info to be saved and loaded</param>
        public void setlvlButton(Color c, string lvl, int number, int nPanel, int lvlIndex, Logic.Level logicLevel)
        {
            //We initialize the level button color and info
            LevelButton button = lvlButton[number];

            button.setButtonLevelColor(c);
            button.setLevelInfo(lvl, nPanel, lvlIndex);

            //We enable the lock image if the level is locked
            if (logicLevel.locked)
            {
                button.enableLock(true);
            }
            else if(logicLevel.completed) //If the level is completed
            {
                //If it is done perfectly we enable the start image, in other case we enable the check image
                string[] header = lvl.ToString().Split(';')[0].Split(',');
                int nPipes = int.Parse(header[3]);
                if (logicLevel.record == nPipes) button.enableStar(true);
                else button.enableCheck(true);
            }
        }

        /// <summary>
        /// Returns the width of the panel
        /// </summary>
        /// <returns></returns>
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