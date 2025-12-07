using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();

        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.value = AudioManager.Instance.generalVolume;
            slider.onValueChanged.AddListener(delegate { Changey(); }); //trigger method when the slider moves
        }
    }

    void Changey()
    {
        
        AudioManager.Instance.generalVolume = slider.value; //set audiomanager general volume to the slider's new value
        Debug.Log(AudioManager.Instance.generalVolume);
    }

}
