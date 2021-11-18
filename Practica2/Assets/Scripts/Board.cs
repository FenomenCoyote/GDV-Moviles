using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Tile tile;

#if UNITY_EDITOR
        void Start()
        {
            if(tile == null)
            {
                Debug.LogError("Prefab of board not setted");
                return;
            }
        }
#endif
        public void setForGame(Logic.Map map)
        {
            Vector3 pos = transform.position;
            pos.y = -2f;
            for (int i = 0; i < 5; i++)
            {
                pos.x = -2f;
                for (float j = 0; j < 5; j++)
                {
                    Instantiate<Tile>(tile, pos, Quaternion.identity, transform);
                    pos.x++;
                }
                pos.y++;
            }
        }
    }
}
