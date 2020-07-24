using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGenerator;

public class GeneratorDebuger : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private List<Room> _rooms;
    private Generator _generator;

    private void Start()
    {
        _generator = new Generator(_rooms, _gridSize, 1);
        _generator.Build();
    }
}
