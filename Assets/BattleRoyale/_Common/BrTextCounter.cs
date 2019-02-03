using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrTextCounter : MonoBehaviour
{
    public Text text;

    public int EndCount=10;

    public UnityEvent OnEnd;
    private int counter = 0;

    public void SetEnd(int value)
    {
        EndCount = value;
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (counter == EndCount)
            OnEnd.Invoke();

    }

    public void Set(int value)
    {
        counter = value;
        text.text = counter.ToString();
        CheckEnd();

    }
    public void Add(int value)
    {
        counter += value;
        Debug.Log(counter);
        text.text = counter.ToString();
        CheckEnd();

    }
}
