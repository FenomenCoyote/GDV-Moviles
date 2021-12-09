using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace flow
{
    public class ProgressSaverLoader : MonoBehaviour
    {
        private Logic.GameState gameState;

        SHA256 sha256;

        [SerializeField] string saveFile;

        // Start is called before the first frame update
        void Start()
        {
            sha256 = SHA256.Create();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void saveProgress()
        {
            byte[] json = Encoding.UTF8.GetBytes(JsonUtility.ToJson(gameState));
            byte[] hash = sha256.ComputeHash(json);

            gameState.hashCode = hash;

            json = Encoding.UTF8.GetBytes(JsonUtility.ToJson(gameState));

            FileStream file = File.Open(saveFile, FileMode.Create);
            file.Write(json, 0, json.Length);
            file.Close();
        }

        public void loadProgress()
        {

            FileStream file = File.Open(saveFile, FileMode.Create);
            byte[] readState = new byte[file.Length];
            file.Read(readState, 0, (int)file.Length);
            file.Close();

            gameState = JsonUtility.FromJson<Logic.GameState>(Encoding.UTF8.GetString(readState));

            //TODO Está mal porque para encriptar habría que hacerlos sin el hash y al cargar comprobar sin usar el hash

        }

    }
}
