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

            Logic.Map map = new Logic.Map();
            map.loadLevel("5,0,1,5;18,17,12;21,16,11,6;3,4,9;0,1,2,7,8,13,14,19,24,23,22;20,15,10,5");
            setForGame(map);
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
