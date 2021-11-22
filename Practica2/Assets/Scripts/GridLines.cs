using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class GridLines : MonoBehaviour
    {
        [SerializeField] private GameObject contour;

        private uint width, height;

#if UNITY_EDITOR
        void Start()
        {
            if(contour == null)
            {
                Debug.LogError("Line sprite of grid is not setted");
                return;
            }

            setSizeAndInstantiate(5, 5);
        }
#endif

        public void setSizeAndInstantiate(uint w, uint h)
        {
            width = w;
            height = h;

            Vector3 pos = Vector3.zero;
            Vector3 scale = Vector3.one;

            scale.y = width;

            pos.y = (height / 2) + 0.5f;
            for (int i = 0; i < height + 1; i++)
            {
                GameObject line = Instantiate(contour, pos, Quaternion.AngleAxis(90.0f, Vector3.forward), transform);
                line.transform.localScale = scale;

                Vector3 aux = line.transform.localPosition;
                aux.y = pos.y;
                line.transform.localPosition = aux;

                pos.y--;
            }

            scale.y = height;
            pos.y = 0.0f;
            pos.x = -(width / 2) - 0.5f;
            for (int i = 0; i < height + 1; i++)
            {
                GameObject line = Instantiate(contour, pos, Quaternion.identity, transform);
                line.transform.localScale = scale;

                Vector3 aux = line.transform.localPosition;
                aux.x = pos.x;
                line.transform.localPosition = aux;

                pos.x++;
            }
        }
    }
}