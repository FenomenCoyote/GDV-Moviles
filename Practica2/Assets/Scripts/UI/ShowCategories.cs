using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{
    public class ShowCategories : MonoBehaviour
    {
        [SerializeField]
        Text levelsText;                //The text that contains the word "levels"

        [SerializeField]
        MenuPanel categoryHeader;       //MenuPanel object

        [SerializeField]
        MenuButton categoryButton;      //CategoryButton object

        [SerializeField]
        Text removeAdsText;             //Remove ads text

        RectTransform rectTransform;    //The RectTransform attached to this gameobject

        VerticalLayoutGroup vOrder;     //The VerticalLayout attached to this gameobject

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            vOrder = GetComponent<VerticalLayoutGroup>();
        }

        /// <summary>
        /// Returns the color as a hexadecimal string in the format "RRGGBB"
        /// </summary>
        /// <param name="color">The color to be converted</param>
        /// <returns></returns>
        private string ColorToHex(Color32 color)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }

        void Start()
        {
            float totalHeight = 0.0f;  //Total width of the RectTransform

            //We get the color theme
            ColorTheme theme = GameManager.Instance.GetColorTheme();

            //We set the level text
            levelsText.text = "<color=" + ColorToHex(theme.colors[0]) + ">l</color>" +
                               "<color=" + ColorToHex(theme.colors[1]) + ">e</color>" +
                               "<color=" + ColorToHex(theme.colors[2]) + ">v</color>" +
                               "<color=" + ColorToHex(theme.colors[3]) + ">e</color>" +
                               "<color=" + ColorToHex(theme.colors[4]) + ">l</color>" +
                               "<color=" + ColorToHex(theme.colors[5]) + ">s</color>";

            //We add to the total height the levelsText and the removeAds height
            totalHeight += levelsText.rectTransform.rect.height + vOrder.spacing;
            totalHeight += removeAdsText.rectTransform.rect.height + vOrder.spacing;

            //We get the pack categories array
            PackCategory[] categories = GameManager.Instance.getPackCategories();
            //We get the gameState of the game
            Logic.GameState state = GameManager.Instance.getState();

            //We iterate through the categories
            for (int c = 0; c < categories.Length; ++c)
            {
                //We get the PackCategory
                PackCategory category = categories[c];

                //We instantiate a MenulPanel object
                MenuPanel panel = Instantiate(categoryHeader, transform);

                //We get the Category container
                Logic.Category logicCategory = state.categories[c];

                //We get some information of the Category container
                int completedPacks = logicCategory.completedPacks;
                int perfectPacks = logicCategory.perfectPacks;
                int nPacks = logicCategory.packs.Length;

                //If we have completed all levels packs perfectly we enable the star image
                if (completedPacks == nPacks && perfectPacks == nPacks)
                    panel.enableStar(true);
                else if (completedPacks == nPacks)  //If we have completed all levels packs we enable the check image
                    panel.enableCheck(true);

                //We set the panel color and text
                panel.setColor(category.categoryColor);
                panel.setText(category.categoryName);

                //We add to the total height the panel height
                totalHeight += panel.getHeight() + vOrder.spacing;

                //We iterate through the levelPacks of the Category container
                for (int p = 0; p < category.levelPacks.Length; ++p)
                {
                    //We instantiate a MenulPanel object
                    MenuButton packButton = Instantiate(categoryButton, transform);
                    
                    //We get the LevelPack and the levelPack container
                    LevelPack pack = category.levelPacks[p];
                    Logic.LvlPack logicPack = logicCategory.packs[p];

                    //We get some information of the LevelPack container
                    int completedLevels = logicPack.completedLevels;
                    int perfectLevels = logicPack.perfectLevels;
                    int nLeves = logicCategory.packs[p].levels.Length;
                    //If we have completed all levels perfectly we enable the star image
                    if (completedLevels == nLeves && perfectLevels == nLeves)
                        packButton.enableStar(true);
                    else if (completedLevels == nLeves) //If we have completed all levels we enable the check image
                        packButton.enableCheck(true);

                    //We set the LevelPack a the PackCategory
                    packButton.setPack(pack, category);

                    //We add to the total height the packButon height
                    totalHeight += packButton.getHeight() + vOrder.spacing;

                    //We set packProgressText
                    packButton.setPackProgress(state.categories[c].packs[p].completedLevels, nLeves);
                }
            }

            //We set the height of the RectTrasform
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

        }
    }

}