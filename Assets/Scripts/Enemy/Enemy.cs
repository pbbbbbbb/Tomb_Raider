using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected private string enemyName;
    [SerializeField] protected private float moveSpeed;
    protected private float healthPoint;
    [SerializeField] protected private float maxHealthPoint;
    protected private Transform target;//target is our player;
    [SerializeField] protected private float attackRange;
    protected private SpriteRenderer sp;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        healthPoint = maxHealthPoint;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sp = GetComponent<SpriteRenderer>();
    }

    protected virtual void Move()
    {
        if (Vector2.Distance(transform.position, target.position) < attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    protected private void TurnDirection()
    {
        if (transform.position.x > target.position.x)
        {
            sp.flipX = true;
            /*Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;*/
        }
        else
        {
            sp.flipX = false;
            /*Vector3 localTemp = transform.localScale;
            localTemp.x *= -1;
            transform.localScale = localTemp;*/
        }
    }
    // Update is called once per frame

    protected virtual void Attack()
    {

    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        TurnDirection();
        if (healthPoint <= 0)
        {
            Death();//实际不要这么写 消耗性能
        }
        Attack();
    }

    protected void FixedUpdate()
    {
        Move();
    }
}
