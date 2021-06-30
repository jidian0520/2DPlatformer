using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションタイプのエフェクトを自動削除するスクリプト
/// </summary>
public class EffectController : MonoBehaviour
{
    //対象アニメーション名
    [SerializeField] AnimationClip animationName;
    //アニメーション時間
    float animationTime;

    // Start is called before the first frame update
    void Start()
    {
        //アニメーター取得
        Animator animator = GetComponent<Animator>();
        //アニメーターに紐づいているアニメーションクリップをすべて取得
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        //アニメーションクリップを１つ１つ取り出す
        foreach (AnimationClip clip in clips)
        {
            //対象アニメーション名と一致するクリップのアニメーション時間を取得
            if (clip.name == animationName.name)
            {
                animationTime = clip.length;
            }
        }
        //アニメーション時間が終わったら自身を削除
        Destroy(gameObject, animationTime);
    }
}
