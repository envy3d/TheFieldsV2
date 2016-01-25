using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileScript))]
[CanEditMultipleObjects]
public class TileScriptEditor : Editor
{
    TileProperty oldTileProperties;
    SerializedProperty tileProperties;
    DebugTileMaterials tileMaterials;

    void OnEnable()
    {
        var tileMaterialsGUID = AssetDatabase.FindAssets("EditorTileMaterials")[0];
        var tileMaterialsPath = AssetDatabase.GUIDToAssetPath(tileMaterialsGUID);
        tileMaterials = AssetDatabase.LoadAssetAtPath<DebugTileMaterials>(tileMaterialsPath);
        tileProperties = serializedObject.FindProperty("properties");
        oldTileProperties = (TileProperty)tileProperties.intValue;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        TileProperty tp = (TileProperty)tileProperties.intValue;
        
        if (oldTileProperties != tp)
        {
            var renderer = ((TileScript)serializedObject.targetObject).GetComponent<Renderer>();
            if ((tp & TileProperty.Landmine) == TileProperty.Landmine)
            {
                renderer.sharedMaterial = tileMaterials.landmineMat;
            }
            else if ((tp & TileProperty.Start) == TileProperty.Start)
            {
                renderer.sharedMaterial = tileMaterials.startMat;
            }
            else if ((tp & TileProperty.Finish) == TileProperty.Finish)
            {
                renderer.sharedMaterial = tileMaterials.finishMat;
            }
            else if ((tp & TileProperty.Traversable) == TileProperty.Traversable)
            {
                renderer.sharedMaterial = tileMaterials.traversableMat;
            }
            else
            {
                renderer.sharedMaterial = tileMaterials.defaultMat;
            }
        }
        oldTileProperties = tp;
    }
}