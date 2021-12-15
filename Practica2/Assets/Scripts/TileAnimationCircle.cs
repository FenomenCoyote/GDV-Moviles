using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimationCircle : MonoBehaviour
{
    [SerializeField] private AnimationCurve animatorCurve;

    [SerializeField] private float time = 1.0f;

    private Vector3 initialScale;

    public void setInitialScale(Vector3 scale)
    {
        initialScale = scale;
    }

    public void startAnim()
    {
        StartCoroutine(Scale());
    }

    private IEnumerator Scale()
    {
        float i = 0;
        float rate = 1 / time;
        while (i < 1)
        {
            i += rate * Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, initialScale * animatorCurve.Evaluate(i), animatorCurve.Evaluate(i));
            yield return 0;
        }
    }
}
