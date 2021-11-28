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

        private void Awake()
        {
            if (Instance != null) {
                setInfo(levelMngr);
                Destroy(this);
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            //TODO: temporal
            if (levelMngr)
            {
                levelMngr.initLevel(categories[0].levelPacks[0], 0);
                selectedPack = categories[0].levelPacks[0];
            }
        }

        private void setInfo(LevelManager lm)
        {
            levelMngr = lm;
        }

        public uint getNHints() { return hints; }

        public PackCategory[] getPackCategories() { return categories; }

        public void selectPack(LevelPack pack)
        {
            selectedPack = pack;
            SceneManager.LoadScene((int)Scene.ChooseLevel);
        }
    }
}
