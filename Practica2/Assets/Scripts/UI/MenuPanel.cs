using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    Text panelText;

    [SerializeField]
    Image checkImg;

    [SerializeField]
    Image starImg;

    Image img;
    RectTransform rectTransform;

    private void Awake()
    {
        img = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void setText(string text)
    {
        panelText.text = text;
    }

    public void setColor(Color color)
    {
        img.color = color;
    }

    public float getHeight()
    {
        return rectTransform.rect.height;
    }

    public void enableCheck(bool enable)
    {
        checkImg.enabled = enable;
    }

    public void enableStar(bool enable)
    {
        starImg.enabled = enable;
    }
}
