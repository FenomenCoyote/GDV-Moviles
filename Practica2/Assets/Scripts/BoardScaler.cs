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

    const float OFFSETX = 1f;
    const float OFFSETY = 3f;

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
        float scale;
        if (boardWidth >= boardHeight)
            scale = (widthInUnits - OFFSETX) / boardWidth;
        else
        {
            float offsetY = OFFSETY + topDownMargins;
            scale = (heightInUnits - offsetY) / boardHeight;
        }

        transform.localScale = new Vector3(scale, scale, 0);
    }
}
