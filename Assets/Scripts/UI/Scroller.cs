using System;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{

    // scrolls the background and fades between them wow

    [SerializeField] private RawImage _img;
    [SerializeField] private CanvasGroup _group; //allows adjusting alpha
    [SerializeField] private float speed;
    [SerializeField] private float fadeDuration = 0.5f;

    public Texture Forest;
    public Texture Cave;
    public Texture Clearing;
    public Texture Lake;
    public Texture Beach;
    public Texture Mountain;


    void Update()
    {
        //scroll the image using UVs
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(0,0.3f) * Time.deltaTime,_img.uvRect.size);
    }

    void Start()
    {
        EventManager.Subscribe("OnLevelChanged", ChangeBG); //when level changes, we get a string of the new level name i.e "Forest" "Cave"

        ChangeBG("Clearing"); //first background
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnLevelChanged", ChangeBG);
    }



    private void ChangeBG(object data)
    {
        string LevelName = data.ToString();

        Texture nextTex = null;

        switch (LevelName) //use different textures based on name of level
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

            case "Beach":
                nextTex = Beach;
                break;

            case "Mountain":
                nextTex = Mountain;
                break;
        }
        bool skipFadeOut = (data.ToString() == "Clearing"); //skip fade out if starter level


        StartCoroutine(FadeTo(nextTex, skipFadeOut));
    }

    //coroutine so there is no halting
    private System.Collections.IEnumerator FadeTo(Texture nextTex,bool skipFadeOut)
    {
        //fades to black, then to another texture. (skips first fade based on bool skipFadeOut)
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
