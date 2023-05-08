using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip bossBattleMusic;

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

    private void Start()
    {
        AudioClip backgroundMusic = audioDataList.Find(x => x.name == "BackgroundMusic")?.clip;
        float backgroundMusicVolume = audioDataList.Find(x => x.name == "BackgroundMusic")?.volume ?? 1f;
        PlayBackgroundMusic(backgroundMusic, backgroundMusicVolume);
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

    public void PlayBackgroundMusic(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.volume = volume;
            backgroundMusicSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music clip is null");
        }
    }

    public void PlayBossBattleMusic(float volume = 1f)
    {
        if (bossBattleMusic != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = bossBattleMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.volume = volume;
            backgroundMusicSource.Play();
        }
        else
        {
            Debug.LogWarning("Boss battle music clip is null");
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

