using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラの視野を制御する
/// </summary>
public class FieldViewController : MonoBehaviour
{
    public bool viewFlag;       //視野内に入ったかどうかのフラグ

    //視界に何かが進入した
    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーが進入
        if (other.gameObject.tag == "Player")
        {
            viewFlag = true;
        }
    }

    //視界から何かが出ていった
    private void OnTriggerExit2D(Collider2D other)
    {
        //プレイヤーが出て行った
        if (other.gameObject.tag == "Player")
        {
            viewFlag = false;
        }
    }
}
