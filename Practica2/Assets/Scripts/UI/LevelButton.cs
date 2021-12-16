using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace flow.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] Text levelTextNumber;
        [SerializeField] Image check;
        [SerializeField] Image lockImg;
        [SerializeField] Image star;

        Image img;
        RectTransform rectTr;
        Button button;

        bool locked;

        string levelInfo;
        int nPanel;
        int lvlIndex;

        void Awake()
        {
            img = GetComponent<Image>();
            rectTr = GetComponent<RectTransform>();
            button = GetComponent<Button>();
        }

        private void Start()
        {
            //Here we add a callback method to the button
            button.onClick.AddListener(delegate () { clickCallback(); });
        }

        /// <summary>
        /// Sets the color of the image
        /// </summary>
        /// <param name="c">color</param>
        public void setButtonLevelColor(Color c)
        {
            img.color = c;
        }

        /// <summary>
        /// Sets the level information of this levelButton
        /// </summary>
        /// <param name="lvl">The level string with its configuration</param>
        /// <param name="nPanel">The panel where is the level</param>
        /// <param name="lvlIndex">The index of the levels array</param>
        public void setLevelInfo(string lvl, int nPanel, int lvlIndex)
        {
            levelTextNumber.text = lvl.Split(',')[2]; //we set the level number
            levelInfo = lvl;
            this.nPanel = nPanel;
            this.lvlIndex = lvlIndex;
        }

        /// <summary>
        /// Enables or disables the check image
        /// </summary>
        /// <param name="b">if the image is enabled or not</param>
        public void enableCheck(bool b)
        {
            check.enabled = b;
        }
        /// <summary>
        /// Enables or disables the lock image and sets if the levelButton is locked or not
        /// </summary>
        /// <param name="b">if the image is enabled or not</param>
        public void enableLock(bool b)
        {
            locked = b;
            lockImg.enabled = b;
        }
        /// <summary>
        /// Enables or disables the start image
        /// </summary>
        /// <param name="b">if the image is enabled or not</param>
        public void enableStar(bool b)
        {
            star.enabled = b;
        }

        /// <summary>
        /// Returns the width of the rect transform attached to this gameobject
        /// </summary>
        /// <returns></returns>
        public float getWidth()
        {
            return rectTr.rect.width;
        }

        /// <summary>
        /// This callback is called when we preseed the button.
        /// It sets the level information to the GameManager
        /// </summary>
        private void clickCallback()
        {
            if (!locked)
                GameManager.Instance.setLevel(levelInfo, lvlIndex, nPanel);
        }

    }
}