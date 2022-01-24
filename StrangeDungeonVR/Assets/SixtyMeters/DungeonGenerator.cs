using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<GameObject> tiles;
    public DungeonTile startTile;

    // Start is called before the first frame update
    void Start()
    {
        var tileDoorOnStartTile = startTile.tileDoor[2];
        var newTile = Instantiate(tiles[0], Vector3.zero, Quaternion.identity);
        var doorOnNewTile = newTile.GetComponent<DungeonTile>().tileDoor[2];

        var targetTransform = tileDoorOnStartTile.transform;
        var parentTransform = newTile.transform;
        var childTransform = doorOnNewTile.transform;
        
        var childRotToTarget = targetTransform.rotation * Quaternion.Inverse(childTransform.rotation);
        parentTransform.rotation = childRotToTarget;
        parentTransform.transform.RotateAround (parentTransform.transform.position, transform.up, 180f);
        
        var childToTarget = targetTransform.position - childTransform.position;

        parentTransform.position += childToTarget;
    }

    // Update is called once per frame
    void Update()
    {
    }
}