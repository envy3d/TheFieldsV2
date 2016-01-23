using UnityEngine;
using System;
using System.Collections.Generic;

public class TilePrefabs : ScriptableObject
{
    public Dictionary<TileProperty,List<Mesh>> tileAppearanceLists;

    [SerializeField]
    private List<TileProperty> tileDictionaryKeys;
    [SerializeField]
    private List<MeshList> tileDictionaryValues;

    void OnBeforeSerialize()
    {
        tileDictionaryKeys.Clear();
        tileDictionaryValues.Clear();
        foreach (var kvp in tileAppearanceLists)
        {
            tileDictionaryKeys.Add(kvp.Key);
            tileDictionaryValues.Add(new MeshList(kvp.Value));
        }
    }

    void OnAfterSerialize()
    {
        tileAppearanceLists = new Dictionary<TileProperty, List<Mesh>>();
        for (int i = 0, l = Math.Min(tileDictionaryKeys.Count, tileDictionaryValues.Count); i < l; i++)
        {
            tileAppearanceLists.Add(tileDictionaryKeys[i], tileDictionaryValues[i].meshes);
        }
    }

    [Serializable]
    class MeshList
    {
        public List<Mesh> meshes;

        public MeshList(List<Mesh> meshes)
        {
            this.meshes = meshes;
        }
    }
}
