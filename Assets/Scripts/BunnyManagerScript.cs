using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class BunnyManagerScript : MonoBehaviour
{
    public JumpScript bunnyPrefab;
    public int respawnDistance = 3;

    private PlayerController player;
    private MapScript map;
    private JumpScript nextBunny;
    private Vector3 startPos = new Vector3();
    private Quaternion startRot = Quaternion.LookRotation(Vector3.forward);
    private Vector2 startDir = new Vector2();
    //private Timer nextBunnyMoveInTimer = new Timer();
    private Timer delayBunnyTimer = new Timer();
    private bool nextBunnyHasStarted = false;
    private bool startLocClear = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        map = GetComponent<MapScript>();
        delayBunnyTimer = new Timer();
        
    }

    void Update()
    {
        //nextBunnyMoveInTimer.Update(Time.deltaTime);
        delayBunnyTimer.Update(Time.deltaTime);
        //Debug.Log("auto bunny " + nextBunnyMoveInTimer.IsRunning);
        //Debug.Log("Start Tile Open " + startLocClear);
    }

    public void HandOverBunny()
    {
        if (nextBunny == null)
        {
            nextBunny = GameObject.Instantiate(bunnyPrefab) as JumpScript;
            nextBunny.ForcePlacement(startPos, startRot);
        }
        JumpScript bunnyToPass = nextBunny;
        nextBunnyHasStarted = false;
        startLocClear = false;
        nextBunny = GameObject.Instantiate(bunnyPrefab) as JumpScript;
        startDir = new Vector2(Mathf.Sin(Mathf.Deg2Rad * startRot.eulerAngles.y), Mathf.Cos(Mathf.Deg2Rad * startRot.eulerAngles.y));
        nextBunny.ForcePlacement(startPos - (respawnDistance * new Vector3(startDir.x, 0, startDir.y)), startRot);
        delayBunnyTimer.SetEventFunction(() => { player.SetBunny(bunnyToPass); Debug.Log("Bunny Attached"); });
       // delayBunnyTimer.SetTargetTime(nextBunnyMoveInTimer.RemainingTime + -0.2f);
        delayBunnyTimer.Restart();
        return;
    }

    public void StartLocationClear()
    {
        startLocClear = true;

        BunnyAutomationScript autoBun = nextBunny.GetComponent<BunnyAutomationScript>();
        autoBun.SetDirection(startDir);
        autoBun.SetDistance(respawnDistance);
        autoBun.Begin();
    }

    public void SetStartLocation(Vector3 startPos, Quaternion startRot)
    {
        this.startPos = startPos;
        this.startRot = startRot;
    }

    private void MoveNextBunny()
    {
        nextBunny.Move(startDir);
    }

}
