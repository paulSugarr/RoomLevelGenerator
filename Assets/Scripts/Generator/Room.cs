using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GridGenerator
{
    [System.Serializable]
    public class Room
    {
        public Vector2Int Size;
        public Vector2Int Position = new Vector2Int(0, 0);
        public Room(Vector2Int size)
        {
            Size = new Vector2Int(size.x, size.y);
        }
        public Room(int sizeX, int sizeY)
        {
            Size = new Vector2Int(sizeX, sizeY);
        }
        
        public Vector2Int GetDoorLocalPosition(Direction direction)
        {
            Vector2Int result;
            switch (direction)
            {
                case Direction.Up:
                    result = new Vector2Int(Size.x / 2, Size.y - 1);
                    break;
                case Direction.Right:
                    result = new Vector2Int(Size.x - 1, Size.y / 2);
                    break;
                case Direction.Down:
                    result = new Vector2Int(Size.x / 2, 0);
                    break;
                case Direction.Left:
                    result = new Vector2Int(0, Size.y / 2);
                    break;
                default:
                    throw new System.ArgumentException();
            }
            return result;
        }
        public Vector2Int GetDoorPosition(Direction direction)
        {
            return GetDoorLocalPosition(direction) + Position;
        }
        public Vector2Int GetPositionFromDoor(Direction thisDoorDirection, Vector2Int doorPosition)
        {
            return doorPosition - GetDoorLocalPosition(thisDoorDirection);
        }
    }
}