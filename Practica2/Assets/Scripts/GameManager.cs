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

        /*[SerializeField]*/ private LevelPack selectedPack;
        /*[SerializeField]*/ private PackCategory selectedCategory;

        private string selectedLevel;
        private int levelPanelNumber;

        private void Awake()
        {
            if (Instance != null)
            {
                Instance.setInfo(levelMngr);
                Destroy(gameObject);
                return;
            }

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
