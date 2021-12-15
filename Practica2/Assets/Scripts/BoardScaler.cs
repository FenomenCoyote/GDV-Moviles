using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardScaler : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    [SerializeField]
    RectTransform statisticText;

    [SerializeField]
    RectTransform backButton;

    [SerializeField]
    RectTransform resetLevelButton;

    [SerializeField] float OFFSETX = 1f;
    [SerializeField] float OFFSETY = 3f;

    float heightInUnits;
    float widthInUnits;

    float topDownMargins;

    private void Start()
    {
        heightInUnits = camera.orthographicSize * 2;
        widthInUnits = heightInUnits * ((float)Screen.width / (float)Screen.height);

        float unitsPerPixel = heightInUnits / Screen.height;
        topDownMargins = 0;
        topDownMargins += statisticText.rect.height * unitsPerPixel;
        topDownMargins += backButton.rect.height * unitsPerPixel;
        topDownMargins += resetLevelButton.rect.height * unitsPerPixel;
    }

    public void fitInScreen(int boardWidth, int boardHeight)
    {
        float scaleWidth = (widthInUnits - OFFSETX) / boardWidth;

        float offsetY = OFFSETY + topDownMargins;
        float scaleHeight = (heightInUnits - offsetY) / boardHeight;

        float scale = Mathf.Min(scaleHeight, scaleWidth);
       
        transform.localScale = new Vector3(scale, scale, 0);
    }
}
