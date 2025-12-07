using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WaveBullet : MonoBehaviour
{
    //Wave Bullet weapon, sends three bullets in three directions at a faster rate, and bullets move in a wave pattern 

    [Header("WaveBullet Settings")]
    public GameObject bloom;
    private float speed = 10f;
    public float lifetime = 3f;
    private List<Rigidbody2D> rbList = new List<Rigidbody2D>();


    private Rigidbody2D rb;

    private void FixedUpdate()
    {
        //makes the list of rigidbodies move in a wave pattern
        if (rbList.Count > 0)
        {
            for(int i = 0; i < rbList.Count; i++)
            {
                float velocity = Mathf.Sin((Time.time+i) * 10) * 4f; //move in a wave pattern using sine function

                if (rbList==null||rbList[i] == null || i > rbList.Count)
                {
                    rbList.Clear();
                    Destroy(gameObject);
                    return;
                }
                rbList[i].AddForce((new Vector2(velocity, 0))*9);
            }
        }
    }
    private void send(GameObject bullet,Vector2 direction, bool waveIt)
    {
        
        bullet.SetActive(true);
        bullet.transform.position = gameObject.transform.position; //move to proper location
        rb = bullet.GetComponent<Rigidbody2D>();
        if (waveIt == true) //if we want to make the bullet move like a wave, add it to the rigidbody list
        {
            rbList.Add(rb);
        }
        rb.linearVelocity = direction * speed; //move bullet in given direction
        Destroy(bullet, lifetime); //destroy after some time
    }
    private void Start()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.WaveShootSound); //play wave bullet sound

        send(Instantiate(bloom), (Vector2.up+new Vector2(-0.5f,0f)), true); //fire bullet up and to the left
        send(Instantiate(bloom), (Vector2.up + new Vector2(0.5f, 0f)), true); //fire bullet up and to the right

        send(Instantiate(bloom), Vector2.up, false); //fire non-waving bullet straight up

        // Destroy WaveBullet after lifetime
        Destroy(gameObject, lifetime);
    }
}