using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFinder : MonoBehaviour
{
    Player player;
    RoomTracker roomTracker;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        roomTracker = FindObjectOfType<RoomTracker>();
    }
}
