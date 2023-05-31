using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGlobe : MonoBehaviour
{
    public int expGrantAmount;
    private Rigidbody2D rb;
    private Vector2 force;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        force.x = Random.value;
        force.y = Random.value;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.GainExp(expGrantAmount);
            Destroy(gameObject);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.GetComponent<Player>())
    //    {
    //        Player player = collision.gameObject.GetComponent<Player>();
    //        player.GainExp(expGrantAmount);
    //        Destroy(gameObject);
    //    }
    //}
}
