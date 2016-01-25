using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileScript))]
[CanEditMultipleObjects]
public class TileScriptEditor : Editor
{
    TileProperty oldTileProperties;
    TileAppearance oldTileApprearance;
    SerializedProperty tileAppearance;
    SerializedProperty tileProperties;
    DebugTileMaterials tileMaterials;

    void OnEnable()
    {
        var tileMaterialsGUID = AssetDatabase.FindAssets("EditorTileMaterials")[0];
        var tileMaterialsPath = AssetDatabase.GUIDToAssetPath(tileMaterialsGUID);
        tileMaterials = AssetDatabase.LoadAssetAtPath<DebugTileMaterials>(tileMaterialsPath);

        tileAppearance = serializedObject.FindProperty("appearance");
        oldTileApprearance = (TileAppearance)tileAppearance.intValue;
        tileProperties = serializedObject.FindProperty("properties");
        oldTileProperties = (TileProperty)tileProperties.intValue;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        TileAppearance ta = (TileAppearance)tileAppearance.intValue;
        TileProperty tp = (TileProperty)tileProperties.intValue;
        
        if (oldTileApprearance != ta || oldTileProperties != tp)
        {
            //var renderer = ((TileScript)serializedObject.targetObject).GetComponent<Renderer>();
            MonoBehaviour[] targetComponents = GetMonoBehaviours(serializedObject.targetObjects);

            if ((tp & TileProperty.Landmine) == TileProperty.Landmine && ta != TileAppearance.Normal)
            {
                //renderer.sharedMaterial = tileMaterials.invalidMat;
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.invalidMat);
            }
            else if ((tp & TileProperty.Traversable) == TileProperty.Traversable &&
                      ta == TileAppearance.Obstacle)
            {
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.invalidMat);
            }
            else if ((tp & TileProperty.Traversable) != TileProperty.Traversable &&
                     (tp & TileProperty.Finish) == TileProperty.Finish)
            {
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.invalidMat);
            }
            else if ((tp & TileProperty.Landmine) == TileProperty.Landmine)
            {
                //renderer.sharedMaterial = tileMaterials.landmineMat;
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.landmineMat);

            }
            else if ((tp & TileProperty.Start) == TileProperty.Start)
            {
                //renderer.sharedMaterial = tileMaterials.startMat;
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.startMat);
            }
            else if ((tp & TileProperty.Finish) == TileProperty.Finish)
            {
                //renderer.sharedMaterial = tileMaterials.finishMat;
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.finishMat);
            }
            else if (ta == TileAppearance.Obstacle)
            {
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.obstacleMat);
            }
            else if ((tp & TileProperty.Traversable) == TileProperty.Traversable)
            {
                //renderer.sharedMaterial = tileMaterials.normalTraversableMat;
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.normalTraversableMat);
            }
            else if ((tp & TileProperty.Traversable) != TileProperty.Traversable && ta == TileAppearance.Normal)
            {
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.normalNontraversableMat);
            }
            else
            {
                //renderer.sharedMaterial = tileMaterials.defaultMat;
                ChangeMaterialForAllObjects(targetComponents, tileMaterials.defaultMat);
            }
        }
        oldTileProperties = tp;
    }

    private MonoBehaviour[] GetMonoBehaviours(Object[] objects)
    {
        MonoBehaviour[] mbs = new MonoBehaviour[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            mbs[i] = (MonoBehaviour)objects[i];
        }
        return mbs;
    }

    private void ChangeMaterialForAllObjects(MonoBehaviour[] mbs, Material mat)
    {
        foreach (var mb in mbs)
        {
            mb.GetComponent<Renderer>().sharedMaterial = mat;
        }
    }
}