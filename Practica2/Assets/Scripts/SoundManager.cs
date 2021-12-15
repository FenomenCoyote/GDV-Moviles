using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public class SoundManager : MonoBehaviour
    {
        public enum Sound
        {
            Back, Forward, Leak, Flow
        }

        [SerializeField] private AudioSource back;
        [SerializeField] private AudioSource forward;
        [SerializeField] private AudioSource leak;
        [SerializeField] private AudioSource flow;

#if UNITY_EDITOR
        private void Start()
        {
            if(back == null || forward == null || leak == null || flow == null)
            {
                Debug.Log("Sound Manager has null variables");
            }
        }
#endif

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