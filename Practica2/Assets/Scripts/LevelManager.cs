using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace flow {

    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private Board board;

        [SerializeField]
        private Text levelText;

        [SerializeField]
        private Text boardSizeText;

        private string level;
        private LevelPack pack;
        private PackCategory category;
        private int currentLvlIndex;


        public void setLevel(string levelInfo, int index,LevelPack selectedPack, PackCategory selectedCategory)
        {
            level = levelInfo;
            currentLvlIndex = index;
            pack = selectedPack;
            category = selectedCategory;
        }

        private void Start()
        {
            initLevel();
        }

        private void initLevel()
        {
            Logic.Map map = new Logic.Map();
            map.loadLevel(level);

            levelText.text = "Level " + map.getNLevel();
            levelText.color = category.categoryColor;
            boardSizeText.text = map.getLevelWidth().ToString() + "x" + map.getLevelHeight();

            board.setForGame(map, pack.theme.colors, category.categoryColor);
        }

        public void levelDone(int perfectGame, int steps)
        {
            Debug.Log("Level done");
        }

        public void exitLevel()
        {
            GameManager.Instance.exitLevel();
        }

        public void nextLevel()
        {
            GameManager.Instance.selectLevel(currentLvlIndex + 1);
        }

        public void previousLevel()
        {
            GameManager.Instance.selectLevel(currentLvlIndex - 1);
        }
    }
}