using UnityEngine;

namespace Bunny
{
    public class Sensor : MonoBehaviour
    {
        public Tile tile;

        void Awake()
        {
            var trigger = gameObject.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            tile = other.GetComponent<Tile>();
        }

        void OnTriggerExit(Collider other)
        {
            tile = null;
        }

        public bool TileHasProperty(TileProperty property)
        {
            if (tile != null && tile.HasTileProperty(property))
            {
                return true;
            }
            return false;
        }
    }
}

