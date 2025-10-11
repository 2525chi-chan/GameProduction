//作成者　寺村
//｛https://bluebirdofoz.hatenablog.com/entry/2023/03/28/224836｝を参考に作成

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// 固定軸の定義
    private enum LockAxis
    {
        None,
        X,
        Y,
    }

    [Header("固定する回転軸の指定")]
    [SerializeField] private LockAxis lockAxis;

    [Header("UIの場合はZ方向が反転するため、チェックボックスにチェックを入れてください")]
    [SerializeField] private bool reverseFront;

    Transform mainCamera;   //常に正面にさせるカメラの座標

    private void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();    //Scene内からMainCameraを参照
    }

    private void Update()
    {
        // 現オブジェクトからメインカメラ方向のベクトルを取得する
        Vector3 direction = mainCamera.transform.position - this.transform.position;

        // ベクトルの固定軸を考慮する
        Vector3 lockDirection = lockAxis switch
        {
            // ロック軸なしの場合はベクトルをそのまま利用する
            LockAxis.None => direction,
            // X軸固定の場合はX軸方向のベクトルの変量を0にする
            LockAxis.X => new Vector3(0.0f, direction.y, direction.z),
            // Y軸固定の場合はY軸方向のベクトルの変量を0にする
            LockAxis.Y => new Vector3(direction.x, 0.0f, direction.z),
            _ => throw new ArgumentOutOfRangeException()
        };

        // オブジェクトをベクトル方向に従って回転させる
        // (正面方向を逆転する場合はベクトルにマイナスをかける)
        transform.rotation = Quaternion.LookRotation(reverseFront ? -lockDirection : lockDirection);
    }
}
