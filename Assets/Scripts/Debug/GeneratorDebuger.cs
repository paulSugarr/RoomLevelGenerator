using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGenerator;

public class GeneratorDebuger : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private List<Room> _rooms;

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Material _roomMaterial;
    [SerializeField] private Material _doorMaterial;
    private Generator _generator;

    private void Start()
    {
        _generator = new Generator(_rooms, _gridSize, Random.Range(0, 1000));
        _generator.Build();
        
        var grid = _generator.Grid;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                var cell = Instantiate(_cellPrefab, new Vector3(i, 0, j), Quaternion.identity);
                if (grid[i,j] == Cell.Room) { cell.GetComponent<MeshRenderer>().material = _roomMaterial; }
                if (grid[i,j] == Cell.Door) { cell.GetComponent<MeshRenderer>().material = _doorMaterial; }
            }
        }
    }
}
