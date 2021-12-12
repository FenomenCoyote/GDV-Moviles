using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

namespace flow
{
    public class ProgressSaverLoader : MonoBehaviour
    {
        SHA256 sha256;
        private Logic.GameState state;

        [SerializeField] string saveFile;

        private string pepper = "x49378jhx10ex456e12342e21axew";

        public ProgressSaverLoader()
        {
            sha256 = SHA256.Create();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                loadProgress();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                saveProgress();
            }
        }

        public void init()
        {
            state = new Logic.GameState();
            if (!System.IO.File.Exists(saveFile))
            {
                state.init(GameManager.Instance.getPackCategories());
            }
            else loadProgress();
        }

        private void saveProgress()
        {
            Logic.Save save = new Logic.Save();
            save.gameState = state;

            string serializedState = JsonUtility.ToJson(save.gameState);
            //serializedState += pepper;

            save.hashCode = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(serializedState)));

            string serializedSave = JsonUtility.ToJson(save);

            byte[] json = Encoding.UTF8.GetBytes(serializedSave);

            FileStream file = File.Open(saveFile, FileMode.Create);
            file.Write(json, 0, json.Length);
            file.Close();
        }


        private void loadProgress()
        {
            string readSave = File.ReadAllText(saveFile, Encoding.UTF8);

            Logic.Save save;

            try
            {
                save = JsonUtility.FromJson<Logic.Save>(readSave);
            }
            catch (Exception e)
            {
                Debug.Log("Detectado intento de hack");
                return;
            }

            //Comprobacion de hash
            byte[] readState = Encoding.UTF8.GetBytes(JsonUtility.ToJson(save.gameState));
            byte[] hash = sha256.ComputeHash(readState);

            string readHash = Encoding.UTF8.GetString(hash);

            string actualHash = save.hashCode;

            if (string.Compare(readHash, actualHash) != 0)
            {
                Debug.Log("Detectado intento de hack");
                return;
            }

            state = save.gameState;
        }

        public void levelFinished(int steps, int lvlIndex, PackCategory selectedCategory, LevelPack selectedPack)
        {
            Logic.LvlPack pack = state.getCategoryByName(selectedCategory.categoryName).getPackByName(selectedPack.packName);
            Logic.Level level = pack.levels[lvlIndex];
            if (!level.completed)
            {
                level.completed = true;
                pack.completedLevels++;
                level.record = steps;
                //Desbloquear siguiente nivel
                if (lvlIndex < pack.levels.Length - 1)
                    pack.levels[lvlIndex + 1].locked = false;
            }
            else if (level.record > steps)
            {
                level.record = steps;
            }
            state.getCategoryByName(selectedCategory.categoryName).getPackByName(selectedPack.packName).levels[lvlIndex] = level;

            saveProgress();
        }

        public uint getNHints() { return state.nHints; }
        public Logic.GameState getState() { return state; }
    }
}
