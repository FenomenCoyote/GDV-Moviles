using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{
    public class ShowCategories : MonoBehaviour
    {
        private PackCategory[] categories;

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
                               "<color=green>i</color>"+
                               "<color=blue>v</color>"+
                               "<color=yellow>e</color>"+
                               "<color=orange>l</color>"+
                               "<color=red>e</color>"+
                               "<color=green>s</color>";

            //Esto es bucear en la escena?
            auxHeight += nivelesText.rectTransform.rect.height + vOrder.spacing;

            categories = GameManager.Instance.getPackCategories();

            foreach(PackCategory category in categories)
            {
                MenuPanel panel = Instantiate(categoryHeader, transform);
                panel.setColor(category.categoryColor);
                panel.setText(category.categoryName);
                auxHeight += panel.getHeight() + vOrder.spacing;

                foreach (LevelPack levelPack in category.levelPacks)
                {
                    MenuButton packButton = Instantiate(categoryButton, transform);
                    packButton.setPackName(levelPack.packName, category.categoryColor);
                    auxHeight += packButton.getHeight() + vOrder.spacing;
                }
            }

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, auxHeight);

        }
    }

}