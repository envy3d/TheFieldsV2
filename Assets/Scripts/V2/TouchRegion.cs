using UnityEngine;

public class TouchRegion : MonoBehaviour
{
    public RegionInfo regionInfo;


    public void Touched()
    {

    }

    public enum RegionInfo
    {
        Center,
        Left,
        Right,
        Up,
        Down
    }
}
