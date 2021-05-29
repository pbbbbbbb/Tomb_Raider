using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    public Transform wayPoint1, wayPoint2;
    private Transform wayPointTarget;
    public float hurtLength;//MARKER 效果持续多久
    private float hurtCounter;//MARKER 相当于计数器
    private GameObject SwordPointG;//武器攻击
    [SerializeField] float moveRate = 5.0f;
    private float moveTimer;
    

    [SerializeField] private float minX, maxX, minY, maxY;//记得去找一下这个地图的最大最小范围！

    protected override void Move()
    {
        base.Move();
        //base.Move();//当到达attack range 执行父类的跟随

        /*if (Vector2.Distance(transform.position, target.position) > attackRange)
        {
            //guard 到达第一个巡逻点
            if (Vector2.Distance(transform.position, wayPoint1.position) < 0.01f)
            {
                wayPointTarget = wayPoint2;
            }
            //guard到达第二个巡逻点
            if (Vector2.Distance(transform.position, wayPoint2.position) < 0.01f)
            {
                wayPointTarget = wayPoint1;
            }
        }*/
        //transform.position = Vector2.MoveTowards(transform.position, wayPointTarget.position, moveSpeed * Time.deltaTime);

    }


    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        //武器初始化
        SwordPointG = transform.Find("SwordPointG").gameObject;
        SwordPointG.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        moveTimer += Time.deltaTime;

        //Attack();
        //受伤Shader
        hurtCounter -= Time.deltaTime;
        if (hurtCounter <= 0)
            sp.material.SetFloat("_FlashAmount", 0);
    }

    //受伤
    public void TakenDamage(float _amount)
    {
        healthPoint -= _amount;
        HurtShader();
        if (healthPoint <= 0)
            Destroy(gameObject);
    }

    //受伤Shader
    private void HurtShader()
    {
        sp.material.SetFloat("_FlashAmount", 1);
        hurtCounter = hurtLength;
    }



}


