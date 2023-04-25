using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int Score;
    public int Lives;

    public Vector3 pacmanPosition;
    public Vector3 blinkyPos;
    public Vector3 inkyPos;
    public Vector3 pinkyPos;
    public Vector3 clydePos;

    public List<Vector3> pelPos;

    public GameData()
    {
        this.Score = 0;
        this.Lives = 3;
        pacmanPosition = Vector3.zero;
        blinkyPos = Vector3.zero;
        inkyPos = Vector3.zero;
        pinkyPos = Vector3.zero;
        clydePos = Vector3.zero;

    }
}
