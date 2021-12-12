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
            button.onClick.AddListener(delegate () { clickCallback(); });
        }

        public void setButtonLevelColor(Color c)
        {
            img.color = c;
        }

        public void setLevelInfo(string lvl, int nPanel, int lvlIndex)
        {
            levelTextNumber.text = lvl.Split(',')[2];
            levelInfo = lvl;
            this.nPanel = nPanel;
            this.lvlIndex = lvlIndex;
        }

        public void enableCheck(bool b)
        {
            check.enabled = b;
        }
        public void enableLock(bool b)
        {
            locked = b;
            lockImg.enabled = b;
        }
        public void enableStar(bool b)
        {
            star.enabled = b;
        }

        public float getWidth()
        {
            return rectTr.rect.width;
        }

        private void clickCallback()
        {
            if (!locked)
                GameManager.Instance.setLevel(levelInfo, lvlIndex, nPanel);
        }

    }
}