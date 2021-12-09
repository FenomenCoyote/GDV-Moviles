using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace flow.UI { 
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]Text levelTextNumber;
        [SerializeField]Image check;
        [SerializeField]Image lockImg;
        [SerializeField]Image star;

        Image img;
        RectTransform rectTr;
        Button button;

        string levelInfo;
        int nPanel;

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

        public void setLevelInfo(string lvl, int nPanel)
        {
            levelTextNumber.text = lvl.Split(',')[2];
            levelInfo = lvl;
            this.nPanel = nPanel;
        }

        public void enableCheck(bool b)
        {
            check.enabled = b;
        }
        public void enableLock(bool b)
        {
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
            GameManager.Instance.setLevel(levelInfo, nPanel);
        }

    }
}