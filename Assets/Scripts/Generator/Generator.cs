using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGenerator
{
    public class Generator
    {
        private Vector2Int _gridSize;
        private Room[] _roomsPrototypes;
        private List<Room> _rooms;
        private System.Random _random;
        private Stack<Room> _roomsWithoutNeighboors;

        public Cell[,] Grid { get; private set; }

        public Generator(List<Room> rooms, Vector2Int gridSize, int seed)
        {
            _random = new System.Random(seed);
            _rooms = new List<Room>();
            _roomsPrototypes = new Room[rooms.Count];
            _roomsWithoutNeighboors = new Stack<Room>();
            rooms.CopyTo(_roomsPrototypes);
            _gridSize = new Vector2Int(gridSize.x, gridSize.y);
            Grid = new Cell[gridSize.x, gridSize.y];
        }
        public void Build()
        {
            var startRoom = NextRoom();
            if (CanPasteRoom(_gridSize.x / 2, _gridSize.y / 2, startRoom))
            {
                PasteRoom(_gridSize.x / 2, _gridSize.y / 2, startRoom);
            }

            while (_roomsWithoutNeighboors.Count > 0)
            {
                Room room = _roomsWithoutNeighboors.Pop();
                if (_roomsWithoutNeighboors.Count <= 4)
                {
                    CreateNeighboors(room, 4);
                }
                else
                {
                    CreateNeighboors(room);
                }

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
            Debug.Log(startX + " " + startY);
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
            int roomIndex = _random.Next(0, _roomsPrototypes.Length);
            Room newRoom = _roomsPrototypes[roomIndex].Copy();
            _rooms.Add(newRoom);
            return newRoom;
        }

        private void CreateNeighboors(Room room, int neighboorsCount)
        {
            int startDirection = _random.Next(0, 4);
            for (int i = 0; i < neighboorsCount; i++)
            {
                Room nextRoom = NextRoom();
                Direction direction = (Direction)startDirection;
                Vector2Int doorPosition = room.GetNextRoomDoorPosition(direction);
                direction = (Direction)((startDirection + 2) % 4);
                Vector2Int position = nextRoom.GetPositionFromDoor(direction, doorPosition);
                if (CanPasteRoom(position.x, position.y, nextRoom) && neighboorsCount > 0)
                {
                    neighboorsCount--;
                    PasteRoom(position.x, position.y, nextRoom);
                }
                startDirection = (startDirection + 1) % 4;
            }
        }

        private void CreateNeighboors(Room room)
        {
            int neighboorsCount = _random.Next(1, 5); //from 1 to 4 neighboors
            CreateNeighboors(room, neighboorsCount);
        }

    }
}

