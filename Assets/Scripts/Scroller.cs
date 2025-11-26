using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{

    // scrolls the background wow

    [SerializeField] private RawImage _img;
    [SerializeField] private float speed;
    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(0,speed) * Time.deltaTime,_img.uvRect.size);
    }
}
