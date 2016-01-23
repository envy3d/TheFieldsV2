using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class BunnyAutomationScript : MonoBehaviour
{
    private bool hasStarted = false;
    private Vector2 direction;
    private int distance;
    private JumpScript bunny;
    private Timer timer;
	
	void Start()
    {
        bunny = GetComponent<JumpScript>();
        timer = new Timer();
	}
	
	void Update()
    {
        timer.Update(Time.deltaTime);
	}

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void SetDistance(int distance)
    {
        this.distance = distance;
    }

    public void Begin()
    {
        if (!hasStarted)
        {
            //timer = new Timer(bunny.hopTime + 0.1f, () => Move(), distance, false);
            timer.SetLoops(distance);
            timer.SetTargetTime(bunny.hopTime + 0.1f);
            timer.SetEventFunction(() => Move());
            timer.Restart();
            hasStarted = true;
        }
    }

    private void Move()
    {
        bunny.Move(direction);
    }
}
