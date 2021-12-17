using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class SoundManager : MonoBehaviour
    {
        //Enum with the type of sounds
        public enum Sound
        {
            Back, Forward, Leak, Flow
        }

        //Instance of the SoundManager
        public static SoundManager Instance { get; private set; }

        //AudioSurces to play each type of sound
        [SerializeField] private AudioSource back;
        [SerializeField] private AudioSource forward;
        [SerializeField] private AudioSource leak;
        [SerializeField] private AudioSource flow;

        private void Awake()
        {
            //We make sure that we have only one instance of the SoundManager between scenes
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_EDITOR
        private void Start()
        {
            if(back == null || forward == null || leak == null || flow == null)
            {
                Debug.Log("Sound Manager has null variables");
            }
        }
#endif
        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <param name="s">Sound to play</param>
        public void playSound(Sound s)
        {
            switch (s)
            {
                case Sound.Back:
                    back.Play();
                    break;
                case Sound.Forward:
                    forward.Play();
                    break;
                case Sound.Leak:
                    leak.Play();
                    break;
                case Sound.Flow:
                    flow.Play();
                    break;
            }

        }
    }
}