using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    public Text info;
    public WeaponType weaponType;
    public Weapon currentWeapon;
    public static int vis;
    public GameObject slash,shoot,ray;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = weaponType.slash;
        vis = 0;
        renew(vis);
        slash = transform.Find("slash").gameObject;
        shoot = transform.Find("shoot").gameObject;
        ray = transform.Find("ray").gameObject;
        slash.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            vis = (vis + 1) % 3;
            renew(vis);
        }
    }

    public void renew(int vis)
    {
        if(vis == 0)
        {
            currentWeapon = weaponType.slash;
            info.text = currentWeapon.info;
            slash.SetActive(true);
            shoot.SetActive(false);
            ray.SetActive(false);
        }
        else if(vis == 1)
        {
            currentWeapon = weaponType.shoot;
            weaponType.shoot.info = weaponType.shoot.equipped.ToString() + " / " + weaponType.shoot.max.ToString();
            info.text = currentWeapon.info;
            slash.SetActive(false);
            shoot.SetActive(true);
            ray.SetActive(false);
        }
        else if(vis == 2)
        {
            currentWeapon = weaponType.ray;
            weaponType.ray.info = weaponType.ray.equipped.ToString() + " / " + weaponType.ray.max.ToString();
            info.text = currentWeapon.info;
            slash.SetActive(false);
            shoot.SetActive(false);
            ray.SetActive(true);
        }
    }

}

[System.Serializable]
public class WeaponType
{
    public Weapon shoot, slash, ray;
}
