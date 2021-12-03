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

        void Start()
        {

            nivelesText.text = "<color=red>n</color>" +
                               "<color=green>i</color>"+
                               "<color=blue>v</color>"+
                               "<color=yellow>e</color>"+
                               "<color=orange>l</color>"+
                               "<color=red>e</color>"+
                               "<color=green>s</color>";

            categories = GameManager.Instance.getPackCategories();

            foreach(PackCategory category in categories)
            {
                MenuPanel panel = Instantiate(categoryHeader, transform);
                panel.setColor(category.categoryColor);
                panel.setText(category.categoryName);

                foreach (LevelPack levelPack in category.levelPacks)
                {
                    MenuButton packButton = Instantiate(categoryButton, transform);
                    packButton.setPackName(levelPack.name, category.categoryColor);
                    packButton.setColor(category.categoryColor);
                }
            }

        }
    }

}