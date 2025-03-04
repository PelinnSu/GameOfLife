using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameBoard : MonoBehaviour
{
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [SerializeField] private Pattern pattern;
    private Coroutine simulationCoroutine;

    [SerializeField] private float updateInterval = 0.05f;
    private bool gameRunning = false;

    private readonly HashSet<Vector3Int> aliveCells = new();
    private readonly HashSet<Vector3Int> cellsToCheck = new();

    public int population { get; private set; }
    public int iterations { get; private set; }
    public float time { get; private set; }

    private void Start()
    {
        SetPattern(pattern);
    }
    private void Update()
    {
        HandleTouch();
    }

    private void SetPattern(Pattern pattern)
    {
        Clear();

        Vector2Int center = pattern.GetCenter();

        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int)(pattern.cells[i] - center);
            currentState.SetTile(cell, aliveTile);
            aliveCells.Add(cell);
        }

        population = aliveCells.Count;
    }

    private void HandleTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Vector3Int touchedCell = currentState.WorldToCell(worldPosition);
            PaintCellWithTouch(touchedCell);
        }
    }

    private void PaintCellWithTouch(Vector3Int touchedCell)
    {
        if (!IsAlive(touchedCell))
        {
            currentState.SetTile(touchedCell, aliveTile);
            aliveCells.Add((Vector3Int)touchedCell);
            population = aliveCells.Count;
        }
    }

    private void Clear()
    {
        aliveCells.Clear();
        cellsToCheck.Clear();
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();
        population = 0;
        iterations = 0;
        time = 0f;
    }

    public IEnumerator Simulate()
    {
        Debug.Log("simulation started");
        var interval = new WaitForSeconds(updateInterval);
        yield return interval;

        while (gameRunning)
        {
            UpdateState();

            population = aliveCells.Count;
            iterations++;
            time += updateInterval;

            yield return interval;
        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();

        // Gather cells to check
        foreach (Vector3Int cell in aliveCells)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    cellsToCheck.Add(cell + new Vector3Int(x, y));
                }
            }
        }

        // Transition cells to the next state
        foreach (Vector3Int cell in cellsToCheck)
        {
            int neighbors = CountNeighbors(cell);
            bool alive = IsAlive(cell);

            if (!alive && neighbors == 3)
            {
                nextState.SetTile(cell, aliveTile);
                aliveCells.Add(cell);
            }
            else if (alive && (neighbors < 2 || neighbors > 3))
            {
                nextState.SetTile(cell, deadTile);
                aliveCells.Remove(cell);
            }
            else // no change
            {
                nextState.SetTile(cell, currentState.GetTile(cell));
            }
        }

        // Swap current state with next state
        Tilemap temp = currentState;
        currentState = nextState;
        nextState = temp;
        nextState.ClearAllTiles();
    }

    private int CountNeighbors(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbor = cell + new Vector3Int(x, y);

                if (x == 0 && y == 0)
                {
                    continue;
                }
                else if (IsAlive(neighbor))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell) == aliveTile;
    }

    public void StartGame()
    {
        if (gameRunning) return; // Prevent multiple starts
        gameRunning = true;
        simulationCoroutine = StartCoroutine(Simulate());
    }

    public void StopSimulation()
    {
        gameRunning = false; 
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine); 
            simulationCoroutine = null;
        }
    }
}
