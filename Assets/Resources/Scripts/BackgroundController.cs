using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景画像をプレイヤーに追従させる
/// </summary>
public class BackgroundController : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
