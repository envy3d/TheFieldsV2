using UnityEngine;
using System.Collections;

namespace Bunny
{
    [RequireComponent(typeof(Bunny.SensorArray))]
    public class BunnyController : MonoBehaviour
    {
        public float digActionDuration = 1.0f;
        public float shortMoveDuration = 0.6f;
        public float longMoveDuration = 1.0f;

        [SerializeField]
        private AnimationPath shortJumpPath;
        [SerializeField]
        private AnimationPath longJumpPath;

        private SensorArray sensorArray;
        private Animator animator;
        private Transform modelTransform;
        private bool checkSensorsOnUpdate;
        private bool isActing;
        private int digAnimHash;
        private int jumpAnimHash;

        void OnEnable() 
        {
            animator = GetComponentInChildren<Animator>();
            modelTransform = animator.transform;
            digAnimHash = Animator.StringToHash("Digging");
            jumpAnimHash = Animator.StringToHash("Jumping");
            sensorArray = GetComponent<SensorArray>();
            checkSensorsOnUpdate = true;
            isActing = false;
        }

        void Update()
        {
            if (checkSensorsOnUpdate)
            {
                checkSensorsOnUpdate = false;
                if (sensorArray.centerSensor.TileHasProperty(TileProperty.Landmine))
                {
                    sensorArray.centerSensor.tile.Detonate();
                    Destroy(gameObject, 0.05f);
                    return;
                }
                if (sensorArray.centerSensor.TileHasProperty(TileProperty.Finish))
                {

                }
            }
        }

        public void Dig()
        {
            if (isActing)
                return;

            if (!sensorArray.centerSensor.TileHasProperty(TileProperty.Hole | TileProperty.Crater))
            {
                animator.SetTrigger(digAnimHash);
                sensorArray.centerSensor.tile.Dig();
                isActing = true;
                Invoke("CompleteAction", digActionDuration);
            }
        }

        public void Move(int x, int z, bool longJump)
        {
            Debug.Log("Move called on BunnyController");
            if (isActing)
                return;

            if (x != 0 && z != 0)
            {
                Debug.Log("Bunny Controller was told to move diagonally.");
                return;
            }
            if (Mathf.Abs(x) > 1 || Mathf.Abs(z) > 1)
            {
                Debug.Log("Bunny Controller was told to move farther than 1 unit.");
                return;
            }
            if (IsShortJumpValid(x, z))
            {
                if (longJump && IsLongJumpValid(x, z))
                {
                    longJumpPath.Setup(transform.position, new Vector3(transform.position.x + (x * 2), transform.position.y, transform.position.z + (z * 2)),
                                       modelTransform.rotation, Quaternion.LookRotation(new Vector3(x, 0, z)));

                    animator.SetTrigger(jumpAnimHash);
                    StartCoroutine(AnimateMovement(longJumpPath));
                }
                else
                {
                    shortJumpPath.Setup(transform.position, new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z),
                                        modelTransform.rotation, Quaternion.LookRotation(new Vector3(x, 0, z)));

                    animator.SetTrigger(jumpAnimHash);
                    StartCoroutine(AnimateMovement(shortJumpPath));
                }
                isActing = true;
                return;
            }
        }

        private IEnumerator AnimateMovement(AnimationPath animationPath)
        {
            while (true)
            {
                transform.position = animationPath.UpdatePosition();
                modelTransform.rotation = animationPath.UpdateRotation();
                if (animationPath.IsFinishedUpdating())
                {
                    break;
                }
                yield return null;
            }
            isActing = false;
        }

        private bool IsShortJumpValid(int x, int z)
        {
            if (z == 1 && sensorArray.northSensor.TileHasProperty(TileProperty.Traversable))
                return true;
            else if (x == 1 && sensorArray.eastSensor.TileHasProperty(TileProperty.Traversable))
                return true;
            else if (z == -1 && sensorArray.southSensor.TileHasProperty(TileProperty.Traversable))
                return true;
            else if (x == -1 && sensorArray.westSensor.TileHasProperty(TileProperty.Traversable))
                return true;
            return false;
        }

        private bool IsLongJumpValid(int x, int z)
        {
            if (z == 1 && sensorArray.north2Sensor.TileHasProperty(TileProperty.Traversable))
                return true;
            else if (x == 1 && sensorArray.east2Sensor.TileHasProperty(TileProperty.Traversable))
                return true;
            else if (z == -1 && sensorArray.south2Sensor.TileHasProperty(TileProperty.Traversable))
                return true;
            else if (x == -1 && sensorArray.west2Sensor.TileHasProperty(TileProperty.Traversable))
                return true;
            return false;
        }

        
        //  private Quaternion MovementDirectionToQuaternion(int x, int z)
        //  {
        //      return Quaternion.LookRotation(new Vector3(x, 0, z));
        //  }

        public void CheckSensors()
        {
            checkSensorsOnUpdate = true;
        }

        public void CompleteAction()
        {
            isActing = false;
        }
    }
}