using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();

        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.value = AudioManager.Instance.generalVolume;
            slider.onValueChanged.AddListener(delegate { Changey(); });
        }
    }

    void Changey()
    {
        
        AudioManager.Instance.generalVolume = slider.value;
        Debug.Log(AudioManager.Instance.generalVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
