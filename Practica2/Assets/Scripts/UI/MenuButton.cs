using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton: MonoBehaviour
{
    [SerializeField]
    Text levelPackName;

    [SerializeField]
    Text packProgress;

    [SerializeField]
    Image finishedPackCheck;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void setPackName(string packName, Color color)
    {
        levelPackName.text = packName;
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
}
