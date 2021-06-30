using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    //メニューウインドウUI（追加）
    GameObject menuWindow;

    //シーン遷移用（追加）
    GameObject tranPanelPrefab;
    GameObject canvas;

    //（追加）
    private void Awake()
    {

        //シーン遷移用オブジェクト取得（追加）
        tranPanelPrefab = Resources.Load("Prefabs/TranPanel") as GameObject;
        canvas = GameObject.Find("Canvas");

        //シーン遷移用オブジェクト生成（追加）
        GameObject tranPanel = Instantiate(tranPanelPrefab, canvas.transform);

        //シーン遷移アニメーション終了後にBGMを再生させる（ラムダ式）
        StartCoroutine(SceneTransitionManager.FadeIn(tranPanel, 0.5f, 0f, () => {
            //BGM再生
            GSound.Instance.PlayBgm("BGM_Field", true);
        }));
    }

    // Start is called before the first frame update
    void Start()
    {
        ////BGM再生
        //GSound.Instance.PlayBgm("BGM_Field", true);

        //メニューウインドウ（追加）
        menuWindow = GameObject.Find("Menu");
        menuWindow.SetActive(false);
    }

    //メニューボタン（追加）
    public void OnPressMenuBtn()
    {
        //メニューウインドウを表示してゲーム内時間を止める
        menuWindow.SetActive(true);
        Time.timeScale = 0;
    }
}
