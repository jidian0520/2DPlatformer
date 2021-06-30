using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームスタートとなるオブジェクトの制御
/// プレイヤーを生成する
/// </summary>
public class StartPointController : MonoBehaviour
{
    GameObject playerPrefab;

    //他のスクリプトのStartメソッドよりも先に処理
    void Awake()
    {
        //プレイヤープレハブを取得
        playerPrefab = Resources.Load("Prefabs/PlayerPrefab") as GameObject;
        //プレイヤープレハブからプレイヤーをインスタント化する
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }
}
