using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<AudioData> audioDataList;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayAudioClip(string clipName)
    {
        AudioData data = audioDataList.Find(x => x.name == clipName);
        if (data != null && data.clip != null)
        {
            audioSource.PlayOneShot(data.clip, data.volume);
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clipName);
        }
    }


    [System.Serializable]
    public class AudioData
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

}

