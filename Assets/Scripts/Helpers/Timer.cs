using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public delegate void TimerEvent();

    public class Timer
    {
        public bool IsRunning
        {
            get { return running; }
        }
        public float RemainingTime
        {
            get { return timer + (loopCount * SET_TIME); }
        }

        protected float SET_TIME;
        protected float timer;
        protected bool running;
        protected int SET_LOOPS;
        protected int loopCount;
        protected TimerEvent timerEvent;


        public Timer()
        {
            Init(1.0f, null, 1, false);
        }

        public Timer(float setTime)
        {
            Init(setTime, null, 1, false);
        }

        public Timer(float setTime, TimerEvent eventFunction)
        {
            Init(setTime, eventFunction, 1, false);
        }

        public Timer(float setTime, TimerEvent eventFunction, bool startTimer)
        {
            Init(setTime, eventFunction, 1, startTimer);
        }

        public Timer(float setTime, TimerEvent eventFunction, int loops, bool startTimer)
        {
            Init(setTime, eventFunction, loops, startTimer);
        }

        public void Update(float deltaTime)
        {
            if (running)
            {
                timer -= deltaTime;
                if (timer <= 0)
                {
                    Debug.Log("Looped");
                    --loopCount;
                    if (loopCount == 0)
                    {
                        running = false;
                        timer = 0;
                    }
                    else
                    {
                        timer = SET_TIME;
                    }
                    timerEvent();
                }
            }
        }

        public void Start()
        {
            if (timer <= 0)
                timer = SET_TIME;
            running = true;
        }

        public void Pause()
        {
            running = false;
        }

        public void Restart()
        {
            timer = SET_TIME;
            loopCount = SET_LOOPS;
            running = true;
        }

        public void SetEventFunction(TimerEvent eventFunction)
        {
            timerEvent = eventFunction;
        }

        public void SetTargetTime(float setTime)
        {
            SET_TIME = setTime;
        }

        public void SetLoops(int loops)
        {
            SET_LOOPS = loops;
        }

        protected void Init(float setTime, TimerEvent eventFunction, int loops, bool startTimer)
        {
            SET_TIME = setTime;
            timer = SET_TIME;
            SET_LOOPS = loops;
            loopCount = SET_LOOPS;
            timerEvent = eventFunction;
            running = false;
            if (startTimer)
                Start();
        }
    }
}
