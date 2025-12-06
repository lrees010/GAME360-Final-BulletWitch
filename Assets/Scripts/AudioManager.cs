using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour


{
    // Singleton instance
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        // Simple singleton - one per scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource for sound effects
        audioSource.playOnAwake = false;
        audioSource.volume = generalVolume; // Adjust volume as needed

        //musicsource
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = generalVolume;
        musicSource.playOnAwake = false;
        musicSource.loop = true;


        //EventManager.Subscribe("OnScoreChanged", PlayCoinSound);
    }

    [Header("Settings")]
    public float generalVolume = 1f;

    [Header("Audio")]
    public AudioClip ShootSound; //this is where you put your mp3/wav files
    public AudioClip BloomShootSound;
    public AudioClip CoinSound;
    public AudioClip AchievementSound;
    public AudioClip EnemyKilledSound;
    public AudioClip BombSound;
    public AudioClip ObsidianDamageSound;
    public AudioClip DieSound;
    public AudioClip WaveShootSound;
    public AudioClip HitSound;
    public AudioClip SpitterShootSound;
    public AudioClip BombPickupSound;
    public AudioClip LifePickupSound;
    public AudioClip ObsidianShootSound;
    public AudioClip GameoverSound;

    [Header("Music")]
    public AudioClip ClearingMusic;
    public AudioClip ForestMusic;
    public AudioClip CaveMusic;
    public AudioClip LakeMusic;
    public AudioClip BeachMusic;
    public AudioClip MountainMusic;
    public AudioClip VictoryMusic;

    private AudioSource audioSource;//Unity componenet

    private AudioSource musicSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void Update()
    {
        if (GameManager.Instance.speedOfTime > 0)
        {
            musicSource.pitch = GameManager.Instance.speedOfTime;
            audioSource.pitch = GameManager.Instance.speedOfTime;
        }

        musicSource.volume = generalVolume;
        audioSource.volume = generalVolume;

    }

    private void OnDestroy()
    {
        //EventManager.Unsubscribe("OnScoreChanged", PlayCoinSound);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.volume = generalVolume;
            audioSource.PlayOneShot(clip);
        }
    }
    private float lastSound = 0f;
    public void PlayLimitedSFX(AudioClip clip)
    {
        if (Time.time - lastSound > 0.23f)
        {
            lastSound = Time.time;
            PlaySFX(clip);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        Debug.Log("musico");
        if (clip != null && musicSource != null)
        {
            musicSource.volume = generalVolume;
            musicSource.clip = clip;
            musicSource.Play();
        }
    }


    /*
    public void ChangeMusic(string level)
    {
        switch (level)
        {
            default:
                PlayMusic(ClearingMusic);
                break;
            case "Clearing":
                PlayMusic(ClearingMusic);
                break;
            case "Forest":
                PlayMusic(ForestMusic);
                break;
        }
    }*/

    public void PauseMusic(bool pause)
    {
        if (pause==true)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.Play();
        }
    }




    //non observer sounds

    public void PlayEnemyKilledSound() => PlaySFX(EnemyKilledSound);

    
}
