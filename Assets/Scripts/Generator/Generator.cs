using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGenerator
{
    public class Generator
    {
        private bool[,] _grid;
        private Vector2Int _gridSize;
        private Room[] _rooms;
        public Generator(List<Room> rooms, Vector2Int gridSize)
        {
            rooms.CopyTo(_rooms);
            _gridSize = new Vector2Int(gridSize.x, gridSize.y);
            _grid = new bool[gridSize.x, gridSize.y];
        }
        public void Build()
        {
            
        }
    }
}

