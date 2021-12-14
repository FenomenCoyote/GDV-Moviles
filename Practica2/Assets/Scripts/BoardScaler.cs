using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScaler : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    public void fitInScreen(int boardWidth, int boardHeight)
    {
        //float heightInUnits = camera.orthographicSize * 2;
        //float widthInUnits = heightInUnits * ((float)Screen.width / (float)Screen.height);

        //float maxWidth = widthInUnits - 1;
        //float maxHeight = heightInUnits - 3;

        //float propX = maxWidth / widthInUnits;
        //float propY = maxHeight / heightInUnits;

        //float scale = Mathf.Min(propX, propY);

        //transform.localScale = new Vector3(scale, scale, 0);

        float propX = 5.0f / boardWidth;
        float propY = 8.0f / boardHeight;

        float scale = Mathf.Min(propX, propY);
        scale *= transform.localScale.x;

        Vector3 newScale = new Vector3(scale, scale, 1);
        transform.localScale = newScale;
    }
}
