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
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    [Header("Audio")]
    public AudioClip ShootSound; //this is where you put your mp3/wav files
    public AudioClip CoinSound;
    public AudioClip AchievementSound;
    public AudioClip EnemyKilledSound;

    private AudioSource audioSource;//Unity componenet

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource for sound effects
        audioSource.playOnAwake = false;
        audioSource.volume = 0.7f; // Adjust volume as needed


        EventManager.Subscribe("OnScoreChanged", PlayCoinSound);
        EventManager.Subscribe("OnEnemyKilled", PlayKilledSound);


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
            audioSource.PlayOneShot(clip);
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
