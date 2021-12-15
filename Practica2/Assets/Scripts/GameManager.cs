using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flow
{
    public class GameManager : MonoBehaviour
    {
        public enum Scene
        {
            ChoosePack = 0,
            ChooseLevel = 1,
            Level = 2,
        }

        ProgressSaverLoader saver;

        [SerializeField]
        private Scene scene;

        [SerializeField]
        private PackCategory[] categories;

        [SerializeField]
        private ColorTheme theme;

        [SerializeField]
        private LevelManager levelMngr;

        public static GameManager Instance { get; private set; }

        public SoundManager soundManager { get; private set; }

        private LevelPack selectedPack;
        private PackCategory selectedCategory;

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
            if (Instance != null)
            {
                Instance.setInfo(levelMngr, categories, scene);
                Destroy(gameObject);
                return;
            }

#if UNITY_EDITOR
            if(scene != Scene.ChoosePack)
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

            saver = GetComponent<ProgressSaverLoader>();
            saver.init();
            soundManager = GetComponent<SoundManager>();
        }

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

        public void addHint()
        {
            //TODO
        }

        public void removeAdds()
        {
            //TODO
        }

        public void hintWasted() { saver.hintWasted(); }

        public uint getNHints() { return saver.getNHints(); }

        public PackCategory[] getPackCategories() { 
            return categories; 
        }

        public ColorTheme GetColorTheme()
        {
            return theme;
        }

        public void selectPack(LevelPack pack, PackCategory category)
        {
            selectedPack = pack;
            selectedCategory = category;
            soundManager.playSound(SoundManager.Sound.Forward);
            SceneManager.LoadScene((int)Scene.ChooseLevel);
        }

        public void gotoSelectCategoryMenu()
        {
            soundManager.playSound(SoundManager.Sound.Back);
            SceneManager.LoadScene((int)Scene.ChoosePack);
        }

        public void setLevel(string levelInfo, int index, int nPanel = 0)
        {
            selectedLevel = levelInfo;
            lvlIndex = index;
            soundManager.playSound(SoundManager.Sound.Forward);
            SceneManager.LoadScene((int)Scene.Level);
        }

        public void exitLevel()
        {
            levelMngr = null;
            soundManager.playSound(SoundManager.Sound.Back);
            SceneManager.LoadScene((int)Scene.ChooseLevel);
        }

        public void selectLevel(int lvl)
        {
            string[] aux = selectedPack.levels.ToString().Split('\n');
            
            if (lvl>=0 && lvl<aux.Length-1) //-1 porque el tamaño del level pack es de 151
            {
                selectedLevel = aux[lvl];
                lvlIndex = lvl;
                levelMngr.setLevel(selectedLevel, lvlIndex, selectedPack, selectedCategory);
                SceneManager.LoadScene((int)Scene.Level);
            }           
        }
        public void levelFinished(int steps, bool perfect)
        {
            saver.levelFinished(steps, perfect, lvlIndex, selectedCategory, selectedPack);
        }

        public Logic.GameState getState() { return saver.getState(); }

        public LevelPack getLevelPack() { return selectedPack; }
        public PackCategory getPackCategory() { return selectedCategory; }
        public int getLevelPackSize() { return selectedPack.levels.ToString().Split('\n').Length; }

    }
}
