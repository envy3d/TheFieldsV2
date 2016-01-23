using UnityEngine;

public class Tile : MonoBehaviour
{

    public float triggerRadius = 0.1f;
    public float triggerHeight = 1.0f;

    private float holeCreationDelay = 0.05f;
    private float craterCreationDelay = 0.05f;
    [SerializeField]
    private TileProperty tileProperties;
    private SphereCollider trigger;

    void Awake()
    {
        trigger = gameObject.AddComponent<SphereCollider>();
        trigger.center.Set(trigger.center.x, trigger.center.y + triggerHeight, trigger.center.z);
        trigger.radius = triggerRadius;
    }

    public bool HasTileProperty(TileProperty tileProperty)
    {
        return (tileProperties & tileProperty) == tileProperty;
    }

    public void Dig()
    {
        if (HasTileProperty(TileProperty.Hole | TileProperty.Crater))
        {
            Debug.Log("Tile received Dig command bug the tile is not diggable.");
            return;
        }
        Invoke("SwitchToHole", holeCreationDelay);
    }

    public void Detonate()
    {
        if (HasTileProperty(TileProperty.Landmine))
        {
            Debug.Log("Tile received Dig command bug the tile is not diggable.");
            return;
        }
        Invoke("SwitchToCrater", craterCreationDelay);
    }

    public void SwitchToHole()
    {
        tileProperties = tileProperties | TileProperty.Hole;
        // Switch mesh
    }

    public void SwitchToCrater()
    {
        tileProperties = tileProperties | TileProperty.Crater;
        tileProperties = tileProperties ^ TileProperty.Landmine;
        // Switch mesh
    }
}
