using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameState : MonoBehaviour
{
    public static event Action<List<int[]>, int[]> OnDominoesTilesChainUpdate;

    private readonly List<int[]> chain = new();
    private readonly int[] firstTilePlayed = { 6, 6 };

    private readonly List<int[]> availableTiles = new();

    private float nextPlayTime;
    public static bool gameFinished;

    private void Start()
    {
        for (var i = 0; i <= 6; i++)
        {
            for (var j = 0; j <= i; j++)
            {
                if (Random.value > 0.5)
                {
                    availableTiles.Add(new[] { i, j });
                }
                else
                {
                    availableTiles.Insert(0, new[] { i, j });
                }
            }
        }

        RemoveTile(firstTilePlayed);
        chain.Add(firstTilePlayed);
        nextPlayTime = GetRandomNextPlayTime();
    }

    private void Update()
    {
        if (Time.time > nextPlayTime && !gameFinished)
        {
            nextPlayTime = GetRandomNextPlayTime();
            OnDominoesTilesChainUpdate?.Invoke(chain, firstTilePlayed);

            var nextPlay = GetRandomValidNextPlay();
            if (availableTiles.Count <= 0 || nextPlay.tile[0] == -1)
            {
                gameFinished = true;
                Debug.Log("Game Ended!");
            }
            else
            {
                if (nextPlay.first)
                {
                    chain.Insert(0, nextPlay.tile);
                }
                else
                {
                    chain.Add(nextPlay.tile);
                }
                RemoveTile(nextPlay.tile);
            }
        }
    }

    private (int[] tile, bool first) GetRandomValidNextPlay()
    {
        foreach (var t in availableTiles)
        {
            if (Random.value > 0.5f)
            {
                if (t[0] == chain[0][0])
                {
                    return (t.Reverse().ToArray(), true);
                }

                if (t[1] == chain[0][0])
                {
                    return (t, true);
                }

                if (t[0] == chain[^1][1])
                {
                    return (t, false);
                }

                if (t[1] == chain[^1][1])
                {
                    return (t.Reverse().ToArray(), false);
                }
            }
            else
            {
                if (t[0] == chain[^1][1])
                {
                    return (t, false);
                }

                if (t[1] == chain[^1][1])
                {
                    return (t.Reverse().ToArray(), false);
                }

                if (t[0] == chain[0][0])
                {
                    return (t.Reverse().ToArray(), true);
                }

                if (t[1] == chain[0][0])
                {
                    return (t, true);
                }
            }
        }

        return (new[] { -1, -1 }, false);
    }

    private void RemoveTile(int[] tile)
    {
        for (var i = 0; i < availableTiles.Count; i++)
        {
            if (TilesAreEqual(availableTiles[i], tile))
            {
                availableTiles.RemoveAt(i);
                break;
            }
        }
    }

    private static bool TilesAreEqual(int[] tile1, int[] tile2)
    {
        return (tile1[0] == tile2[0] && tile1[1] == tile2[1]) || (tile1[0] == tile2[1] && tile1[1] == tile2[0]);
    }

    private static float GetRandomNextPlayTime()
    {
        return Time.time;// + Random.Range(0.5f, 3f);
    }

    private void OnDestroy()
    {
        OnDominoesTilesChainUpdate = null;
    }
}