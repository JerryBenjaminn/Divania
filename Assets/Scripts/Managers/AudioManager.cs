using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip bossBattleMusic;

    [SerializeField] public List<AudioData> audioDataList;

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
            if (audioSource.clip != data.clip)
            {
                audioSource.clip = data.clip;
            }
            audioSource.volume = data.volume;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
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
            if (backgroundMusicSource.clip != clip || !backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.clip = clip;
                backgroundMusicSource.loop = true;
            }
            backgroundMusicSource.volume = volume;
            if (!backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Background music clip is null");
        }
    }


    public void PlayBossBattleMusic(float volume = 0.1f)
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

    public void SetSFXVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
    }

    public void UpdateVolume(float sfxVolume, float bgMusicVolume)
    {
        // list of SFX names
        List<string> sfxNames = new List<string> { "PlayerJump", "PlayerMiss", "BossHurt", "EnemyHurt", "Fireball", "PlayerHurt", "PlayerMeleeAttack", "PlayerStep", "GhoulExplode", "BossBattleMusic"};

        foreach (var sfxName in sfxNames)
        {
            AudioData sfxData = audioDataList.Find(x => x.name == sfxName);
            if (sfxData != null)
            {
                sfxData.volume = sfxVolume;
            }
        }

        AudioData bgMusicData = audioDataList.Find(x => x.name == "BackgroundMusic");
        if (bgMusicData != null)
        {
            bgMusicData.volume = bgMusicVolume;
        }
    }

    public float GetSFXVolume()
    {
        return audioSource.volume;
    }

    public float GetMusicVolume()
    {
        return backgroundMusicSource.volume;
    }





    [System.Serializable]
    public class AudioData
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

}

