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
        public float Destination;
        public Room(Vector2Int size)
        {
            Size = new Vector2Int(size.x, size.y);
        }
        public Room(int sizeX, int sizeY)
        {
            Size = new Vector2Int(sizeX, sizeY);
        }
        public Room Copy()
        {
            return new Room(Size);
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

        public Vector2Int GetNextRoomDoorPosition(Direction direction)
        {
            Vector2Int result;
            switch (direction)
            {
                case Direction.Up:
                    result = new Vector2Int(Size.x / 2, Size.y - 1) + Vector2Int.up;
                    break;
                case Direction.Right:
                    result = new Vector2Int(Size.x - 1, Size.y / 2) + Vector2Int.right;
                    break;
                case Direction.Down:
                    result = new Vector2Int(Size.x / 2, 0) + Vector2Int.down;
                    break;
                case Direction.Left:
                    result = new Vector2Int(0, Size.y / 2) + Vector2Int.left;
                    break;
                default:
                    throw new System.ArgumentException();
            }
            return result + Position;
        }

        public void SetWalls(ref Cell[,] grid)
        {
            for (int x = Position.x; x < Size.x + Position.x; x++)
            {
                for (int y = Position.y; y < Size.y + Position.y; y++)
                {
                    if (grid[x, y] != Cell.Room) { continue; }

                    if (x == Position.x || x == Size.x + Position.x - 1 ||
                        y == Position.y || y == Size.y + Position.y - 1)
                    {
                        grid[x, y] = Cell.Wall;
                    }
                }
            }
        }
    }
}