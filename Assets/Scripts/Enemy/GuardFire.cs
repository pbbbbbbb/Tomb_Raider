using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFire : MonoBehaviour
{
    protected private Transform target;//target is our player;
    [SerializeField] float fireRate = 2.0f;
    private float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = 0;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //TurnDirection();
        fireTimer += Time.deltaTime;
        if (fireTimer > fireRate)
        {
            Attack();
            fireTimer = 0;
        }
    }

    /*protected private void TurnDirection()
    {
        if (transform.position.x > target.position.x)
        {
            Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;
        }
        else
        {
            Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;
        }
    }*/

    void Attack()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector2 difference = target.position - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        transform.GetChild(0).gameObject.SetActive(true);
    }



    
}
