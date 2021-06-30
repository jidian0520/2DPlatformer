using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //変数宣言
    Animator animator;

    Rigidbody2D rbody;
    public float speed = 3f;         //移動スピード
    public float jumpPower = 8f;     //ジャンプ力
    float vx;                        //現在の移動速度

    bool ladderFlag;                 //はしごと接触時フラグ
    float ladderX;                   //接触しているはしごのx座標
    float vy;                        //はしご昇降時上下の移動速度

    //UI
    GameObject gameOverText;            //ゲームオーバーUI

    //ブラスター
    GameObject blaster;                 //ブラスターオブジェクトを参照
    float blasterTime = 0.5f;           //ブラスターの連射間隔時間
    float blasterDelta;                 //連射間隔を計測する変数
    GameObject laserPrefab;

    //HP
    int currentHp;
    int maxHp = 5;
    GameObject playerHp;
    GameObject heartPrefab;
    float damageTime = 1f;              //ダメージ後の無敵時間
    float damageDelta;                  //ダメージ後の計測時間

    //ハートのスプライト切り替え用
    Texture2D heart0;
    Texture2D heart1;

    //死亡時アニメーション
    bool dieFlg;

    void Start()
    {
        //初期化
        animator = GetComponent<Animator>();
        //初期化でRigidbody2Dの紐付けを行います
        rbody = GetComponent<Rigidbody2D>();
        //ゲームオーバーUIの紐付け
        gameOverText = GameObject.Find("GameOver");
        //非表示にする
        gameOverText.SetActive(false);
        //ブラスター取得
        blaster = transform.Find("Blaster").gameObject;
        blaster.SetActive(false);
        //レーザー取得
        laserPrefab = Resources.Load("Prefabs/LaserPrehab") as GameObject;

        //HP関連
        currentHp = maxHp;
        playerHp = GameObject.Find("PlayerHP");
        heartPrefab = Resources.Load("Prefabs/HeartPrefab") as GameObject;
        //ハートのスプライト切り替え用
        heart0 = Resources.Load("Sprites/Heart0") as Texture2D;
        heart1 = Resources.Load("Sprites/Heart1") as Texture2D;
        //ハートの更新
        SetHeart();
    }

    void Update()
    {
        //待機状態にリセット
        if (!Input.anyKey)
        {
            animator.SetBool("runFlag", false);
            animator.SetBool("crouchFlag", false);
            //はしご昇降時ははしごアニメをリセットさせない
            if (!ladderFlag)
            {
                animator.SetBool("climbFlag", false);
            }
            vx = 0;
            //何もキーが押されていない間は上下移動の速度を0にする
            vy = 0;
        }

        //右移動
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("runFlag", true);
            //左右反転
            GetComponent<SpriteRenderer>().flipX = false;
            //速度設定
            vx = speed;
        }

        //左移動
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("runFlag", true);
            //左右反転
            GetComponent<SpriteRenderer>().flipX = true;
            //速度設定
            vx = -speed;
        }

        //しゃがむ・はしごを下る
        if (Input.GetKey(KeyCode.S))
        {
            //はしごを下る処理
            if (ladderFlag)
            {
                animator.SetBool("climbFlag", true);
                //速度設定
                vx = 0;
                vy = -speed;
                //はしごにピタッと合わせる
                Vector2 pos = transform.position;
                pos.x = ladderX;
                transform.position = pos;
            }
            else
            {
                //しゃがむアニメーション設定
                animator.SetBool("crouchFlag", true);
                //速度設定
                vx = 0;
            }

        }

        //登る
        if (Input.GetKey(KeyCode.W))
        {
            if (ladderFlag)
            {
                //アニメーション設定
                animator.SetBool("climbFlag", true);
                //速度設定
                vx = 0;
                vy = speed;
                //はしごにピタッと合わせる
                Vector2 pos = transform.position;
                pos.x = ladderX;
                transform.position = pos;
            }

        }

        //ジャンプ（押しっぱなし防止の為KyeDownにする）
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("jumpTrigger");
            //ジャンプ中でなければ（多重ジャンプ防止）
            if (Mathf.Abs(rbody.velocity.y) < 0.01f)
            {
                //上方向に力を加える
                rbody.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
                //SE
                GSound.Instance.PlaySe("jump");
            }
        }

        //攻撃（ブラスター）
        if (Input.GetKeyDown(KeyCode.J))
        {
            //連射防止
            if (blasterDelta > 0) return;

            //連射防止時間をセット
            blasterDelta = blasterTime;
            //ブラスター表示
            blaster.SetActive(true);
            //ブラスターの向き
            if (GetComponent<SpriteRenderer>().flipX)
            {
                //左向き
                blaster.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                //右向き
                blaster.GetComponent<SpriteRenderer>().flipX = false;
            }

            //レーザー発射（ブラスターと同じ座標で生成）
            GameObject laser = Instantiate(laserPrefab, transform.Find("Blaster").transform.position, Quaternion.identity);
            //レーザーの向きを設定
            laser.GetComponent<LaserController>().SetDirection(GetComponent<SpriteRenderer>().flipX);
            //SE
            GSound.Instance.PlaySe("blaster");
        }
    }

    //一定間隔(一秒間50回)でずっと繰り返す
    void FixedUpdate()
    {
        //死亡時は横へのスピードを0にして真上に飛び上がるようにする
        if (dieFlg)
        {
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            return;
        }

        //はしごを上り下りしている場合
        if (ladderFlag)
        {
            rbody.velocity = new Vector2(vx, vy);
            //はしごを登るとき重力をなしにする
            rbody.gravityScale = 0;
        }
        else
        {
            //横移動させる（重力をかけたまま）
            rbody.velocity = new Vector2(vx, rbody.velocity.y);
            rbody.gravityScale = 1;
        }

        //連射間隔時間の更新
        if (blasterDelta > 0)
        {
            blasterDelta -= Time.fixedDeltaTime;
        }
        else
        {
            blaster.SetActive(false);
        }

        //ダメージ後の無敵時間更新
        if (damageDelta > 0)
        {
            damageDelta -= Time.fixedDeltaTime;
            //無敵時間が終わりかけたら透明状態を元に戻す、連続ダメージ受けないため
            if (damageDelta < damageTime * 0.2f)
            {
                Color color = GetComponent<SpriteRenderer>().color;
                color.a = 1f;
                GetComponent<SpriteRenderer>().color = color;
                //レイヤーを元に戻す
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }

    }

    ////何かに触れた
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    //はしごと接触
    //    if (other.gameObject.tag == "Ladder")
    //    {
    //        ladderFlag = true;
    //        //接触したはしごのx座標取得
    //        ladderX = other.transform.position.x;
    //    }
    //}

    //何かと触れている（追加）
    private void OnTriggerStay2D(Collider2D other)
    {
        //はしごと接触
        if (other.gameObject.tag == "Ladder")
        {
            ladderFlag = true;
            //接触したはしごのx座標取得
            ladderX = other.transform.position.x;
        }
    }

    //何かに触れた
    private void OnTriggerEnter2D(Collider2D other)
    {
        //ゴールジェムと接触
        if (other.gameObject.tag == "Goal")
        {
            Destroy(other.gameObject);
            //Debug.Log("ゲームクリアしました！！！");
            //ゲームクリアシーンへ遷移（追加）
            SceneManager.LoadScene("GameClearScene");
        }

        //ライフチェリーと接触
        if (other.gameObject.tag == "Life")
        {
            Destroy(other.gameObject);
            //ライフ回復メソッドを呼び出す
            LifeRecovery();
        }
    }

    //何かから離れた
    private void OnTriggerExit2D(Collider2D other)
    {
        //はしごから離れた
        if (other.gameObject.tag == "Ladder")
        {
            ladderFlag = false;
        }
    }

    ////何かとぶつかった
    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    //敵と衝突
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        //ゲームオーバーUIの表示
    //        gameOverText.SetActive(true);

    //        //1秒後にゲームリセット（遅延処理）
    //        Invoke("Reset", 1f);
    //    }
    //}

    //何かとぶつかっている間
    private void OnCollisionStay2D(Collision2D other)
    {
        //敵と衝突
        if (other.gameObject.tag == "Enemy")
        {
            //死亡時はダメージを受け付けない
            if (dieFlg) return;

            //無敵時間中はダメージを受けない
            if (damageDelta > 0) return;

            //HPを減らす
            currentHp--;
            GSound.Instance.PlaySe("player_damage");
            //無敵時間セット
            damageDelta = damageTime;
            SetHeart();
            //ダメージアニメーションセット
            animator.SetTrigger("hurtTrigger");
            //ダメージ時にびっくりさせる（小ジャンプ）
            rbody.AddForce(transform.up * 2.5f, ForceMode2D.Impulse);
            //ダメージ時の無敵時間中は半透明にする
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            GetComponent<SpriteRenderer>().color = color;
            //ダメージ時はレイヤーを変更して敵と衝突しないようにする
            gameObject.layer = LayerMask.NameToLayer("Damage");

            //HP0でゲームオーバー
            if (currentHp <= 0)
            {
                //ゲームオーバーUIの表示
                gameOverText.SetActive(true);

                //1秒後にゲームリセット（遅延処理）
                Invoke("Reset", 2f);

                //死亡フラグ
                dieFlg = true;
                //死亡時アニメーション
                animator.SetTrigger("dieTrigger");
                //大きめに飛び上らせる
                rbody.AddForce(transform.up * 3f, ForceMode2D.Impulse);
                //コライダーを停止して落下させる
                GetComponent<CapsuleCollider2D>().enabled = false;
            }
            else
            {
                //SE
                GSound.Instance.PlaySe("player_damage");
            }
        }
    }

    //ゲームをリセット
    private void Reset()
    {
        //SceneManager.LoadScene("GameScene");
        //ゲームオーバーシーンへ遷移（追加）
        SceneManager.LoadScene("GameOverScene");
    }

    //ハートの表示更新
    void SetHeart()
    {
        //いったん全てのハートプレハブを削除する
        foreach (Transform child in playerHp.transform)
        {
            Destroy(child.gameObject);
        }

        ////HPの数だけハートプレハブをPlayerHPの子オブジェクトとして生成する
        //for (int i = 0; i < currentHp; i++)
        //{
        //    Instantiate(heartPrefab, playerHp.transform);
        //}

        //最大HPの数だけハートプレハブをPlayerHPの子オブジェクトとして生成する
        for (int i = 0; i < maxHp; i++)
        {
            GameObject heart = Instantiate(heartPrefab, playerHp.transform);
            //現在HPより大きいHPのハートは空ハートの画像に切り替える
            if (currentHp <= i)
            {
                heart.GetComponent<Image>().sprite = Sprite.Create(heart0, new Rect(0, 0, heart0.width, heart0.height), Vector2.zero);
            }
        }
    }

    //ライフ回復
    void LifeRecovery()
    {
        //HPを1回復
        currentHp++;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        //ハート表示を更新
        SetHeart();
        //SE
        GSound.Instance.PlaySe("coin");
    }
}
