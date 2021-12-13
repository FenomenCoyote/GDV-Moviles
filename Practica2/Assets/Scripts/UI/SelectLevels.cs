using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{

    public class SelectLevels : MonoBehaviour
    {

        [SerializeField] Text categoryText;
        [SerializeField] Button backButton;

        [SerializeField] Image check;
        [SerializeField] Image star;

        [SerializeField] LevelsPanel levelsPanel;

        [SerializeField] ScrollRect scroll;

        PackCategory category;
        LevelPack pack;

        HorizontalLayoutGroup hLayout;
        RectTransform rectTr;

        private const int panelSize = 30;

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

            //Texto nombre categor�a
            categoryText.text = pack.packName;
            categoryText.color = category.categoryColor;
            check.color = category.categoryColor;

            Logic.Category logicCategory = GameManager.Instance.getState().getCategoryByName(category.categoryName);
            Logic.LvlPack logicPack = logicCategory.getPackByName(pack.packName);
            int completedLevels = logicPack.completedLevels;
            int perfectLevels = logicPack.perfectLevels;
            int nLeves = logicPack.levels.Length;
            if (completedLevels == nLeves && perfectLevels == nLeves)
                star.enabled = true;
            else if (completedLevels == nLeves)
                check.enabled = true;

            Logic.Level[] logicLevels = logicPack.levels;

            string[] levels = GameManager.Instance.getLevelPack().levels.ToString().Split('\n');

            float auxWidth = 0;
            int nPanel = -1;
            //Recorre los paneles
            for (int i = 0; i < (levels.Length - 1) / panelSize; i++)
            {
                if (levels[i * panelSize].Split(',')[2] == "1")
                {
                    nPanel++;
                }

                LevelsPanel lvlPanel = Instantiate(levelsPanel, transform);
                auxWidth += lvlPanel.getWidth() + hLayout.spacing;

                lvlPanel.setDimensionsText(pack.levelPanelName[i]);
                //Recorre los niveles del panel
                for (int j = 0; j < panelSize; j++)
                {
                    int nLevel = i * panelSize + j;
                    lvlPanel.setlvlButton(pack.levelPanelColors[i], levels[nLevel], j, nPanel, i * panelSize + j, logicLevels[nLevel]);
                }
            }

            rectTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, auxWidth + hLayout.spacing);
        }

        void backClickCallback()
        {
            GameManager.Instance.gotoSelectCategoryMenu();
        }

    }
}
