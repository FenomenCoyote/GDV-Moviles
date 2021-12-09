using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow {

    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private Board board;

        private string level;
        private LevelPack pack;

        public void setLevel(string levelInfo, LevelPack selectedPack)
        {
            level = levelInfo;
            pack = selectedPack;
        }

        private void Start()
        {
            initLevel();
        }

        private void initLevel()
        {
            Logic.Map map = new Logic.Map();
            map.loadLevel(level);

            board.setForGame(map, pack.theme.colors);
        }

        public void exitLevel()
        {
            GameManager.Instance.exitLevel();
        }

        public void nextLevel()
        {

        }

        public void previousLevel()
        {

        }
    }

}