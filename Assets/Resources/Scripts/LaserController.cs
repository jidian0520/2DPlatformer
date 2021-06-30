using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザーの動きを制御
/// </summary>
public class LaserController : MonoBehaviour
{
    float speed = 0.05f;       //レーザーのスピード
    float range = 0.05f;        //レーザーの射程距離

    // Start is called before the first frame update
    void Start()
    {
        //一定時間後に自動的に削除（射程距離）
        Destroy(gameObject, range);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed, 0, 0);
    }

    //レーザーの向きを設定
    public void SetDirection(bool dir)
    {
        if (dir)
        {
            //左向き
            speed = Mathf.Abs(speed) * -1;  //絶対値を取ることで+-をリセットする
        }
        else
        {
            speed = Mathf.Abs(speed);
        }
    }
}
