using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject target;
    [SerializeField] private Camera came
        ;
    void Start()
    {
        
//        StartCoroutine(Fukidasu());
        StartCoroutine(DisplayPopup());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fukidasu()
    {
        //プレイヤーのちょっと前面上に表示
        var chottoue = target.transform.position+(target.transform.forward * 2.0f);
        chottoue.y += 0.5f;
        var fukidashi = FukidashiManager.MakeFukdashi("ほげえええええええええええ",
            target, came, chottoue,new Vector2(500,100));
        //ポップアップ表示テストスクリプト
        //LocalPosition
        yield return new WaitForSeconds(3.0f);
        FukidashiManager.Close(fukidashi,2.0f);
    }
    IEnumerator DisplayPopup()
    {
        //ポップアップ表示テストスクリプト
        //LocalPosition
        GameObject canvas = GameObject.Find("/Canvas");
        if (canvas)
        {
            yield return new WaitForSeconds(1.0f);
            GameObject popup = (GameObject)GameObject.Instantiate(Resources.Load("simple_popup"));
            popup.transform.SetParent(canvas.transform);
            RectTransform rect = popup.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(0,0,0);
            yield return new WaitForSeconds(3.0f);

            GameObject popup2 = (GameObject)GameObject.Instantiate(Resources.Load("simple_popup"));
            popup2.transform.SetParent(canvas.transform);
            RectTransform rect2 = popup2.GetComponent<RectTransform>();
            rect2.localPosition = new Vector3(-100,-100,0);
        }
    }
}
