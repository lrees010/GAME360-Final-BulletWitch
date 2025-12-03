using System;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{

    // scrolls the background and fades between them wow

    [SerializeField] private RawImage _img;
    [SerializeField] private CanvasGroup _group; //allows adjusting opacity
    [SerializeField] private float speed;
    [SerializeField] private float fadeDuration = 0.5f;

    public Texture Forest;
    public Texture Cave;
    public Texture Clearing;
    public Texture Lake;

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(0,speed) * Time.deltaTime,_img.uvRect.size);
    }

    void Start()
    {
        EventManager.Subscribe("OnLevelChanged", ChangeBG); //i.e "Forest" "Cave"

        ChangeBG("Clearing");
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnLevelChanged", ChangeBG);
    }



    private void ChangeBG(object data)
    {
        string LevelName = data.ToString();

        Texture nextTex = null;

        switch (LevelName) //Maybe change this
        {
            case "Clearing":
                nextTex = Clearing;
                break;
            case "Forest":
                nextTex = Forest;
                break;

            case "Cave":
                nextTex = Cave;
                break;

            case "Lake":
                nextTex = Lake;
                break;
        }
        bool skipFadeOut = (data.ToString() == "Clearing"); //skip fade out if starter level

        StartCoroutine(FadeTo(nextTex, skipFadeOut));
    }

    //coroutine no halting
    private System.Collections.IEnumerator FadeTo(Texture nextTex,bool skipFadeOut)
    {
        float time = 0f;

        if (!skipFadeOut)
        {
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                _group.alpha = 1f - (time / fadeDuration);
                yield return null;
            }
        }



        _img.texture = nextTex;

        time = 0f;
        while (time<fadeDuration)
        {
            time += Time.deltaTime;
            _group.alpha = (time / fadeDuration);
            yield return null;
        }
    }
}
