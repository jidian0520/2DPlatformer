using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面全体を制御する
/// </summary>
public class TitleDirector : MonoBehaviour
{
    //シーン遷移用（追加）
    GameObject tranPanelPrefab;
    GameObject canvas;

    void Start()
    {
        //シーン遷移用オブジェクト取得（追加）
        tranPanelPrefab = Resources.Load("Prefabs/TranPanel") as GameObject;
        canvas = GameObject.Find("Canvas");
    }

    //スタートボタン
    public void OnPressStartBtn()
    {
        ////GameSceneへ遷移
        //SceneManager.LoadScene("GameScene");
        //シーン遷移用オブジェクトのクローンを生成（追加）
        GameObject tranPanel = Instantiate(tranPanelPrefab, canvas.transform);

        //シーン遷移アニメーション終了後にMainシーンへ遷移（ラムダ式）
        StartCoroutine(SceneTransitionManager.FadeOut(tranPanel, 1f, 0.5f, () => {
            //遷移アニメーション終了後に呼び出したい処理
            SceneManager.LoadScene("GameScene");
        }));
    }
}
