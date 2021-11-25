using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow {

    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private Board board;

        public void initLevel(LevelPack pack, int level)
        {
            string[] levels = pack.levels.text.Split('\n');

            Logic.Map map = new Logic.Map();
            map.loadLevel(levels[level]);

            board.setForGame(map, pack.theme.colors);
        }
    }

}