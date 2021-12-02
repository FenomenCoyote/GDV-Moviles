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

        // Start is called before the first frame update
        void Start()
        {

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