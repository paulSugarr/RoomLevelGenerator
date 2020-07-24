using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGenerator
{
    public class Generator
    {
        private Vector2Int _gridSize;
        private Room[] _rooms;
        private System.Random _random;

        public bool[,] Grid { get; private set; }

        public Generator(List<Room> rooms, Vector2Int gridSize, int seed)
        {
            _random = new System.Random(seed);
            _rooms = new Room[rooms.Count];
            rooms.CopyTo(_rooms);
            _gridSize = new Vector2Int(gridSize.x, gridSize.y);
            Grid = new bool[gridSize.x, gridSize.y];
        }
        public void Build()
        {
            int roomIndex = _random.Next(0, _rooms.Length);
            if (CanPasteRoom(_gridSize.x / 2, _gridSize.y / 2, _rooms[roomIndex]))
            {
                PasteRoom(_gridSize.x / 2, _gridSize.y / 2, _rooms[roomIndex]);
            }
        }
        private void PasteRoom(int startX, int startY, Room room)
        {
            if (startX < 0 || startY < 0 || startX >= Grid.GetLength(0) || startY >= Grid.GetLength(1))
            {
                throw new System.ArgumentException();
            }

            for (int x = startX; x < room.Size.x; x++)
            {
                for (int y = startY; y < room.Size.y; y++)
                {
                    Grid[x, y] = true;
                }
            }

            Debug.Log("pasted");
        }
        private bool CanPasteRoom(int startX, int startY, Room room)
        {
            if (startX < 0 || startY < 0 || startX + room.Size.x > Grid.GetLength(0) || startY + room.Size.y > Grid.GetLength(1))
            {
                return false;
            }

            for (int x = startX; x < room.Size.x; x++)
            {
                for (int y = startY; y < room.Size.y; y++)
                {
                    if (Grid[x, y]) { return false; }
                }
            }
            return true;
        }
    }
}

