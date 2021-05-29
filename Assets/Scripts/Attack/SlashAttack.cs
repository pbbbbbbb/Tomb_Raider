using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    Vector2 attackDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // attackDirection.x = Input.GetAxisRaw("Horizontal");
        // attackDirection.y = Input.GetAxisRaw("Vertical");
        if(isSlashAttack())
        {
            slashAttack();
        }

        // Vector2 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // difference.Normalize();
        // float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private bool isSlashAttack()
    {
        if(Input.GetKey(KeyCode.RightArrow)) attackDirection.x = 1;
        else if(Input.GetKey(KeyCode.LeftArrow)) attackDirection.x = -1;
        else attackDirection.x = 0;
        if(Input.GetKey(KeyCode.DownArrow)) attackDirection.y = -1;
        else if(Input.GetKey(KeyCode.UpArrow)) attackDirection.y = 1;
        else attackDirection.y = 0;
        if(!(attackDirection.x == 0 && attackDirection.y == 0)) return true;
        else return false;
    }
    private void slashAttack()
    {
        float rotZ = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
