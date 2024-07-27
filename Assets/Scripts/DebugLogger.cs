using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameState.OnDominoesTilesChainUpdate += GameState_OnDominoesTilesChainUpdate;
    }

    private void GameState_OnDominoesTilesChainUpdate(List<int[]> arg1, int[] arg2)
    {
        string array = string.Empty;
        foreach (int[] arg in arg1)
        {
            array += $"[{arg[0]}, {arg[1]}]";
        }
        array += $"-[{arg2[0]}, {arg2[1]}]";
        Debug.Log(array);
    }
}
