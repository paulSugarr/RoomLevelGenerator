using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GridGenerator
{
    [System.Serializable]
    public class Room
    {
        public Vector2Int Size;
        public Room(Vector2Int size)
        {
            Size = new Vector2Int(size.x, size.y);
        }
        public Room(int sizeX, int sizeY)
        {
            Size = new Vector2Int(sizeX, sizeY);
        }
    }
}