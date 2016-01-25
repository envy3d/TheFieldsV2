using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu]
public class DebugTileMaterials : ScriptableObject
{
    public Material landmineMat;
    public Material startMat;
    public Material finishMat;
    public Material traversableMat;
    public Material defaultMat;
}