using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WaveBullet : MonoBehaviour
{
    [Header("WaveBullet Settings")]
    public GameObject bloom;
    private float speed = 10f;
    public float lifetime = 3f;
    private List<Rigidbody2D> rbList = new List<Rigidbody2D>();


    private Rigidbody2D rb;

    private void Update()
    {
        if (rbList.Count > 0)
        {
            for(int i = 0; i < rbList.Count; i++)
            {
                float velocity = Mathf.Sin((Time.time+i) * 10) * 4f;

                if (rbList[i] == null)
                {
                    rbList.Clear();
                    Destroy(gameObject);
                }
                rbList[i].AddForce(new Vector2(velocity, 0));
            }
        }
    }
    private void send(GameObject bullet,Vector2 direction, bool waveIt)
    {
        
        bullet.SetActive(true);
        bullet.transform.position = gameObject.transform.position;
        rb = bullet.GetComponent<Rigidbody2D>();
        if (waveIt == true)
        {
            rbList.Add(rb);
        }
        rb.linearVelocity = direction * speed;
        Destroy(bullet, lifetime);
    }
    private void Start()
    {
        AudioManager.Instance.PlayShootSound();
        //send(Instantiate(bloom), Vector2.up, true);
        send(Instantiate(bloom), (Vector2.up+new Vector2(-0.5f,0f)), true);
        send(Instantiate(bloom), (Vector2.up + new Vector2(0.5f, 0f)), true);

        send(Instantiate(bloom), Vector2.up, false);

        // Destroy WaveBullet after lifetime
        Destroy(gameObject, lifetime);
    }
}