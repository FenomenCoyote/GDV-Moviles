using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            levelMngr.initLevel(categories[0].levelPacks[0], 0);
        }

        private void setInfo(LevelManager lm)
        {
            levelMngr = lm;
        }

        public uint getNHints() { return hints; }

        public PackCategory[] getPackCategories() { return categories; }
    }
}
