using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] Transform initialPosition;
    [SerializeField] GameObject unitPrefab;

    // List of all created domino objects
    public List<Unit> units = new List<Unit>();
    public List<Image> unitsImages = new List<Image>();

    // The first and last domino in the chain
    private Unit _firstDomino;
    private Unit _lastDomino;

    // Static event for camera refresh
    public static event Action<List<Image>> OnDominoesTilesPlaced;

    private bool _isLogging = false;

    void Start()
    {
        // Subscribe to the event that is triggered when the domino chain is updated
        GameState.OnDominoesTilesChainUpdate += GameState_OnDominoesTilesChainUpdate;
    }

    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKey)
        {
            GameState.gameFinished = true;
        }
    }

    private void GameState_OnDominoesTilesChainUpdate(List<int[]> chain, int[] central)
    {
        // Log incoming data for debugging
        if (_isLogging) LogIncomingArray(chain, central);

        // Determine if the first domino should be used as the attachment point
        bool attachToFirst = chain.FirstOrDefault() != _firstDomino?.dots;
        if (_isLogging) Debug.Log("useFirstDomino = " + attachToFirst);

        // Select the unit to attach to (either the first or the last)
        Unit attachUnit = attachToFirst ? _firstDomino : _lastDomino;
        int[] newDominoDots = attachToFirst ? chain.First() : chain.Last();
        if (_isLogging) Debug.Log($"new [{newDominoDots[0]}, {newDominoDots[1]}]");

        // Determine if a left turn is needed
        // Variable isTurn returns null if it is not turn, true if it is odd turn and false if even
        bool? isTurnLeft = IsTurnLeft(chain, central, newDominoDots, attachToFirst, out bool isTurn);

        // Instantiate the new domino and configure it
        Transform unitTransform = Instantiate(unitPrefab, initialPosition).transform;
        Unit unit = unitTransform.GetComponent<Unit>();
        unit.SetDomino(newDominoDots);

        // Handle double domino (rotate 90 degrees)
        if (!isTurn) unit.RotateIfDouble(newDominoDots[0] == newDominoDots[1]);
        else unit.isTurned = true;

        // If this is the first domino in the chain, set its position
        if (units.Count == 0)
        {
            InitializeFirstDomino(unit, unitTransform);
            return;
        }

        // Determine the position and direction of the new domino
        unitTransform.position = attachUnit.GetNextDominoPosition(out Vector3 direction, newDominoDots[0] != newDominoDots[1], isTurnLeft, !attachToFirst);
        if (_isLogging) Debug.Log(direction);

        // Rotate upside down if it is added to end of chain
        unitTransform.up = attachToFirst ? direction : -direction;

        // Add the domino to the beginning or end of the chain
        if (attachToFirst)
        {
            unitTransform.SetAsFirstSibling();
            units.Insert(0, unit);
        }
        else
        {
            unit.Flip();
            units.Add(unit);
        }

        // Handle the case where the domino is flipped
        if (unit.isTurned && !attachUnit.isTurned && attachUnit.isThisFlipped) 
            unitTransform.eulerAngles = new Vector3 (unitTransform.eulerAngles.x + 180, 
                unitTransform.eulerAngles.y, unitTransform.eulerAngles.z);

        // Update the first and last domino
        _firstDomino = units.First();
        _lastDomino = units.Last();

        // Log the current array state
        if (_isLogging) LogCurrentArray();

        // Add new image to list to refresh camera
        unitsImages.Add(unit.Image);
        OnDominoesTilesPlaced?.Invoke(unitsImages);
    }

    private void InitializeFirstDomino(Unit unit, Transform unitTransform)
    {
        unitTransform.position = initialPosition.position;
        units.Add(unit);
        _firstDomino = unit;
        _lastDomino = unit;
    }

    private bool? IsTurnLeft(List<int[]> chain, int[] central, int[] newDots, bool useFirstDomino, out bool isTurn)
    {
        // Get index of central element of chain
        int indexOfX = chain.IndexOf(central);

        // Make list of first or last half of chain (list always starts from central element)
        List<int[]> listPart = useFirstDomino ? chain.Take(indexOfX + 1).Reverse().ToList() : chain.Skip(indexOfX).ToList();

        // Get index of new domino in new list
        int indexOfNewArg = listPart.IndexOf(newDots);

        // Get if element should turn depending on index
        isTurn = (indexOfNewArg + 4) % 7 == 1 || (indexOfNewArg + 4) % 7 == 2;
        bool? isTurnLeft = null;
        if (isTurn)
        {
            // Get turn direction
            isTurnLeft = ((indexOfNewArg + 4) / 7) % 2 == 1;
        }
        if (_isLogging) Debug.Log("isTurnLeft = " + isTurnLeft);
        return isTurnLeft;
    }

    private void LogIncomingArray(List<int[]> arg1, int[] arg2)
    {
        string array = "incoming array: ";
        foreach (int[] arg in arg1)
        {
            array += $"[{arg[0]}, {arg[1]}]";
        }
        array += $"-[{arg2[0]}, {arg2[1]}]";
        Debug.Log(array);
    }

    private void LogCurrentArray()
    {
        string array = "current array: ";
        foreach (Unit unit in units)
        {
            array += $"[{unit.dots[0]}, {unit.dots[1]}]";
        }
        array += $"-[6,6]";
        Debug.Log(array);
    }
}
