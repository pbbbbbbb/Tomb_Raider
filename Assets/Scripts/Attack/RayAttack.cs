using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayAttack : MonoBehaviour
{
    //激光
    private float maxDist;
    public LayerMask mask;
    public float rayTimeMax;
    public Weapon ray;
    public Text info;
    private Vector2 rayAttackDirection;
    [Header("Laser")]
    private LineRenderer lineRenderer;
    [SerializeField] private Gradient yellowColor, redColor;
    // Start is called before the first frame update

    private float rayTime;
    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        maxDist = 100000;
        rayTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(rayTime > 0)
        {
            rayDetect();
            rayTime -= Time.deltaTime;
        }
        else
        {
            lineRenderer.enabled = false;
            if(isRayAttack())
            {
                if(ray.equipped > 0)
                {
                    rayTime = rayTimeMax;
                    ray.equipped -= 1;
                    info.text = ray.equipped.ToString() + " / " + ray.max.ToString();
                }
                else
                {
                    BagManager.UpdateItemInfo("Lack of ray.");
                }
            }
        }
    }


    private void rayDetect()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, rayAttackDirection, maxDist, mask);
        if(hitInfo.collider != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.colorGradient = yellowColor;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            
            //Debug.DrawLine(transform.position, hitInfo.point, Color.green);
            // if(hitInfo.collider.tag == "Block")
            // {
            //     lineRenderer.SetPosition(1, hitInfo.point);
            //     lineRenderer.colorGradient = yellowColor;
            // }
            // if(hitInfo.collider.tag == "Enemy")
            // {
            //     lineRenderer.colorGradient = redColor;
            // }
        }
    }

    private bool isRayAttack()
    {
        if(Input.GetKey(KeyCode.RightArrow)) rayAttackDirection.x = 1;
        else if(Input.GetKey(KeyCode.LeftArrow)) rayAttackDirection.x = -1;
        else rayAttackDirection.x = 0;
        if(Input.GetKey(KeyCode.DownArrow)) rayAttackDirection.y = -1;
        else if(Input.GetKey(KeyCode.UpArrow)) rayAttackDirection.y = 1;
        else rayAttackDirection.y = 0;
        if(!(rayAttackDirection.x == 0 && rayAttackDirection.y == 0)) return true;
        else return false;
    }
}
