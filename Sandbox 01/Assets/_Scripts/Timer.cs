using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int time;

    float deltatime;
    Text timer;
    Animator animator;

    private void Start()
    {
        timer = GetComponent<Text>();
        animator = GetComponent<Animator>();
        UpdateTimerText();
    }
	
	void FixedUpdate ()
    {
        TimerTickTak();
    }
    public void UpdateTimerText()
    {
        timer.text = string.Format("Time: {0}", time.ToString("000"));
    }
    void TimerTickTak()
    {
        if (time > 0)
        {
            if (deltatime < 1)
            {
                deltatime += Time.fixedDeltaTime;
            }
            else
            {
                deltatime = 1;
                time -= (int)deltatime;
                UpdateTimerText();
                if (time % 5 == 0)
                {
                    animator.SetTrigger("timerTrigger");
                }
                deltatime = 0;
            }
        }
        else
        {
            time = 0;
        }
    }
    
}
