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

            nivelesText.text = "<color=red>n</color>" +
                               "<color=green>i</color>" +
                               "<color=blue>v</color>" +
                               "<color=yellow>e</color>" +
                               "<color=orange>l</color>" +
                               "<color=red>e</color>" +
                               "<color=green>s</color>";

            //Esto es bucear en la escena?
            auxHeight += nivelesText.rectTransform.rect.height + vOrder.spacing;

            PackCategory[] categories = GameManager.Instance.getPackCategories();
            Logic.GameState state = GameManager.Instance.getState();

            for (int c = 0; c < categories.Length; ++c)
            {
                PackCategory category = categories[c];

                MenuPanel panel = Instantiate(categoryHeader, transform);
                panel.setColor(category.categoryColor);
                panel.setText(category.categoryName);
                auxHeight += panel.getHeight() + vOrder.spacing;

                for (int p = 0; p < category.levelPacks.Length; ++p)
                {
                    LevelPack pack = category.levelPacks[p];

                    MenuButton packButton = Instantiate(categoryButton, transform);
                    packButton.setPack(pack, category);
                    auxHeight += packButton.getHeight() + vOrder.spacing;

                    string[] levels = pack.levels.ToString().Split('\n');
                    int nLeves = levels.Length - 1;
                    packButton.setPackProgress(state.categories[c].packs[p].completedLevels, nLeves);
                }
            }

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, auxHeight);

        }
    }

}