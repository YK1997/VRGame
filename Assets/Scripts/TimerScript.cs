using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    //　時間計測と表示時刻用変数（分と秒）
    [SerializeField]
    private int minute;
    [SerializeField]
    private float seconds;
    
    // 前回Update時の秒数
    private float oldSeconds;
    //時刻表示用UI(Text)
    private Text timerText;
    
    // Start is called before the first frame update
    void Start()
    {
        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;
        
        timerText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		seconds += Time.deltaTime;
        if(seconds >= 60f)
        {
        	minute++;
        	seconds = seconds - 60;
        }
        //　値が変わった時だけTextを更新
        if((int)seconds != (int)oldSeconds)
        {
        	timerText.text = minute.ToString("00") + ":" + ((int) seconds).ToString ("00");
        }
        oldSeconds = seconds;
    }
}
