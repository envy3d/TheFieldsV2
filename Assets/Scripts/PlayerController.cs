using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class PlayerController : MonoBehaviour
{
    public int health = 1;
    public int hopDistance = 1;
    public int jumpDistance;
    public float jumpTime = 0.6f;
    public float digTime = 1.0f;
    public float movePositionTime = 0.5f;
    public float startUpTime = 1.5f;

    private int numHops;
    private int numJumps;
    private int numDigs;

    private MapScript map;
    private GameManagerScript gms;
    private JumpScript bunny;
    private Timer initTimer;
    private Timer jumpTimer;
    private Timer digTimer;
    private Timer movePositionTimer;
    private bool jumpFinished = true;
    private bool digFinished = true;
    private bool activated = false;
    private Vector2 moveDisplacement = new Vector2();


	void Start()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("Game Manager");
        map = gm.GetComponent<MapScript>();
        gms = gm.GetComponent<GameManagerScript>();
        Init();
        jumpTimer = new Timer(jumpTime, () => FinishJump());
        digTimer = new Timer(digTime, () => FinishDig());
        movePositionTimer = new Timer(movePositionTime, () => SetPosition());
	}
	
	void Update()
    {
        initTimer.Update(Time.deltaTime);
        jumpTimer.Update(Time.deltaTime);
        digTimer.Update(Time.deltaTime);
        movePositionTimer.Update(Time.deltaTime);
        if (bunny != null)
            HandlePlayerInput();
	}

    public void Init()
    {
        if (initTimer == null)
            initTimer = new Timer(startUpTime, () => Activate(), true);
        else
        {
            initTimer.SetTargetTime(startUpTime);
            initTimer.Restart();
        }
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }

    public void SetBunny(JumpScript bunny)
    {
        this.bunny = bunny;
    }

    public void DestroyBunny()
    {
        GameObject.Destroy(bunny.gameObject);
        bunny = null;
    }

    public void SetPosition()
    {
        GetComponent<AudioSource>().Play();
        if (map.DetonateTile((int)transform.position.x, (int)transform.position.z))
        {
            --health;
            if (health <= 0) gms.KillPlayer();
        }
    }

    public void ChangeInitTimerDelay(float time)
    {
        initTimer.SetTargetTime(time);
        initTimer.Restart();
    }

    private void Move(Vector2 direction, int distance)
    {
        int tilesToMove = FindJumpableDistance(distance, new Vector2(transform.position.x, transform.position.z), direction);
        moveDisplacement.x = direction.x * tilesToMove;
        moveDisplacement.y = direction.y * tilesToMove;

        if (tilesToMove == 1)
            numHops++;
        else if (tilesToMove == 2)
            numJumps++;

        if (tilesToMove > 0)
        {
            StartJump();
            transform.position = new Vector3(transform.position.x + moveDisplacement.x, transform.position.y, transform.position.z + moveDisplacement.y);
            bunny.Move(moveDisplacement);
        }

        movePositionTimer.Restart();
    }

    private int FindJumpableDistance(int jumpDistance, Vector2 pos, Vector2 dir)
    {
        for (int distance = 1; distance <= jumpDistance; ++distance)
        {
            if (!map.QueryTilePassability((int)Mathf.Round(pos.x + dir.x * distance), (int)Mathf.Round(pos.y + dir.y * distance)))
            {
                return distance - 1;
            }
        }
        return jumpDistance;
    }

    private void Dig()
    {
        if (map.DigTile((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)))
        {
            bunny.Dig();
            StartDig();
            numDigs++;
        }
    }

    private void HandlePlayerInput()
    {
        if (ReadyForAction())
        {
            if (Input.GetButton("Dig"))
            {
                Dig();
            }
            else
            {
                Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                
                if (Input.GetButton("LongJump"))
                    jumpDistance = 2;
                else
                    jumpDistance = 1;
                if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) && Mathf.Abs(moveDirection.x) >= 0.15f)
                {
                    moveDirection.y = 0.0f;
                    moveDirection.x = (moveDirection.x > 0) ? 1 : -1;
                    Move(moveDirection, jumpDistance);
                }
                else if (Mathf.Abs(moveDirection.x) < Mathf.Abs(moveDirection.y) && Mathf.Abs(moveDirection.y) >= 0.15f)
                {
                    moveDirection.x = 0.0f;
                    moveDirection.y = (moveDirection.y > 0) ? 1 : -1;
                    Move(moveDirection, jumpDistance);
                }
            }
        }
    }

    private void StartJump()
    {
        jumpFinished = false;
        jumpTimer.Restart();
    }

    private void StartDig()
    {
        digFinished = false;
        digTimer.Restart();
    }

    private void FinishJump()
    {
        jumpFinished = true;
    }

    private void FinishDig()
    {
        digFinished = true;
    }

    private bool ReadyForAction()
    {
        return activated && jumpFinished && digFinished;
    }

    public void EndOfLevel()
    {
        Application.ExternalCall("LongJumpsOnLevel", Application.loadedLevelName, numJumps);
        Application.ExternalCall("DigsOnLevel", Application.loadedLevelName, numDigs);
        Application.ExternalCall("HopsOnLevel", Application.loadedLevelName, numHops);
    }

}
