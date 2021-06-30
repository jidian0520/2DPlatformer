using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カエルの動き制御
/// </summary>
public class FrogController : EnemyDirector
{
    //変数宣言
    Animator animator;
    Rigidbody2D rbody;
    Transform player;
    float intervalTime;     //ジャンプする間隔時間
    float direction;        //移動する方向                          
    FieldViewController fieldViewController;//視野スクリプト

    // Start is called before the first frame update
    void Start()
    {
        //親クラスStartを呼び出す
        base.Start();

        //初期化
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        fieldViewController = transform.Find("FieldViewFrog").GetComponent<FieldViewController>();

        //HPセット
        SetHp(2);
    }

    // Update is called once per frame
    void Update()
    {
        //ジャンプする間隔を計測
        if (intervalTime < 0)
        {
            //プレイヤーが視野内であればジャンプ
            if (fieldViewController.viewFlag)
            {
                Jump();
            }
            intervalTime = Random.Range(1f, 3f);
        }
        else
        {
            intervalTime -= Time.deltaTime;
        }
    }

    //ジャンプ
    private void Jump()
    {
        //向きの設定
        if (player.position.x > transform.position.x)
        {
            direction = 1f;
            //左右反転
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            direction = -1f;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        //ジャンプアニメーション
        animator.SetTrigger("jumpTrigger");
        //ジャンプする力を加える
        rbody.AddForce(new Vector2(direction, 2), ForceMode2D.Impulse);
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
