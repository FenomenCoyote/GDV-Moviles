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

        private static GameManager instance;
        public static GameManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance != null) {
                getInfo(instance);
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            //TODO: temporal
            levelMngr.initLevel(categories[0].levelPacks[0], 0);
        }

        private void getInfo(GameManager gm)
        {
            hints = gm.getNHints();
            categories = gm.getPackCategories();
        }

        public uint getNHints() { return hints; }
        public PackCategory[] getPackCategories() { return categories; }
    }
}
