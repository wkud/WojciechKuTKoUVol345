using System;
using UnityEngine;

namespace TKOU.SimAI
{
    public class Timer // quick and dirty way of doing a timer, I got issues implementing coroutines
    {
        private Action callback;
        private float timer = 0;
        private readonly float interval;

        private bool enabled = true;
        
        public Timer(Action callback, float interval)
        {
            this.callback = callback;
            this.interval = interval;
        }

        public void Update()
        {
            if (!enabled)
                return;
            
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                callback.Invoke();
                timer = interval;
            }
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}