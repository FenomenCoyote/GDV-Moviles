using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace flow.UI
{
    public class MenuButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        Text levelPackName;

        [SerializeField]
        Text packProgress;

        [SerializeField]
        Image finishedPackCheck;

        [SerializeField]
        Image perfectPackStar;

        RectTransform rectTransform;

        LevelPack pack;
        PackCategory category;

        Button button;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            button = GetComponent<Button>();
        }

        private void Start()
        {
            //Here we add a callback method to the button
            button.onClick.AddListener(delegate () { clickCallback(); });
        }

        /// <summary>
        /// when we press the level pack button, changes the color of the level pack name to white 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            levelPackName.color = Color.white;
        }
        /// <summary>
        /// When we release the level pack button, changes the color color of the level pack name to the category color
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            levelPackName.color = category.categoryColor;
        }

        /// <summary>
        /// Sets the level pack and the pack categoty of this menuButton.
        /// We also set the level pack name and its color.
        /// </summary>
        /// <param name="levelPack">Level pack</param>
        /// <param name="cat">Pack category</param>
        public void setPack(LevelPack levelPack, PackCategory cat)
        {
            pack = levelPack;
            category = cat;
            levelPackName.text = pack.packName;
            levelPackName.color = category.categoryColor;
        }

        /// <summary>
        /// Sets the level we have completed of the level pack in a text
        /// </summary>
        /// <param name="levelsDone">Levels we have completed</param>
        /// <param name="totalLevels">Totals levelsof the level pack</param>
        public void setPackProgress(int levelsDone, int totalLevels)
        {
            packProgress.text = levelsDone.ToString() + "/" + totalLevels.ToString();
        }

        /// <summary>
        /// Enables or disables the check image 
        /// </summary>
        /// <param name="enable">if the image is enabled or not</param>
        public void enableCheck(bool enable)
        {
            finishedPackCheck.enabled = enable;
        }

        /// <summary>
        /// Enables or disables the start image 
        /// </summary>
        /// <param name="enable">if the image is enabled or not</param>
        public void enableStar(bool enable)
        {
            perfectPackStar.enabled = enable;
        }

        /// <summary>
        /// Returns the height of the rect transform attached to this gameobject
        /// </summary>
        /// <returns></returns>
        public float getHeight()
        {
            return rectTransform.rect.height;
        }


        /// <summary>
        /// This callback is called when we preseed the button.
        /// It sets the selected category pack and the level pack to the GameManager
        /// </summary>
        private void clickCallback()
        {
            if(pack != null)
                GameManager.Instance.selectPack(pack,category);
        }
    }
}