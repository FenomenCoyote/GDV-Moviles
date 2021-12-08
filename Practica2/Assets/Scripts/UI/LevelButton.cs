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

        void Awake()
        {           
            img = GetComponent<Image>();
            rectTr = GetComponent<RectTransform>();
        }

        
        public void setButtonLevelColor(Color c)
        {
            img.color = c;
        }

        public void setLevelNumberText(string lvl)
        {
            levelTextNumber.text = lvl;
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

    }
}