using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// シーン遷移時のアニメーションイベント
/// </summary>
public class SceneTransitionEventHandler : MonoBehaviour
{
    //アニメーション終了後に実行する処理
    public Action CompleteAction;

    //アニメーション時間
    public float time;

    //アニメーション開始フラグ
    public bool isFadeOut;
    public bool isFadeIn;

    // Update is called once per frame
    void Update()
    {
        //フェードイン処理
        if (isFadeIn)
        {
            //time時間かけて徐々に透明にする
            GetComponent<CanvasGroup>().alpha -= Time.deltaTime / time;
            if (GetComponent<CanvasGroup>().alpha <= 0)
            {
                isFadeIn = false;
                //完全に透明になったらPanelオブジェクトは削除
                Destroy(gameObject, 0.5f);
                //遷移が完了したら呼び出される処理
                CompleteAction();
            }
        }

        //フェードアウト処理
        if (isFadeOut)
        {
            GetComponent<CanvasGroup>().alpha += Time.deltaTime / time;
            if (GetComponent<CanvasGroup>().alpha >= 1)
            {
                isFadeOut = false;
                Destroy(gameObject, 0.5f);
                CompleteAction();
            }
        }
    }
}

/// <summary>
/// シーン遷移の制御
/// </summary>
public static class SceneTransitionManager
{
    //イベントを制御するハンドラーを設定するメソッド
    private static SceneTransitionEventHandler SetUpEventHandler(GameObject target)
    {
        SceneTransitionEventHandler eventHandler = target.AddComponent<SceneTransitionEventHandler>();
        return eventHandler;
    }

    public static IEnumerator FadeIn(GameObject target, float time, float delay, Action action = null)
    {
        //イベントを制御するハンドラーを設置する
        SceneTransitionEventHandler eventHandler = SetUpEventHandler(target.gameObject);
        //アニメーション時間セット
        eventHandler.time = time;
        //透過度初期化
        target.GetComponent<CanvasGroup>().alpha = 1f;
        //遅延処理
        yield return new WaitForSeconds(delay);
        //イベント発動
        eventHandler.isFadeIn = true;
        eventHandler.CompleteAction = action;
    }

    public static IEnumerator FadeOut(GameObject target, float time, float delay, Action action = null)
    {
        //イベントを制御するハンドラーを設置する
        SceneTransitionEventHandler eventHandler = SetUpEventHandler(target.gameObject);
        //アニメーション時間セット
        eventHandler.time = time;
        //透過度初期化
        target.GetComponent<CanvasGroup>().alpha = 0f;
        //遅延処理
        yield return new WaitForSeconds(delay);
        //イベント発動
        eventHandler.isFadeOut = true;
        eventHandler.CompleteAction = action;
    }
}