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

        private uint hints;

        [SerializeField]
        private Scene scene;

        [SerializeField]
        private PackCategory[] categories;

        [SerializeField]
        private LevelManager levelMngr;

        public static GameManager Instance { get; private set; }

        private LevelPack selectedPack;
        private PackCategory selectedCategory;

        private string selectedLevel;
        private int levelPanelNumber;

        /*DEBUG*/
        [SerializeField] private LevelPack DEBUGPack;
        [SerializeField] private PackCategory DEBUGCategory;

        [SerializeField] private int DEBUGLevel;
        [SerializeField] private int DEBUGPanelNumber;

        private void Awake()
        {
            if (Instance != null)
            {
                Instance.setInfo(levelMngr);
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
                }
                if (levelMngr != null)
                {
                    if (levelMngr != null)
                    {
                        levelMngr.setLevel(selectedLevel, selectedPack);
                    }
                }
            }
#endif

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void setInfo(LevelManager lm)
        {
            levelMngr = lm;
            if (levelMngr != null)
            {
                levelMngr.setLevel(selectedLevel, selectedPack);
            }
        }

        public uint getNHints() { return hints; }

        public PackCategory[] getPackCategories() { return categories; }

        public void selectPack(LevelPack pack, PackCategory category)
        {
            selectedPack = pack;
            selectedCategory = category;
            SceneManager.LoadScene((int)Scene.ChooseLevel);
        }

        public void gotoSelectCategoryMenu()
        {
            SceneManager.LoadScene((int)Scene.ChoosePack);
        }

        public void setLevel(string levelInfo, int nPanel = 0)
        {
            levelPanelNumber = 0;
            selectedLevel = levelInfo;
            SceneManager.LoadScene((int)Scene.Level);
        }

        public LevelPack getLevelPack() { return selectedPack; }
        public PackCategory getPackCategory() { return selectedCategory; }

    }
}
