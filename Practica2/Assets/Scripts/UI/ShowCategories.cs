using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{
    public class ShowCategories : MonoBehaviour
    {
        [SerializeField]
        Text nivelesText;

        [SerializeField]
        MenuPanel categoryHeader;

        [SerializeField]
        MenuButton categoryButton;

        RectTransform rectTransform;

        VerticalLayoutGroup vOrder;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            vOrder = GetComponent<VerticalLayoutGroup>();
        }

        void Start()
        {
            float auxHeight = 0.0f;

            nivelesText.text = "<color=red>l</color>" +
                               "<color=green>e</color>" +
                               "<color=blue>v</color>" +
                               "<color=yellow>e</color>" +
                               "<color=orange>l</color>" +
                               "<color=cyan>s</color>";

            //Esto es bucear en la escena?
            auxHeight += nivelesText.rectTransform.rect.height + vOrder.spacing;

            PackCategory[] categories = GameManager.Instance.getPackCategories();
            Logic.GameState state = GameManager.Instance.getState();

            for (int c = 0; c < categories.Length; ++c)
            {
                PackCategory category = categories[c];

                MenuPanel panel = Instantiate(categoryHeader, transform);

                Logic.Category logicCategory = state.categories[c];

                int completedPacks = logicCategory.completedPacks;
                int perfectPacks = logicCategory.perfectPacks;
                int nPacks = logicCategory.packs.Length;

                if (completedPacks == nPacks && perfectPacks == nPacks)
                    panel.enableStar(true);
                else if (completedPacks == nPacks)
                    panel.enableCheck(true);

                panel.setColor(category.categoryColor);
                panel.setText(category.categoryName);
                auxHeight += panel.getHeight() + vOrder.spacing;

                for (int p = 0; p < category.levelPacks.Length; ++p)
                {
                    MenuButton packButton = Instantiate(categoryButton, transform);
                    
                    LevelPack pack = category.levelPacks[p];
                    Logic.LvlPack logicPack = logicCategory.packs[p];

                    int completedLevels = logicPack.completedLevels;
                    int perfectLevels = logicPack.perfectLevels;
                    int nLeves = logicCategory.packs[p].levels.Length;
                    if (completedLevels == nLeves && perfectLevels == nLeves)
                        packButton.enableStar(true);
                    else if (completedLevels == nLeves)
                        packButton.enableCheck(true);

                    packButton.setPack(pack, category);
                    auxHeight += packButton.getHeight() + vOrder.spacing;

                    packButton.setPackProgress(state.categories[c].packs[p].completedLevels, nLeves);
                }
            }

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, auxHeight);

        }
    }

}