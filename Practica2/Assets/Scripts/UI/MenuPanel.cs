using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    Text panelText;

    Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void setText(string text)
    {
        panelText.text = text;
    }

    public void setColor(Color color)
    {
        img.color = color;
    }
}
