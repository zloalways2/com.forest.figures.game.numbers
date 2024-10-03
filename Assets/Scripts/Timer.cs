using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float sec;
    public float min;
    public TextMeshProUGUI timeTxt;
    [SerializeField] private Text timeShadow;
    public bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (startTimer == true)
        {
            //CountingTime();
            sec += Time.deltaTime;
            if (sec > 59.9)
            {
                min++;
                sec = 0;
            }
            timeTxt.text = min.ToString() + ":" + sec.ToString("00");
            timeShadow.text = min.ToString() + ":" + sec.ToString("00");
        }
    }

}
