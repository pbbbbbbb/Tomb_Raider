using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float bulletDistance;

    

    private Rigidbody2D rg2d;
    private Vector3 startPos;


    // Use this for initialization
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        rg2d.velocity = transform.right * speed;
        startPos = transform.position;
    }

    void Update()
    {
        float distance = (transform.position - startPos).sqrMagnitude;
        if (distance > bulletDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            Destroy(gameObject);
            //other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}