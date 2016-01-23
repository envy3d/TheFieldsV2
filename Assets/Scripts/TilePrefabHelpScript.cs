using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilePrefabHelpScript : MonoBehaviour
{

    public GameObject[] startingPrefabs;
    public GameObject[] holePrefabs;
    public GameObject[] craterPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject explosionPrefab;
    public GameObject[] heatmaps;
    public float heatmapUpdateDelay = 0.5f;

    public GameObject ChangeState(TileAppearance state)
    {
        switch (state)
        {
            case TileAppearance.Normal:
                return SetAppearance(startingPrefabs);
            case TileAppearance.Hole:
                return SetAppearance(holePrefabs);
            case TileAppearance.Crater:
                return SetAppearance(craterPrefabs);
            case TileAppearance.Obstacle:
                return SetAppearance(obstaclePrefabs);
            default:
                return null;
        }
    }

    public GameObject GetHeatmap(int proximityToMine)
    {
        return (proximityToMine > 0 && proximityToMine <= heatmaps.Length) ? heatmaps[proximityToMine - 1] : null;
    }

    private GameObject SetAppearance(GameObject[] prefabs)
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

}