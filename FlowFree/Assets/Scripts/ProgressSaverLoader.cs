using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

namespace flow
{
    public class ProgressSaverLoader : MonoBehaviour
    {
        private SHA256 sha256;
        private Logic.GameState state;

        [SerializeField] string saveFile; //json file

        /// <summary>
        /// Constructor. It creates a sha256 object
        /// </summary>
        public ProgressSaverLoader()
        {
            sha256 = SHA256.Create();
        }

        /// <summary>
        /// Initializes the save and load system
        /// </summary>
        public void init()
        {
            //We create the route of the save file
            saveFile = Application.persistentDataPath + "/" + saveFile;

            //We initialize the game state
            state = new Logic.GameState();

            //If the save file doesn't exist we initalize the game state,
            //in other case we load the current progress
            if (!System.IO.File.Exists(saveFile))
            {
                state.init(GameManager.Instance.getPackCategories());
            }
            else loadProgress();
        }

        /// <summary>
        /// Saves the current progress of the game
        /// </summary>
        private void saveProgress()
        {
            Logic.Save save = new Logic.Save();
            save.gameState = state;

            //We create a hash 
            string serializedState = JsonUtility.ToJson(save.gameState);

            save.hashCode = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(serializedState)));

            string serializedSave = JsonUtility.ToJson(save);

            byte[] json = Encoding.UTF8.GetBytes(serializedSave);

            //We write on the save file
            FileStream file = File.Open(saveFile, FileMode.Create);
            file.Write(json, 0, json.Length);
            file.Close();
        }

        /// <summary>
        /// Load the progress from the save file
        /// </summary>
        private void loadProgress()
        {
            //We read the save file
            string readSave = File.ReadAllText(saveFile, Encoding.UTF8);

            Logic.Save save;

            try
            {
                save = JsonUtility.FromJson<Logic.Save>(readSave);
            }
            catch (Exception e)
            {
                Debug.Log("Hack attempt detected");
                state.init(GameManager.Instance.getPackCategories());
                return;
            }

            //Hash check. We compare the read hash and the actual hash
            byte[] readState = Encoding.UTF8.GetBytes(JsonUtility.ToJson(save.gameState));
            byte[] hash = sha256.ComputeHash(readState);

            string readHash = Encoding.UTF8.GetString(hash);

            string actualHash = save.hashCode;

            if (string.Compare(readHash, actualHash) != 0)
            {
                Debug.Log("Hack attempt detected");
                state.init(GameManager.Instance.getPackCategories());
                return;
            }

            state = save.gameState;
        }

        public void levelFinished(int steps, bool perfect, int lvlIndex, PackCategory selectedCategory, LevelPack selectedPack)
        {
            Logic.Category category = state.getCategoryByName(selectedCategory.categoryName);
            Logic.LvlPack pack = category.getPackByName(selectedPack.packName);
            Logic.Level level = pack.levels[lvlIndex];

            if (!level.completed)
            {
                level.completed = true;
                pack.completedLevels++;
                level.record = steps;
                //Unlock tehe next level
                if (lvlIndex < pack.levels.Length - 1)
                    pack.levels[lvlIndex + 1].locked = false;
                if (perfect)
                {
                    pack.perfectLevels++;
                }
            }
            else if (level.record > steps)
            {
                level.record = steps;
            }

            if(pack.completedLevels == pack.levels.Length)
            {
                category.completedPacks++;
                if (pack.perfectLevels == pack.levels.Length)
                    category.perfectPacks++;
            }

            state.getCategoryByName(selectedCategory.categoryName).getPackByName(selectedPack.packName).levels[lvlIndex] = level;

            saveProgress();
        }

        /// <summary>
        /// Spend one hint and save it
        /// </summary>
        public void hintWasted()
        {
            state.nHints--;
            saveProgress();
        }

        /// <summary>
        /// Add one hint and save it
        /// </summary>
        public void addHint()
        {
            state.nHints++;
            saveProgress();
        }

        /// <summary>
        /// Returns the number of hints
        /// </summary>
        /// <returns></returns>
        public uint getNHints() { return state.nHints; }

        /// <summary>
        /// Returns the game state
        /// </summary>
        /// <returns></returns>
        public Logic.GameState getState() { return state; }
    }
}
