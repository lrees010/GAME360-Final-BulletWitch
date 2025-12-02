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


        EventManager.Subscribe("OnScoreChanged", PlayCoinSound);
        EventManager.Subscribe("OnEnemyKilled", PlayKilledSound);
    }

    [Header("Settings")]
    public float generalVolume = 1f;

    [Header("Audio")]
    public AudioClip ShootSound; //this is where you put your mp3/wav files
    public AudioClip CoinSound;
    public AudioClip AchievementSound;
    public AudioClip EnemyKilledSound;

    public AudioClip ClearingMusic;
    public AudioClip ForestMusic;

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
        EventManager.Unsubscribe("OnScoreChanged", PlayCoinSound);
        EventManager.Unsubscribe("OnEnemyKilled", PlayKilledSound);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.volume = generalVolume;
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        Debug.Log("musico");
        if (clip != null && musicSource != null)
        {
            musicSource.volume = generalVolume;
            musicSource.clip = clip;
            musicSource.Play();
        }
    }



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
    }

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

    void PlayCoinSound()
    {
        PlaySFX(CoinSound);
    }


    //non observer sounds

    public void PlayShootSound() => PlaySFX(ShootSound);

    public void PlayAchievementSound() => PlaySFX(AchievementSound);

    public void PlayKilledSound() => PlaySFX(EnemyKilledSound);
}
