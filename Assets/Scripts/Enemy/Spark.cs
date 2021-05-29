using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float shotSpeed;

    public GameObject spark;
    [SerializeField] GameObject destroy;

    private float lifeTimer;
    [SerializeField] private float maxLife = 2.0f;


    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, shotSpeed * Time.deltaTime);

        lifeTimer += Time.deltaTime;
        if(lifeTimer >= maxLife)
        {
            //Instantiate(spark, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //other.GetComponentInChildren<PlayerHealthBar>().hp -= 35;
            //Instantiate(spark, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
