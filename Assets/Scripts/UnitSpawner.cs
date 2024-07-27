using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] Transform initialPosition;
    [SerializeField] GameObject unitPrefab;

    public List<Unit> units = new List<Unit>();
    private Unit _firstDomino;
    private Unit _lastDomino;


    void Start()
    {
        GameState.OnDominoesTilesChainUpdate += GameState_OnDominoesTilesChainUpdate;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            GameState.gameFinished = true;
        }
    }

    private void GameState_OnDominoesTilesChainUpdate(List<int[]> arg1, int[] arg2)
    {
        LogIncomingArray(arg1, arg2);
        bool useFirstDomino = arg1.FirstOrDefault() != _firstDomino?.dots;
        Debug.Log("useFirstDomino = " + useFirstDomino);
        var attachUnit = useFirstDomino ? _firstDomino : _lastDomino;
        int[] arg = useFirstDomino ? arg1.First() : arg1.Last();
        Debug.Log($"new [{arg[0]}, {arg[1]}]");

        int indexOfX = arg1.IndexOf(arg2);
        List<int[]> listPart = useFirstDomino ? arg1.Take(indexOfX + 1).Reverse().ToList() : arg1.Skip(indexOfX).ToList();
        bool isTurn = (listPart.IndexOf(arg) + 4) % 7 == 1 || (listPart.IndexOf(arg) + 4) % 7 == 2;
        bool? isTurnLeft = null;
        if (isTurn) isTurnLeft = (listPart.IndexOf(arg) + 4) % 14 == 1 || (listPart.IndexOf(arg) + 4) % 14 == 2;
        Debug.Log("isTurnLeft = " + isTurnLeft);


        Transform attachedTransform = attachUnit == null ? initialPosition : attachUnit.GetParentTransform(arg[0] != arg[1], isTurnLeft, !useFirstDomino); // isThisPerpendicular
        Debug.Log(attachedTransform.name);

        var unitTransform = Instantiate(unitPrefab, attachedTransform).transform;
        var unit = unitTransform.GetComponent<Unit>();
        unit.SetDomino(arg);

        if (units.Count == 0)
        {
            unitTransform.position = initialPosition.position;
            units.Add(unit);
            _firstDomino = unit;
            _lastDomino = unit;
            return;
        }


        //Transform attachedTransform = attachUnit.GetParentTransform(!unit.isThisPerpendicular, false, !useFirstDomino);
        //unitTransform.position = attachedTransform.position;
        unitTransform.eulerAngles = attachedTransform.eulerAngles;
        //unitTransform.localEulerAngles += attachedTransform.localEulerAngles + attachUnit.transform.localEulerAngles;
        unitTransform.SetParent(initialPosition);

        if (useFirstDomino)
        {
            unitTransform.SetAsFirstSibling();
            units.Insert(0, unit);
        }
        else
        {
            unit.Flip();
            units.Add(unit);
        }

        _firstDomino = units.First();
        _lastDomino = units.Last();

        LogCurrentArray();
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
