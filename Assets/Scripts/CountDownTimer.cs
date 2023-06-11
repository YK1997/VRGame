using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{

    //　時間計測用変数（分と秒）
    private float totalTime;
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
        totalTime = minute * 60 + seconds;
        oldSeconds = 0f;
        timerText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
    	//　制限時間が0秒以下なら何もしない
		if (totalTime <= 0f) return;
    	
    	//　一旦トータルの制限時間を計測；
    	totalTime = minute * 60 + seconds;
    	totalTime -= Time.deltaTime;
        
        //　再設定
        minute = (int) totalTime / 60;
        seconds = totalTime - minute * 60;
        
        //　タイマー表示用UIテキストに時間を表示する
        if((int)seconds != (int)oldSeconds)
        {
        	timerText.text = minute.ToString("00") + ":" + ((int) seconds).ToString("00");
        }
        oldSeconds = seconds;
        //　制限時間以下になったらコンソールに『制限時間終了』という文字列を表示する
        if(totalTime <= 0f)
        {
			timerText.text = "制限時間終了";
        }
    }

    public float GetTotalTime()
    {
	    return totalTime;
    } 
}
