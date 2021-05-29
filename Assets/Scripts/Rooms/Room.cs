using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
// -------------------------- public variables --------------------------
    public GameObject doorLeft, doorRight, doorUp, doorDown;
    public bool roomLeft, roomRight, roomUp, roomDown;

// -------------------------- private variables --------------------------
    private int doorNumber;
    protected Vector3 position; // upper left conner of the *layout*, inner part of the room (without the walls)
    protected const int xRange = 16, yRange = 8;    // length of x, y of the layout

// -------------------------- start --------------------------
    // Start is called before the first frame update
    protected void Start()
    {
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        doorDown.SetActive(roomDown);
        doorUp.SetActive(roomUp);
    }

// -------------------------- other functions --------------------------
    // Count door number
    public void UpdateRoom()
    {
        if (roomUp)
            doorNumber++;
        if (roomDown)
            doorNumber++;
        if (roomLeft)
            doorNumber++;
        if (roomRight)
            doorNumber++;
    }

    // return door number
    public int getDoorNumber()
    {
        return doorNumber;
    }

    // Virtual function, implemented in subclasses
    public virtual void GenerateLayout(GameObject coin, GameObject spikeWeed)
    {

    }

    // Check collision with player & move camera
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            CameraController.instance.ChangeTarget(transform);
        }
    }

    // Set position of the room
    public void setPosition(Vector3 pos)
    {
        this.position = pos + new Vector3(-1 * (float)(xRange / 2 - 0.5), (float)(yRange / 2 - 0.5), 0);
    }
}
