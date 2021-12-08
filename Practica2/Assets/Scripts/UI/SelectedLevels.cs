using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI { 

    public class SelectedLevels : MonoBehaviour
    {

        [SerializeField] Text categoryText;
        [SerializeField] Image check;
        [SerializeField] Button backButton;

        [SerializeField] LevelsPanel levelsPanel;

        PackCategory category;
        LevelPack pack;

        HorizontalLayoutGroup hLayout;
        RectTransform rectTr;

        private void Awake()
        {
            hLayout = GetComponent<HorizontalLayoutGroup>();
            rectTr = GetComponent<RectTransform>();
        }


        void Start()
        {
            backButton.onClick.AddListener(delegate () { backClickCallback(); });

            category = GameManager.Instance.getPackCategory();
            pack = GameManager.Instance.getLevelPack();

            categoryText.text = category.categoryName;
            categoryText.color = category.categoryColor;
            check.color = category.categoryColor;

            string[] levels = GameManager.Instance.getLevelPack().levels.ToString().Split('\n');

            float auxWidth = 0;
            for (int i=0;i<levels.Length/30;i++)
            {
                LevelsPanel lvlPanel = Instantiate(levelsPanel, transform);

                auxWidth += lvlPanel.getWidth() + hLayout.spacing;

                lvlPanel.setDimensionsText(pack.levelPanelName[i]);
                for(int j=0;j<30;j++)
                {
                    string[] aux = levels[i * 30 + j].Split(',');
                    lvlPanel.setlvlButton(pack.levelPanelColors[i], aux[2], j);
                }
            }

            rectTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, auxWidth);
        }

        void backClickCallback()
        {
            GameManager.Instance.gotoSelectCategoryMenu();
        }

    }
}
