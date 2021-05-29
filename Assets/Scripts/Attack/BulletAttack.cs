using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletAttack : MonoBehaviour
{
    public GameObject bullet;
    private Vector2 bulletDirection;
    public float bulletTimeMax;
    public Weapon shoot;
    public Text info;
    private float bulletTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletTime >= bulletTimeMax)
        {
            bulletAttack();
            bulletTime -= Time.deltaTime;
        }
        else
        {
            if(bulletTime > 0)
            {
                bulletTime -= Time.deltaTime;
            }
            else if(isBulletAttack())
            {
                if(shoot.equipped > 0)
                {
                    bulletTime = bulletTimeMax;
                    shoot.equipped -= 1;
                    info.text = shoot.equipped.ToString() + " / " + shoot.max.ToString();
                }
                else
                {
                    BagManager.UpdateItemInfo("Lack of bullet.");
                }
            }
        }
    }

    private bool isBulletAttack()
    {
        if(Input.GetKey(KeyCode.RightArrow)) bulletDirection.x = 1;
        else if(Input.GetKey(KeyCode.LeftArrow)) bulletDirection.x = -1;
        else bulletDirection.x = 0;
        if(Input.GetKey(KeyCode.DownArrow)) bulletDirection.y = -1;
        else if(Input.GetKey(KeyCode.UpArrow)) bulletDirection.y = 1;
        else bulletDirection.y = 0;
        if(!(bulletDirection.x == 0 && bulletDirection.y == 0)) return true;
        else return false;
    }

    private void bulletAttack()
    {
        bulletDirection = bulletDirection.normalized;
        float rotZ = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, rotZ));
    }
}
