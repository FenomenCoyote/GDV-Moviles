using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

namespace flow
{
    public class ProgressSaverLoader
    {
        SHA256 sha256;

        private string pepper = "x49378jhx10ex456e12342e21axew";

        public ProgressSaverLoader()
        {
            sha256 = SHA256.Create();
        }


        public void saveProgress(Logic.GameState stateToSave, string saveFile)
        {
            Logic.Save save = new Logic.Save();
            save.gameState = stateToSave;

            string serializedState = JsonUtility.ToJson(save.gameState);
            //serializedState += pepper;

            save.hashCode = Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(serializedState)));

            string serializedSave = JsonUtility.ToJson(save);
            
            byte[] json = Encoding.UTF8.GetBytes(serializedSave);

            FileStream file = File.Open(saveFile, FileMode.Create);
            file.Write(json, 0, json.Length);
            file.Close();
        }


        public Logic.GameState loadProgress(string saveFile)
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
                return null;
            }

            //Comprobacion de hash
            byte[] readState = Encoding.UTF8.GetBytes(JsonUtility.ToJson(save.gameState));
            byte[] hash = sha256.ComputeHash(readState);

            string readHash = Encoding.UTF8.GetString(hash);

            string actualHash = save.hashCode;

            if (string.Compare(readHash, actualHash) != 0)
            {
                Debug.Log("Detectado intento de hack");
                return null;
            }

            else return save.gameState;
        }

    }
}
