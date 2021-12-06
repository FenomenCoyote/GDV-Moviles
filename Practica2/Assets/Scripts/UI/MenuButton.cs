using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow.UI
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField]
        Text levelPackName;

        [SerializeField]
        Text packProgress;

        [SerializeField]
        Image finishedPackCheck;

        RectTransform rectTransform;

        LevelPack pack;

        Button button;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(delegate () { clickCallback(); });
        }

        public void setPack(LevelPack levelPack, Color color)
        {
            pack = levelPack;
            levelPackName.text = pack.packName;
            levelPackName.color = color;
        }

        public void setPackProgress(int levelsDone, int totalLevels)
        {
            //TODO
        }

        public void enableFinishedPackCheck(bool enable)
        {
            finishedPackCheck.enabled = enable;
        }

        public float getHeight()
        {
            return rectTransform.rect.height;
        }

        private void clickCallback()
        {
            if(pack != null)
                GameManager.Instance.selectPack(pack);
        }
    }
}