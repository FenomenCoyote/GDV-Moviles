using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{

    public class SelectLevels : MonoBehaviour
    {

        [SerializeField] Text categoryText;
        [SerializeField] Image check;
        [SerializeField] Button backButton;

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

            categoryText.text = pack.packName;
            categoryText.color = category.categoryColor;
            check.color = category.categoryColor;

            string[] levels = GameManager.Instance.getLevelPack().levels.ToString().Split('\n');

            float auxWidth = 0;
            int nPanel = -1;
            for (int i = 0; i < levels.Length / panelSize; i++)
            {
                if (levels[i * panelSize].Split(',')[2] == "1")
                    nPanel++;

                LevelsPanel lvlPanel = Instantiate(levelsPanel, transform);
                auxWidth += lvlPanel.getWidth() + hLayout.spacing;

                lvlPanel.setDimensionsText(pack.levelPanelName[i]);
                for (int j = 0; j < panelSize; j++)
                {
                    lvlPanel.setlvlButton(pack.levelPanelColors[i], levels[i * panelSize + j], j, nPanel);
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
