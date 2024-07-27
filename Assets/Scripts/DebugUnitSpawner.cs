using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugUnitSpawner : MonoBehaviour
{
    [SerializeField] Unit debugUnit;
    private int[] _prevFirstDomino;

    void Start()
    {
        GameState.OnDominoesTilesChainUpdate += GameState_OnDominoesTilesChainUpdate;
    }

    private void GameState_OnDominoesTilesChainUpdate(List<int[]> arg1, int[] arg2)
    {
        bool useFirstDomino = arg1.FirstOrDefault() != _prevFirstDomino;
        _prevFirstDomino = arg1.FirstOrDefault();
        int[] value = useFirstDomino ? _prevFirstDomino : arg1.LastOrDefault();
        debugUnit.SetDomino(value);
        Debug.Log($"[{value[0]}, {value[1]}]");
    }
}
