using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BGM、SEを管理するクラス
/// </summary>
public class GSound
{
    //シングルトン化
    private static GSound instance;
    public static GSound Instance {
        get {
            if(instance == null) instance = new GSound();
            return instance;
        }
    }

    //AudioClipをロードするクラス
    class ClipData {
        //AudioClip
        public AudioClip Clip;
        //コンストラクタ
        public ClipData(string fileName) {
            //AudioClipの取得
            Clip = Resources.Load("Sounds/" + fileName) as AudioClip;
        }
    }


    ///サウンド種別
    enum Type {
        bgm,    //BGM
        se,     //SE
    }

    //サウンド再生のためのゲームオブジェクト
    GameObject obj;

    //サウンドリソース
    AudioSource sourceBgm;  //BGM
    AudioSource sourceSe;   //SE

    //BGMデータプール
    Dictionary<string, ClipData> poolBgm = new Dictionary<string, ClipData>();
    //SEデータプール 
    Dictionary<string, ClipData> poolSe = new Dictionary<string, ClipData>();

    public float bgmVolume;
    public float seVolume;

    //初期化
    public GSound() {
        bgmVolume = PlayerPrefs.GetFloat("BgmVolume", 1.0f);
        seVolume = PlayerPrefs.GetFloat("SeVolume", 1.0f);
    }


    //AudioSourceを取得する
    AudioSource GetAudioSource(Type type) {

        //GameObjectがなければ作る
        if(obj == null) {
            obj = new GameObject("Sound");

            //破棄しないようにする
            //GameObject.DontDestroyOnLoad(obj);

            //AudioSource作成
            sourceBgm = obj.AddComponent<AudioSource>();
            sourceSe = obj.AddComponent<AudioSource>();
        }

        if(type == Type.bgm) {
            //BGM
            return sourceBgm;
        } else {
            //SE
            return sourceSe;
        }
    }


    //BGMをプールにセット
    //***Resources/Soundsフォルダに配置すること
    public void SetBgm(string fileName) {

        //すでに登録済みなのでいったん消す
        if(poolBgm.ContainsKey(fileName)) {
            poolBgm.Remove(fileName);
        }

        poolBgm.Add(fileName, new ClipData(fileName));
    }


    //SEをプールにセット
    //***Resources/Soundsフォルダに配置すること
    public void SetSe(string fileName) {

        //すでに登録済みなのでいったん消す
        if(poolSe.ContainsKey(fileName)) {
            poolSe.Remove(fileName);
        }

        poolSe.Add(fileName, new ClipData(fileName));
    }


    //BGMの再生
    //***事前にLoadBgmでロードしておくこと
    public bool PlayBgm(string fileName, bool loop) {

        //指定フィアルがない
        if(poolBgm.ContainsKey(fileName) == false) return false;

        //現在のBGMを止める
        StopBgm();

        //リソースの取得
        AudioSource source = GetAudioSource(Type.bgm);
        ClipData data = poolBgm[fileName];
        source.clip = data.Clip;

        //音量設定
        source.volume = bgmVolume;

        //ループ設定
        if(loop) {
            source.loop = true;
        } else {
            source.loop = false;
        }

        //再生
        source.Play();

        return true;
    }


    //SEの再生
    //***事前にLoadSeでロードしておくこと
    public bool PlaySe(string fileName) {

        //指定フィアルがない
        if(poolSe.ContainsKey(fileName) == false) return false;

        //リソースの取得
        AudioSource source = GetAudioSource(Type.se);
        ClipData data = poolSe[fileName];

        //音量設定
        source.volume = seVolume;

        //再生
        source.PlayOneShot(data.Clip);

        return true;
    }


    //BGMの停止
    public void StopBgm() {
        GetAudioSource(Type.bgm).Stop();
    }

    //BGMの音量変更
    public void BgmVolumeChange() {
        GetAudioSource(Type.bgm).volume = bgmVolume;
    }

}
