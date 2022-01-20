using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : SceneSwitch
{
    public Transform player;
    public Transform movePoint;
    public Vector3 position;
    public string previous;

    public override void Start()
    {
        base.Start();

        if(prevScene == previous)
        {
            player.position = position;
            movePoint.position = position;
        }
    }
}
