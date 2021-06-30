using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イーグルの動き制御
/// </summary>
public class EagleController : EnemyDirector
{
    //変数宣言
    Rigidbody2D rbody;
    Transform player;
    float direction = -1;           //移動する方向
    float speed = 1f;               //移動スピード
    FieldViewController fieldViewController;//視野スクリプト

    // Start is called before the first frame update
    void Start()
    {
        //親クラスStartを呼び出す
        base.Start();

        //初期化
        rbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        fieldViewController = transform.Find("FieldViewEagle").GetComponent<FieldViewController>();

        //HPセット
        SetHp(3);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーとの距離
        if (Mathf.Abs(player.position.x - transform.position.x) > 2.5f)
        {
            if (player.position.x > transform.position.x)
            {
                direction = 1f;
                //右向きへ反転
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                direction = -1f;
                //左向きへ反転
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    //一定間隔でずっと繰り返す
    void FixedUpdate()
    {
        //左右方向へ力を加える
        rbody.MovePosition(transform.position + transform.right * direction * speed * Time.deltaTime);
    }

    //衝突判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        //レーザーと衝突
        if (other.gameObject.tag == "Laser")
        {
            //レーザーはその場で消す
            Destroy(other.gameObject);
            //ダメージを与える
            if (Damage(1))
            {
                //HPが0になったので消滅処理
                Destroy(gameObject);
                //敵キャラと同じ座標に敵死亡時エフェクト（EnemyDeath）を生成する
                Instantiate(Resources.Load("Prefabs/EnemyDeathPrefab"), transform.position, Quaternion.identity);
                GSound.Instance.PlaySe("enemy_die");
            }
            else
            {
                //敵キャラと同じ座標に敵ダメージエフェクト（EnemyDamage）を生成する
                Instantiate(Resources.Load("Prefabs/EnemyDamagePrefab"), transform.position, Quaternion.identity);
            }
        }
    }
}
