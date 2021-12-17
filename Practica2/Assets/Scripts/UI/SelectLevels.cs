using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{
    public class SelectLevels : MonoBehaviour
    {
        [SerializeField] Text categoryText;         //The text that shows the name of the pack category

        [SerializeField] Image check;               //The check image
        [SerializeField] Image star;                //The star image

        [SerializeField] LevelsPanel levelsPanel;   //Levels panel object

        [SerializeField] ScrollRect scroll;         //Scroll rect component

        PackCategory category;                      //The selected pack category
        LevelPack pack;                             //Tthe selected level pack 

        HorizontalLayoutGroup hLayout;              //The HorizontalLayoutGroup attached to this gameobject
        RectTransform rectTr;                       //The RectTransform attached to this gameobject

        private const int panelSize = 30;           //Number of panel buttons

        private void Awake()
        {
            hLayout = GetComponent<HorizontalLayoutGroup>();
            rectTr = GetComponent<RectTransform>();
        }

        void Start()
        {
            //We get the selected LevelPack and the PackCategory
            category = GameManager.Instance.getPackCategory();
            pack = GameManager.Instance.getLevelPack();

            //We set the category text an the check image
            categoryText.text = pack.packName;
            categoryText.color = category.categoryColor;
            check.color = category.categoryColor;

            //We get the Category and LevelPack containers that store information
            Logic.Category logicCategory = GameManager.Instance.getState().getCategoryByName(category.categoryName);
            Logic.LvlPack logicPack = logicCategory.getPackByName(pack.packName);
            //We get some information of the LevelPack container
            int completedLevels = logicPack.completedLevels;
            int perfectLevels = logicPack.perfectLevels;
            int nLeves = logicPack.levels.Length;
            //If we have completed all levels perfectly we enable the star image
            if (completedLevels == nLeves && perfectLevels == nLeves)
                star.enabled = true;
            else if (completedLevels == nLeves) //If we have completed all levels we enable the check image
                check.enabled = true;

            //We get the Level container array of the LevelOck container
            Logic.Level[] logicLevels = logicPack.levels;

            //We get the levels array
            string[] levels = GameManager.Instance.getLevelPack().levels.ToString().Split('\n');

            float totalWidth = 0; //Total width of the RectTransform

            //We iterate through the panels
            for (int i = 0; i < (levels.Length - 1) / panelSize; i++)
            {
                //We instantiate the level panel
                LevelsPanel lvlPanel = Instantiate(levelsPanel, transform);
                //We add the width of each level panel to the total width
                totalWidth += lvlPanel.getWidth() + hLayout.spacing;
                //We set the dimensions text
                lvlPanel.setDimensionsText(pack.levelPanelName[i]);

                //We iterate through the levels of the panel
                for (int j = 0; j < panelSize; j++)
                {
                    //We initialze the levelButton info
                    int nLevel = i * panelSize + j;
                    lvlPanel.setlvlButton(pack.levelPanelColors[i], levels[nLevel], j, nLevel, logicLevels[nLevel]);
                }
            }

            //We set the width of the RectTrasform
            rectTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth + hLayout.spacing);
        }

        //This callback goes to the ChoosePack scene
        public void backClickCallback()
        {
            GameManager.Instance.gotoSelectCategoryMenu();
        }
    }
}
