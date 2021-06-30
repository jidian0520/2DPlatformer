using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定した敵キャラを生成するスポーン（卵）
/// </summary>
public class EnemySpawnController : MonoBehaviour
{
    //敵キャラ名
    [SerializeField]
    private EnemyName enemyName;

    //敵キャラ名の列挙型
    public enum EnemyName
    {
        Frog,
        Eagle,
        Opossum
    }

    //対象敵キャラのプレファブ
    GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //モンスター名から生成する敵プレハブを取得
        enemyPrefab = Resources.Load("Prefabs/Enemy/" + enemyName + "Prefab") as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーがコライダーの範囲内に入った
        if (other.gameObject.tag == "Player")
        {
            //対象の敵キャラを生成
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            //スポーンオブジェクトは削除
            Destroy(gameObject);
        }
    }
}
