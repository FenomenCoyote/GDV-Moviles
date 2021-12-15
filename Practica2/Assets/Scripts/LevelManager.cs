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
        private HintManager hintManager;

        [SerializeField]
        private Text completeText;
        [SerializeField]
        private GameObject nextLevelPanelButton;
        [SerializeField]
        private GameObject choosePackPanelButton;
        [SerializeField]
        private GameObject getMoreHintsPanelButton;
        [SerializeField]
        private GameObject getMoreLevelsPanelButton;

        [SerializeField]
        private Image nextLevelButtonImg;

        [SerializeField]
        private Image previousLevelButtonImg;

        [SerializeField]
        private GameObject levelDonePanel;

        [SerializeField]
        private Text doneInStepsText;

        [SerializeField]
        private UI.EndImage endImage;

        [SerializeField]
        RewardedAdsButton rewardedAdButton;

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

                recordText.text = "récord: " + logicLevel.record.ToString();
            }
            int t = GameManager.Instance.getLevelPackSize();
            if (currentLvlIndex == 0)
            {
                previousLevelButtonImg.color = Color.gray;
            }
            else if (currentLvlIndex >= GameManager.Instance.getLevelPackSize() - 2) { //-2 porque el tamaño del level pack es 151)
                
                nextLevelButtonImg.color = Color.gray;
            }

            board.setForGame(map, GameManager.Instance.GetColorTheme().colors, category.categoryColor, logicLevel.record);

            levelDonePanel.SetActive(false);

            rewardedAdButton.LoadAd();
        }

        public void resetLevel()
        {
            board.resetBoard();

            levelDonePanel.SetActive(false);
        }

        public void levelDone(int steps)
        {
            bool perfect = steps == board.getNPipes();
            GameManager.Instance.levelFinished(steps, perfect);

            Logic.GameState state = GameManager.Instance.getState();
            Logic.Level logicLevel = state.getCategoryByName(category.categoryName).getPackByName(pack.packName).levels[currentLvlIndex];

            if (logicLevel.record == board.getNPipes())
            {
                endImage.enableCheck(false);
                endImage.enableStar(true);
            }
            else
            {
                endImage.enableStar(false);
                endImage.enableCheck(true);
            }
            recordText.text = "récord: " + logicLevel.record.ToString();

            levelDonePanel.SetActive(true);
            //if we are complete the last level we hhave finished the leval pack
            if (currentLvlIndex >= GameManager.Instance.getLevelPackSize() - 2)
            {
                completeText.text = "Congratulations!";
                doneInStepsText.text = "You completed the" + pack.packName;
                nextLevelPanelButton.SetActive(false);
                getMoreHintsPanelButton.SetActive(false);

                choosePackPanelButton.SetActive(true);
                getMoreLevelsPanelButton.SetActive(true);
            }
            else 
            {
                completeText.text = "Level Complete!";
                doneInStepsText.text = "You completed the level in " + steps + " moves.";
                nextLevelPanelButton.SetActive(true);
                getMoreHintsPanelButton.SetActive(true);

                choosePackPanelButton.SetActive(false);
                getMoreLevelsPanelButton.SetActive(false);
            }
            
            //disable board inputs
            board.disableInput();

            GameManager.Instance.soundManager.playSound(SoundManager.Sound.Flow);
        }

        public void continueLevel()
        {
            board.enableInput();
            levelDonePanel.SetActive(false);
        }

        public void applyHint()
        {
            if (GameManager.Instance.getNHints() <= 0)
                return;

            GameManager.Instance.hintWasted();

            hintManager.hintsChanged((int)GameManager.Instance.getNHints());

            //aplicar la pista, llamar aqui a un metodo del board
            board.nextHint();
        }

        public void addHint()
        {
            GameManager.Instance.addHint();
            hintManager.hintsChanged((int)GameManager.Instance.getNHints());
        }


        public void exitLevel()
        {
            GameManager.Instance.exitLevel();
        }

        public void nextLevel()
        {
            int nextLevel = currentLvlIndex + 1;
            if(nextLevel<GameManager.Instance.getLevelPackSize()-1)
            {
                bool lockedNextLevel = GameManager.Instance.getState().getCategoryByName(category.categoryName).getPackByName(pack.packName).levels[nextLevel].locked;
                if (!lockedNextLevel)
                {
                    GameManager.Instance.soundManager.playSound(SoundManager.Sound.Forward);
                    GameManager.Instance.selectLevel(nextLevel);
                }
            }          
        }

        public void previousLevel()
        {
            GameManager.Instance.soundManager.playSound(SoundManager.Sound.Back);
            GameManager.Instance.selectLevel(currentLvlIndex - 1);
        }

        public void goToSelectCategoryMenu()
        {
            GameManager.Instance.gotoSelectCategoryMenu();
        }
    }
}