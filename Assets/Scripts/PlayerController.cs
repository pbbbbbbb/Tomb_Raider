using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
// using static EventListener<float>;


public class PlayerController : MonoBehaviour
{
    //位置初始化
    public static PlayerController instance;

    //菜单
    public GameObject menu;
    bool isOpen;

    //背包
    public Bag myBag;
    public ItemType itemType;

    //道具属性
    public int heal = 10;

    //移动
    Animator anim;
    Rigidbody2D rb;
    Vector2 movement;


    //武器攻击
    private GameObject SlashPoint;
    private GameObject RayPoint;
    private GameObject BulletPoint;
    int weaponChosen;
    public Weapon slash,shoot,ray;
    public Text info;


    //速度
    public float speed;

    //血条
    public float maxHp = 100.0f;
    public float currentHp = 10.0f;

    private void Awake()
    {
        currentHp = maxHp;
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isOpen = false;

        //武器初始化
        SlashPoint = transform.Find("SlashPoint").gameObject;
        RayPoint = transform.Find("RayPoint").gameObject;
        BulletPoint = transform.Find("BulletPoint").gameObject;
        RayPoint.SetActive(false);
        BulletPoint.SetActive(false);
        weaponChosen = 0;
    }

    // Update is called once per frame
    void Update()
    {

        OpenMenu();
        UseProps();

        //人物移动
        if(Input.GetKey(KeyCode.D)) movement.x = 1;
        else if(Input.GetKey(KeyCode.A)) movement.x = -1;
        else movement.x = 0;
        if(Input.GetKey(KeyCode.S)) movement.y = -1;
        else if(Input.GetKey(KeyCode.W)) movement.y = 1;
        else movement.y = 0;

        //人物攻击
        if (Input.GetKeyDown(KeyCode.E))
        {
            weaponChosen = (weaponChosen + 1) % 3;
            Debug.Log(weaponChosen);
            switch (weaponChosen)
            {
                case 0:
                    SlashPoint.SetActive(true);
                    RayPoint.SetActive(false);
                    BulletPoint.SetActive(false);
                    break;
                case 1:
                    SlashPoint.SetActive(false);
                    RayPoint.SetActive(false);
                    BulletPoint.SetActive(true);
                    break;
                case 2:
                    SlashPoint.SetActive(false);
                    RayPoint.SetActive(true);
                    BulletPoint.SetActive(false);
                    break;
            }
        }
        
        //装填弹药
        if(Input.GetKeyDown(KeyCode.R))
        {
            switch(Weapons.vis)
            {
                case 0:
                    BagManager.UpdateItemInfo("No need.");
                    break;
                case 1:
                    if(shoot.equipped == shoot.maxOneTime)
                    {
                        BagManager.UpdateItemInfo("No need.");
                    }
                    else if(shoot.max == 0)
                    {
                        BagManager.UpdateItemInfo("Please find more supplement first.");
                    }
                    else
                    {
                        int temp = shoot.equipped;
                        shoot.equipped = Math.Min(shoot.equipped + shoot.max, shoot.maxOneTime);
                        shoot.max = shoot.max - (shoot.equipped - temp);
                        info.text = shoot.equipped.ToString() + " / " + shoot.max.ToString();
                        BagManager.UpdateItemInfo("Ammunition loading succeeded.");
                    }
                    break;
                case 2:
                    if(ray.equipped == ray.maxOneTime)
                    {
                        BagManager.UpdateItemInfo("No need.");
                    }
                    else if(ray.max == 0)
                    {
                        BagManager.UpdateItemInfo("Please find more supplement first.");
                    }
                    else
                    {
                        int temp = ray.equipped;
                        ray.equipped = Math.Min(ray.equipped + ray.max, ray.maxOneTime);
                        ray.max = ray.max - (ray.equipped - temp);
                        info.text = ray.equipped.ToString() + " / " + ray.max.ToString();
                        BagManager.UpdateItemInfo("Ammunition loading succeeded.");
                    }
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        move();
    }

    // Set the player to the start room (only called at the start of the game)
    public void SetPlayerPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    //人物移动
    void move()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        if(movement.x == 0 && movement.y == 0)
        {
            anim.SetFloat("speed", 0);
        }
        else if(movement.x < 0 && movement.y == 0)
        {
            anim.SetFloat("speed", speed);
            anim.SetInteger("toward", 3);
        }
        else if(movement.x > 0 && movement.y == 0)
        {
            anim.SetFloat("speed", speed);
            anim.SetInteger("toward", 4);
        }
        else if(movement.x == 0 && movement.y > 0)
        {
            anim.SetFloat("speed", speed);
            anim.SetInteger("toward", 1);
        }
        else if(movement.x == 0 && movement.y < 0)
        {
            anim.SetFloat("speed", speed);
            anim.SetInteger("toward", 2);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //触碰伤害 随便写写先
        Debug.Log("I have hitted " + other.name);
        //currentHp -= 15.0f;
        Debug.Log("Current Hp is " + currentHp);

        //触碰道具
        if(other.CompareTag("Coin"))
        {
            Debug.Log("coin\n");
        }
        else if(other.CompareTag("SpikeWeed"))
        {
            currentHp -= 15.0f;
            Debug.Log("spikeWeed\n");
        }
        else
        {
            Debug.Log(other.tag);
        }
    }

    //The openness of the menu
    void OpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isOpen = !isOpen;
            menu.SetActive(isOpen);
        }
        if(isOpen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void UseProps()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (myBag.itemList.Contains(itemType.HpHeal) && itemType.HpHeal.itemHeld != 0)
            {
                if(currentHp == 100)
                {
                    BagManager.UpdateItemInfo("Your Hp is full.");
                }
                else if(currentHp >= 90)
                {
                    itemType.HpHeal.itemHeld -= 1;
                    currentHp = 100;
                    BagManager.UpdateItemInfo("Great! Full Hp again.");
                    BagManager.RefreshItem();
                }
                else
                {
                    itemType.HpHeal.itemHeld -= 1;
                    currentHp += 10;
                    BagManager.UpdateItemInfo("Hp + 10.");
                    BagManager.RefreshItem();
                }
            }
            else
            {
                BagManager.UpdateItemInfo("No such prop.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(myBag.itemList.Contains(itemType.bulletInc) && itemType.bulletInc.itemHeld != 0)
            {
                switch(Weapons.vis)
                {
                    case 0:
                        BagManager.UpdateItemInfo("No need.");
                        break;
                    case 1:
                        if(shoot.max == shoot.bagNum)
                            BagManager.UpdateItemInfo("Full.");
                        else
                        {
                            shoot.max = Math.Min(shoot.max + shoot.inc , shoot.bagNum);
                            BagManager.UpdateItemInfo("Supplement succeeded.");
                            info.text = shoot.equipped.ToString() + " / " + shoot.max.ToString();
                            itemType.bulletInc.itemHeld -= 1;
                            BagManager.RefreshItem();
                        }
                        break;
                    case 2:
                        if(ray.max == ray.bagNum)
                            BagManager.UpdateItemInfo("Full.");
                        else
                        {
                            ray.max = Math.Min(ray.max + ray.inc , ray.bagNum);
                            BagManager.UpdateItemInfo("Supplement succeeded.");
                            info.text = ray.equipped.ToString() + " / " + ray.max.ToString();
                            itemType.bulletInc.itemHeld -= 1;
                            BagManager.RefreshItem();
                        }
                        break;
                }
            }
            else
            {
                BagManager.UpdateItemInfo("No such prop.");
            }
        }
    }

}

[System.Serializable]
public class ItemType
{
    public Item HpHeal, bulletInc;
}
