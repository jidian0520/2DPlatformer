using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Soundデータ初期化
/// </summary>
public class SoundController : MonoBehaviour {

    //Startメソッドよりも先にサウンドデータを読み込む
    void Awake() {
        //SE
        GSound.Instance.SetSe("jump");
        GSound.Instance.SetSe("blaster");
        GSound.Instance.SetSe("player_damage");
        GSound.Instance.SetSe("player_die");
        GSound.Instance.SetSe("enemy_damage");
        GSound.Instance.SetSe("enemy_die");
        GSound.Instance.SetSe("boss_die");
        GSound.Instance.SetSe("weapon_get");
        //BGM
        GSound.Instance.SetBgm("BGM_Field");
    }
}
