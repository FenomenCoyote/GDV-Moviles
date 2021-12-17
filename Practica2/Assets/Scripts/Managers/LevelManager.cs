using UnityEngine;
using UnityEngine.UI;

namespace flow
{

    /// <summary>
    /// Class which implements the funcionalities of level scene UI and initializes board
    /// </summary>
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
        private UI.HintShower hintShower;

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
        private Button nextLevelButton;

        [SerializeField]
        private Button previousLevelButton;

        [SerializeField]
        private GameObject levelDonePanel;

        [SerializeField]
        private Text doneInStepsText;

        [SerializeField]
        private UI.EndImage endImage;

        private string level;
        private LevelPack pack;
        private PackCategory category;
        private int currentLvlIndex;

        /// <summary>
        /// Sets level info for manager
        /// </summary>
        /// <param name="levelInfo">string with all the level info in correct format</param>
        /// <param name="index">Level index in the pack</param>
        /// <param name="selectedPack">Pack containing the level</param>
        /// <param name="selectedCategory">Category containing the pack</param>
        public void setLevel(string levelInfo, int index, LevelPack selectedPack, PackCategory selectedCategory)
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

        /// <summary>
        /// Initializes everything on the level based on level selected
        /// </summary>
        private void initLevel()
        {
            //Creates map and loads level
            Logic.Map map = new Logic.Map();
            map.loadLevel(level);

            //Level name and size
            levelText.text = "Level " + map.getNLevel();
            levelText.color = category.categoryColor;
            boardSizeText.text = map.getLevelWidth().ToString() + "x" + map.getLevelHeight();

            Logic.GameState state = GameManager.Instance.getState();

            Logic.LvlPack logicPack = state.getCategoryByName(category.categoryName).getPackByName(pack.packName);
            Logic.Level logicLevel = logicPack.levels[currentLvlIndex];
            
            //If level was already completed, set record text and icon
            if (logicLevel.completed)
            {
                if (logicLevel.record == map.getNPipes())
                    endImage.enableStar(true);
                else endImage.enableCheck(true);

                recordText.text = "best: " + logicLevel.record.ToString();
            }
            
            //Next and previous level button
            if (currentLvlIndex == 0)
            {
                previousLevelButton.interactable = false;
            }
            if (currentLvlIndex >= logicPack.levels.Length - 1 || 
                state.getCategoryByName(category.categoryName).getPackByName(pack.packName).levels[currentLvlIndex + 1].locked) {          
                nextLevelButton.interactable = false;
            }

            //Set up board
            board.setForGame(map, GameManager.Instance.GetColorTheme().colors, category.categoryColor);

            levelDonePanel.SetActive(false);
        }

        /// <summary>
        /// Called when reset button is pressed
        /// </summary>
        public void resetLevel()
        {
            board.resetBoard();

            levelDonePanel.SetActive(false);
        }

        /// <summary>
        /// Callled when level is finished
        /// </summary>
        /// <param name="steps">steps taken to solve level, at least the number of pipes in the level</param>
        public void levelDone(int steps)
        {
            //Notify game manager to update state
            bool perfect = steps == board.getNPipes();
            GameManager.Instance.levelFinished(steps, perfect);

            Logic.GameState state = GameManager.Instance.getState();
            Logic.LvlPack logicPack = state.getCategoryByName(category.categoryName).getPackByName(pack.packName);
            Logic.Level logicLevel = logicPack.levels[currentLvlIndex];

            //Set level name icon
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

            //Set record text
            recordText.text = "best: " + logicLevel.record.ToString();

            //Set up level finished panel
            if (currentLvlIndex >= logicPack.levels.Length - 1)
            {
                completeText.text = "Congratulations!";
                doneInStepsText.text = "You reached the end of the " + pack.packName;
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

            AdsManager.Instance.playInterstitialAd(adFinished);
        }

        /// <summary>
        /// Called when interstitial ad have finished
        /// </summary>
        private void adFinished()
        {
            levelDonePanel.SetActive(true);
            SoundManager.Instance.playSound(SoundManager.Sound.Flow);
        }

        /// <summary>
        /// Called when level done panel is closed
        /// </summary>
        public void continueLevel()
        {
            board.enableInput();
            levelDonePanel.SetActive(false);
        }

        /// <summary>
        /// Applies hint and notifies game manager and hint manager
        /// </summary>
        public void applyHint()
        {
            if (GameManager.Instance.getNHints() <= 0 || board.isCompleted())
                return;

            GameManager.Instance.hintWasted();

            hintShower.hintsChanged((int)GameManager.Instance.getNHints());

            //Calls board to use hint
            board.nextHint();
        }

        /// <summary>
        /// Called when rewarded ad finishes to add hint
        /// </summary>
        public void addHint()
        {
            GameManager.Instance.addHint();
            hintShower.hintsChanged((int)GameManager.Instance.getNHints());
        }

        /// <summary>
        /// Calls the game manager to go to the choose level scene
        /// </summary>
        public void exitLevel()
        {
            GameManager.Instance.exitLevel();
        }

        /// <summary>
        /// Loads next level
        /// </summary>
        public void nextLevel()
        {
            int nextLevel = currentLvlIndex + 1;

            bool lockedNextLevel = GameManager.Instance.getState().getCategoryByName(category.categoryName).getPackByName(pack.packName).levels[nextLevel].locked;
            if (!lockedNextLevel)
            {
                SoundManager.Instance.playSound(SoundManager.Sound.Forward);
                GameManager.Instance.selectLevel(nextLevel);
            }
        }

        /// <summary>
        /// Loads previous level
        /// </summary>
        public void previousLevel()
        {
            SoundManager.Instance.playSound(SoundManager.Sound.Back);
            GameManager.Instance.selectLevel(currentLvlIndex - 1);
        }

        /// <summary>
        /// Notifies game manager to go to select category menu
        /// </summary>
        public void goToSelectCategoryMenu()
        {
            GameManager.Instance.gotoSelectCategoryMenu();
        }
    }
}