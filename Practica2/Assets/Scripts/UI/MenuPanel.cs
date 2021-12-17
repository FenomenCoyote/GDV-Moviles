using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    Text panelText;                   //The panel text

    [SerializeField]
    Image checkImg;                   //The star image

    [SerializeField]
    Image starImg;                    //The star image

    Image img;                       //The image attached to this gameobject
    RectTransform rectTransform;     //The RectTransform attached to this gameobject

    private void Awake()
    {
        img = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Sets the panel text
    /// </summary>
    /// <param name="text">The text of the panel</param>
    public void setText(string text)
    {
        panelText.text = text;
    }

    /// <summary>
    /// Sets the image color
    /// </summary>
    /// <param name="color"></param>
    public void setColor(Color color)
    {
        img.color = color;
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
    /// Enables or disables the check image 
    /// </summary>
    /// <param name="enable">if the image is enabled or not</param>
    public void enableCheck(bool enable)
    {
        checkImg.enabled = enable;
    }

    /// <summary>
    /// Enables or disables the start image 
    /// </summary>
    /// <param name="enable">if the image is enabled or not</param>
    public void enableStar(bool enable)
    {
        starImg.enabled = enable;
    }
}
