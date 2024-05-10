using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence
{
    public float speed;
    public int nrCircles;
    public int nrOfSpots;
    public float spotRadius;
    public int spotsPosition;


    public string name;
    public int number;
    public bool playedOnce;
    public bool validated;



    public Sequence()
    {
        speed = 0.2f;
        nrCircles = 2;
        nrOfSpots = 1;
        spotRadius = 0.6f;
        spotsPosition = 0;

        name = "unnamed";
        number = -1;
        playedOnce = false;
        validated = false;
    }
}
