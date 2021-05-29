using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Bag/New Weapon")]
public class Weapon : ScriptableObject
{
    public string info;
    public Sprite weaponImage;
    public int max;
    public int bagNum;
    public int equipped;
    public int maxOneTime;
    public int inc;
}
