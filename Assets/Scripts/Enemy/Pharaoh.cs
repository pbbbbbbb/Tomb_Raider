using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pharaoh : Enemy
{
    private float shootRate = 2.0f;
    private float shootTimer;
    //[SerializeField] float bossAttackRange = 10f;
    [SerializeField] private float minX, maxX, minY, maxY;//最终boss格子的那个尺寸！
    [SerializeField] float moveRate = 5.0f;
    private float moveTimer;

    public GameObject projectile;

    protected override void Attack()
    {
        base.Attack();
        shootTimer += Time.deltaTime;
        if (shootTimer > shootRate)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            shootTimer = 0;
        }
            
    }

    protected override void Move()
    {
        RandomMove();
    }

    private void RandomMove()// random show in the room
    {
        moveTimer += Time.deltaTime;
        if (moveTimer > moveRate)
        {
            transform.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            moveTimer = 0;
        }
    }


}
