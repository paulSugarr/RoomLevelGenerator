﻿using System.Collections;
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
        private Queue<Room> _roomsWithoutNeighboors;
        private Cell[,] _grid;
        private Vector2Int _startPosition;

        public Cell[,] Grid { get => _grid; private set => _grid = value; }

        public Generator(List<Room> rooms, Vector2Int gridSize, Vector2Int startPosition, int seed)
        {
            _random = new System.Random(seed);
            _rooms = new List<Room>();
            _roomsPrototypes = new Room[rooms.Count];
            _roomsWithoutNeighboors = new Queue<Room>();
            rooms.CopyTo(_roomsPrototypes);
            _gridSize = new Vector2Int(gridSize.x, gridSize.y);
            _startPosition = new Vector2Int(startPosition.x, startPosition.y);
            Grid = new Cell[gridSize.x, gridSize.y];
        }
        public void Build(Room startRoom)
        {
            startRoom.Destination = 0;
            if (_rooms.Count <= 0) { _rooms.Add(startRoom); }
            if (CanPasteRoom(_startPosition.x, _startPosition.y, startRoom))
            {
                PasteRoom(_startPosition.x, _startPosition.y, startRoom);
            }

            while (_roomsWithoutNeighboors.Count > 0)
            {
                int buffer = _roomsWithoutNeighboors.Count;
                Room room = _roomsWithoutNeighboors.Dequeue();
                CreateNeighboors(room, 4);
            }
            SetWalls();
            RemoveDoors();
            var end = FindEndRoom();
            Debug.Log(end.Position);
        }
        public void Build()
        {
            var startRoom = NextRoom();
            Build(startRoom);
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
            _rooms.Add(room);
            room.Position = new Vector2Int(startX, startY);
            for (int i = 0; i < 4; i++)
            {
                Direction direction = (Direction)i;
                Vector2Int position = room.GetDoorPosition(direction);
                Grid[position.x, position.y] = Cell.Door;
            }
            _roomsWithoutNeighboors.Enqueue(room);
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
            int roomIndex = _random.Next(0, _roomsPrototypes.Length);
            Room newRoom = _roomsPrototypes[roomIndex].Copy();
            return newRoom;
        }

        private void CreateNeighboors(Room room, int neighboorsCount)
        {
            int buffer = neighboorsCount;
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
                    nextRoom.Destination = room.Destination + 1;
                }

                startDirection = (startDirection + 1) % 4;
            }

            //if (neighboorsCount >= buffer)
            //{
            //    _currentDestination--;
            //}
        }

        private void CreateNeighboors(Room room)
        {
            int neighboorsCount = _random.Next(1, 5); //from 1 to 4 neighboors
            CreateNeighboors(room, neighboorsCount);
        }
        private void RemoveDoors()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    if ((x == _gridSize.x - 1 || y == _gridSize.y - 1 || x == 0 || y == 0) && Grid[x, y] != Cell.Empty)
                    {
                        Grid[x, y] = Cell.Wall;
                        continue;
                    }
                    if (Grid[x, y] == Cell.Door && Grid[x - 1, y] != Cell.Door && Grid[x, y - 1] != Cell.Door &&
                        Grid[x + 1, y] != Cell.Door && Grid[x, y + 1] != Cell.Door)
                    {
                        Grid[x, y] = Cell.Wall;
                    }
                }
            }
        }

        private void SetWalls()
        {
            foreach (var room in _rooms)
            {
                room.SetWalls(ref _grid);
            }
        }
        private Room FindEndRoom()
        {
            float destinationMax = 0;
            Room result = _rooms[0];
            foreach (var room in _rooms)
            {
                if (room.Destination > destinationMax)
                {
                    destinationMax = room.Destination;
                    result = room;
                }
            }
            return result;
        }
    }
}

