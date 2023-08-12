using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Faction { normal,holy,hell}
    public Faction faction;

    void Start()
    {
        faction = Faction.normal;
    }
}
