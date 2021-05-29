using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endFireAttack : MonoBehaviour
{
    public void EndAttack()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.GetComponentInChildren<PlayerHealthBar>().hp -= 35;
            //Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
