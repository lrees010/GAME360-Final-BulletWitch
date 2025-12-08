using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour


{
    private AudioSource audioSource; //audio source for sound effects

    private AudioSource musicSource; //audio source for music

    // Singleton instance so we can trigger audio events from other scripts
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
            //if audiosource doesn't exist, create it
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource for sound effects
        audioSource.playOnAwake = false;
        audioSource.volume = generalVolume; // Adjust volume as needed

        //AudioSource for music, configure for music (looping)
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = generalVolume;
        musicSource.playOnAwake = false;
        musicSource.loop = true;


    }

    [Header("Settings")]
    public float generalVolume = 1f; //affects volume of everything

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

    [Header("Music")] //for music mp3s and wavs
    public AudioClip ClearingMusic;
    public AudioClip ForestMusic;
    public AudioClip CaveMusic;
    public AudioClip LakeMusic;
    public AudioClip BeachMusic;
    public AudioClip MountainMusic;
    public AudioClip VictoryMusic;






    private void Update()
    {
        if (GameManager.Instance.speedOfTime > 0) //if we change pitch while the TimeScale is at 0, it will sound bad
        {
            //set pitch of all audio to the speed of time, so when time slows, pitch slows too
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

    public void PlaySFX(AudioClip clip) //public method for playing SFX
    {
        if (clip != null && audioSource != null)
        {
            audioSource.volume = generalVolume;
            audioSource.PlayOneShot(clip);
        }
    }
    private float lastSound = 0f;
    public void PlayLimitedSFX(AudioClip clip) //public method for playing SFX within a rate, for sounds that happen frequently
    {
        if (Time.time - lastSound > 0.23f)
        {
            lastSound = Time.time;
            PlaySFX(clip);
        }
    }

    public void PlayMusic(AudioClip clip) //public method for playing music
    {
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

    public void PauseMusic(bool pause) //public method to pause or unpause music based on bool parameter
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

    public void PlayEnemyKilledSound() => PlaySFX(EnemyKilledSound); //public method to play the generic EnemyKilled sound

    
}
