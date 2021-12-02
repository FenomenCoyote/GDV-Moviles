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

    Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
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

    public void setColor(Color color)
    {
        img.color = color;
    }
}
