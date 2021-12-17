using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

namespace flow
{
    public class GameManager : MonoBehaviour
    {
        //Game scenes
        public enum Scene
        {
            ChoosePack = 0,
            ChooseLevel = 1,
            Level = 2,
        }

        //Save and load system
        private ProgressSaverLoader saver;

        [SerializeField]
        private Scene scene;

        [SerializeField]
        private PackCategory[] categories;

        [SerializeField]
        private ColorTheme theme;

        [SerializeField]
        private LevelManager levelMngr;

        //Instance of the GameManager
        public static GameManager Instance { get; private set; }

        //Selected level pack and pack category
        private LevelPack selectedPack;
        private PackCategory selectedCategory;

        //Level info
        private string selectedLevel;
        private int lvlIndex;

        /*DEBUG*/
        [SerializeField] private LevelPack DEBUGPack;
        [SerializeField] private PackCategory DEBUGCategory;

        [SerializeField] private int DEBUGLevel;
        [SerializeField] private int DEBUGPanelNumber;
        [SerializeField] private int DEBUGLevelIndex;

        private void Awake()
        {
            //We make sure that we have only one instance of the GameManager between scenes
            //and we pass the GameManager information
            if (Instance != null)
            {
                Instance.setInfo(levelMngr, categories, scene);
                Destroy(gameObject);
                return;
            }

            //DEBUG. Just for loading the level we want
#if UNITY_EDITOR
            if (scene != Scene.ChoosePack)
            {
                if (selectedPack == null) selectedPack = DEBUGPack;
                if (selectedCategory == null) selectedCategory = DEBUGCategory;
                if (selectedLevel == null)
                {
                    string[] aux = selectedPack.levels.ToString().Split('\n');
                    selectedLevel = aux[DEBUGLevel + (DEBUGPanelNumber * 30)];
                    lvlIndex = DEBUGLevelIndex;
                }
                if (levelMngr != null)
                {
                    if (levelMngr != null)
                    {
                        levelMngr.setLevel(selectedLevel, lvlIndex, selectedPack, selectedCategory);
                    }
                }
            }
#endif

            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            //We initialize the save and load system
            saver = GetComponent<ProgressSaverLoader>();
            saver.init();
        }

        /// <summary>
        /// Sets the game manager information
        /// </summary>
        /// <param name="lm">LevelManager</param>
        /// <param name="categories">Pack categories array</param>
        /// <param name="scene">Current scene</param>
        private void setInfo(LevelManager lm, PackCategory[] categories, Scene scene)
        {
            levelMngr = lm;
            this.categories = categories;
            this.scene = scene;

            if (levelMngr != null)
            {
                levelMngr.setLevel(selectedLevel, lvlIndex, selectedPack,selectedCategory);
            }
        }

        /// <summary>
        /// Add one hint and save the new number of hints
        /// </summary>
        public void addHint()
        {
            saver.addHint();
        }

        public void removeAdds()
        {
            //TODO
        }

        /// <summary>
        /// Spend one hint and save the new number of hints
        /// </summary>
        public void hintWasted() { saver.hintWasted(); }

        /// <summary>
        /// Returns the current number of hints
        /// </summary>
        /// <returns></returns>
        public uint getNHints() { return saver.getNHints(); }

        /// <summary>
        /// Returns an array that contains all the pack categories
        /// </summary>
        /// <returns></returns>
        public PackCategory[] getPackCategories() {  return categories; }
           
        /// <summary>
        /// Returns the color theme
        /// </summary>
        /// <returns></returns>
        public ColorTheme GetColorTheme(){ return theme;}

        /// <summary>
        /// Sets the selected level pack and pack category and goes to the choose level scene
        /// </summary>
        /// <param name="pack">Level pack</param>
        /// <param name="category">Pack category</param>
        public void selectPack(LevelPack pack, PackCategory category)
        {
            selectedPack = pack;
            selectedCategory = category;
            SoundManager.Instance.playSound(SoundManager.Sound.Forward);
            SceneManager.LoadScene((int)Scene.ChooseLevel);
        }

        /// <summary>
        /// Goes to the choose pack scene
        /// </summary>
        public void gotoSelectCategoryMenu()
        {
            Advertisement.Banner.Hide();
            SoundManager.Instance.playSound(SoundManager.Sound.Back);
            SceneManager.LoadScene((int)Scene.ChoosePack);
        }

        /// <summary>
        /// Goes to the level from a LevelButton
        /// </summary>
        /// <param name="levelInfo">Configuration of the level</param>
        /// <param name="index">Index of the levels array</param>
        public void setLevel(string levelInfo, int index)

        {
            selectedLevel = levelInfo;
            lvlIndex = index;
            SoundManager.Instance.playSound(SoundManager.Sound.Forward);
            SceneManager.LoadScene((int)Scene.Level);
        }

        /// <summary>
        /// Goes to the choose level scene
        /// </summary>
        public void exitLevel()
        {
            levelMngr = null;
            SoundManager.Instance.playSound(SoundManager.Sound.Back);
            SceneManager.LoadScene((int)Scene.ChooseLevel);
        }

        /// <summary>
        /// Goes to a level. It is used to go to the next or previous level in the game scene
        /// </summary>
        /// <param name="lvl">The level we want to go</param>
        public void selectLevel(int lvl)
        {
            string[] aux = selectedPack.levels.ToString().Split('\n');
            
            if (lvl>=0 && lvl<aux.Length-1) //-1 because the level pack size is 151
            {
                selectedLevel = aux[lvl];
                lvlIndex = lvl;
                levelMngr.setLevel(selectedLevel, lvlIndex, selectedPack, selectedCategory);
                SceneManager.LoadScene((int)Scene.Level);
            }           
        }
        /// <summary>
        /// Saves the level information when we finish it 
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="perfect"></param>
        public void levelFinished(int steps, bool perfect)
        {
            saver.levelFinished(steps, perfect, lvlIndex, selectedCategory, selectedPack);
        }

        /// <summary>
        /// Returns the state of the game
        /// </summary>
        /// <returns></returns>
        public Logic.GameState getState() { return saver.getState(); }

        /// <summary>
        /// Returns the selected level pack
        /// </summary>
        /// <returns></returns>
        public LevelPack getLevelPack() { return selectedPack; }
        /// <summary>
        /// Returns the selected pack category
        /// </summary>
        /// <returns></returns>
        public PackCategory getPackCategory() { return selectedCategory; }
    }
}
