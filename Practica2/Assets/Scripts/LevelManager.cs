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

        [SerializeField]
        private Text recordText;

        [SerializeField]
        private Image nextLevelButtonImg;

        [SerializeField]
        private Image previousLevelButtonImg;

        [SerializeField]
        private UI.EndImage endImage;

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

            Logic.GameState state = GameManager.Instance.getState();
            Logic.Level logicLevel = state.getCategoryByName(category.categoryName).getPackByName(pack.packName).levels[currentLvlIndex];
            if (logicLevel.completed)
            {
                if (logicLevel.record == map.getNPipes())
                    endImage.enableStar(true);
                else endImage.enableCheck(true);
            }
    

            if (currentLvlIndex == 0) previousLevelButtonImg.color = Color.gray;
            //-2 porque el tamaño del level pack es 151
            else if (currentLvlIndex >= GameManager.Instance.getLevelPackSize()-2) nextLevelButtonImg.color = Color.gray;

            board.setForGame(map, pack.theme.colors, category.categoryColor, logicLevel.record);
        }

        public void levelDone(int steps)
        {
            GameManager.Instance.levelFinished(steps);

            //update level info

            Logic.GameState state = GameManager.Instance.getState();
            Logic.Level logicLevel = state.getCategoryByName(category.categoryName).getPackByName(pack.packName).levels[currentLvlIndex];

            if (logicLevel.record == board.getNPipes())
            {
                recordText.text = "récord: " + logicLevel.record.ToString();
                endImage.enableCheck(false);
                endImage.enableStar(true);
            }
            else
            {
                endImage.enableStar(false);
                endImage.enableCheck(true);
            }


        }

        public void exitLevel()
        {
            GameManager.Instance.exitLevel();
        }

        public void nextLevel()
        {
           bool lockedNextLevel = GameManager.Instance.getState().getCategoryByName(category.categoryName).getPackByName(pack.packName).levels[currentLvlIndex+1].locked;
           if(!lockedNextLevel) GameManager.Instance.selectLevel(currentLvlIndex + 1);
        }

        public void previousLevel()
        {
            GameManager.Instance.selectLevel(currentLvlIndex - 1);
        }
    }
}