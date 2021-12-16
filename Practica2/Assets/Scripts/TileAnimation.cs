using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimation : MonoBehaviour
{
    [Tooltip("Scale curve")]
    [SerializeField] private AnimationCurve animatorCurve;

    [Tooltip("Duration")]
    [SerializeField][Range(0.1f, 10f)] private float time = 1.0f;

    [Tooltip("How much does it scale")]
    [SerializeField][Range(0.1f, 2f)] private float animationScale = 0.25f;

    [Tooltip("if it hides and fades")]
    [SerializeField] private bool hideWhenEnded;

    private Vector3 initialScale;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
    }

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
        if (hideWhenEnded)
            sprite.enabled = true;

        float i = 0;
        float rate = 1 / time;
        while (i < 1)
        {
            i += rate * Time.deltaTime;
            transform.localScale = initialScale + initialScale * animatorCurve.Evaluate(i) * animationScale;
            if (hideWhenEnded)
            {
                Color aux = sprite.color;
                aux.a = 1 - animatorCurve.Evaluate(i);
                sprite.color = aux;
            }
            yield return 0;
        }

        if (hideWhenEnded)
            sprite.enabled = false;
    }
}
