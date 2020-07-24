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
        private Stack<Room> _roomsWithoutNeighboors;

        public Cell[,] Grid { get; private set; }

        public Generator(List<Room> rooms, Vector2Int gridSize, int seed)
        {
            _random = new System.Random(seed);
            _rooms = new Room[rooms.Count];
            _roomsWithoutNeighboors = new Stack<Room>();
            rooms.CopyTo(_rooms);
            _gridSize = new Vector2Int(gridSize.x, gridSize.y);
            Grid = new Cell[gridSize.x, gridSize.y];
        }
        public void Build()
        {
            int roomIndex = _random.Next(0, _rooms.Length);
            if (CanPasteRoom(_gridSize.x / 2, _gridSize.y / 2, _rooms[roomIndex]))
            {
                PasteRoom(_gridSize.x / 2, _gridSize.y / 2, _rooms[roomIndex]);
            }

            while (_roomsWithoutNeighboors.Count > 0)
            {
                Room room = _roomsWithoutNeighboors.Pop();
                CreateNeighboors(room);
            }
        }
        private void PasteRoom(int startX, int startY, Room room)
        {
            if (startX < 0 || startY < 0 || startX + room.Size.x > Grid.GetLength(0) || startY + room.Size.y > Grid.GetLength(1))
            {
                throw new System.ArgumentException();
            }

            for (int x = startX; x < startX + room.Size.x; x++)
            {
                for (int y = startY; y < startY + room.Size.y; y++)
                {
                    Grid[x, y] = Cell.Room;
                }
            }

            room.Position = new Vector2Int(startX, startY);
            for (int i = 0; i < 4; i++)
            {
                Direction direction = (Direction)i;
                Vector2Int position = room.GetDoorPosition(direction);
                Grid[position.x, position.y] = Cell.Door;
            }
            _roomsWithoutNeighboors.Push(room);
        }
        private bool CanPasteRoom(int startX, int startY, Room room)
        {
            if (startX < 0 || startY < 0 || startX + room.Size.x > Grid.GetLength(0) || startY + room.Size.y > Grid.GetLength(1))
            {
                return false;
            }

            for (int x = startX; x < startX + room.Size.x; x++)
            {
                for (int y = startY; y < startY + room.Size.y; y++)
                {
                    if (Grid[x, y] != Cell.Empty) { return false; }
                }
            }
            return true;
        }
        private Room NextRoom()
        {
            int roomIndex = _random.Next(0, _rooms.Length);
            return _rooms[roomIndex];
        }

        private void CreateNeighboors(Room room)
        {
            int neighboorsCount = 3;
            //int neighboorsCount = _random.Next(0, 3); //from 0 to 3 additional neighboors
            for (int i = 0; i < neighboorsCount; i++)
            {
                Room nextRoom = NextRoom();
                if (CanPasteRoom(room.Position.x, room.Position.y, nextRoom))
                {
                    Direction direction = (Direction)i;
                    Vector2Int position = room.GetDoorPosition(direction);
                    PasteRoom(position.x, position.y, nextRoom);
                }
            }
        }

    }
}

