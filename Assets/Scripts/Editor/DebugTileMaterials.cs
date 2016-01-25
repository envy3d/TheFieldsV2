using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu]
public class DebugTileMaterials : ScriptableObject
{
    public Material landmineMat;
    public Material startMat;
    public Material finishMat;
    public Material normalTraversableMat;
    public Material normalNontraversableMat;
    public Material obstacleMat;
    public Material defaultMat;
    public Material invalidMat;
}