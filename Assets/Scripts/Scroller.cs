using System;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{

    // scrolls the background wow

    [SerializeField] private RawImage _img;
    [SerializeField] private float speed;

    public Texture Forest;
    public Texture Cave;

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(0,speed) * Time.deltaTime,_img.uvRect.size);
    }

    void Start()
    {
        EventManager.Subscribe("OnLevelChanged", ChangeBG); //i.e "Forest" "Cave"
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnLevelChanged", ChangeBG);
    }



    private void ChangeBG(object data)
    {
        string LevelName = data.ToString();


        switch (LevelName) //Maybe change this
        {
            case "Forest":
                _img.texture = Forest;
                break;

            case "Cave":
                _img.texture = Cave;
                break;
        }
    }
}
