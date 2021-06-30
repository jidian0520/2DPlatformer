using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵キャラの共通処理をまとめた親クラス
/// </summary>
public class EnemyDirector : MonoBehaviour
{
    //現在のHP
    protected int currentHp;
    //最大HP
    protected int maxHp;
    //HPバー
    protected Slider hpBar;

    protected void Start()
    {
        //HPBarの取得
        hpBar = transform.Find("HPBar/Slider").GetComponent<Slider>();
        hpBar.value = 1f;
    }

    //HPを初期化
    protected void SetHp(int hp)
    {
        maxHp = hp;
        currentHp = hp;
    }

    //ダメージを受けた
    //戻り値がtrueの場合はHP0で消滅
    protected bool Damage(int d)
    {
        currentHp -= d;
        GSound.Instance.PlaySe("enemy_damage");
        //HPBarの更新
        hpBar.value = (float)currentHp / (float)maxHp;
        if (currentHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
