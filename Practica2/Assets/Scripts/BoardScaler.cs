using UnityEngine;

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
        //Calculate screen width and height in unity units
        heightInUnits = camera.orthographicSize * 2;
        widthInUnits = heightInUnits * ((float)Screen.width / (float)Screen.height);

        //Calculate height units per pixel and margins of UI
        float unitsPerPixel = heightInUnits / Screen.height;
        topDownMargins = 0;
        topDownMargins += statisticText.rect.height * unitsPerPixel;
        topDownMargins += backButton.rect.height * unitsPerPixel;
        topDownMargins += resetLevelButton.rect.height * unitsPerPixel;
    }

    public void fitInScreen(int boardWidth, int boardHeight)
    {
        //Calculate scale if board scale is based on width
        float scaleWidth = (widthInUnits - OFFSETX) / boardWidth;

        //Calculate scale if board scale is based on height
        float offsetY = OFFSETY + topDownMargins;
        float scaleHeight = (heightInUnits - offsetY) / boardHeight;

        //Use minimun scale to fit board in screen
        float scale = Mathf.Min(scaleHeight, scaleWidth);   
        transform.localScale = new Vector3(scale, scale, 0);
    }
}
