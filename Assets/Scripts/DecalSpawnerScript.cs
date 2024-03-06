/*
*　　説明　
*　　日付
*
*
*
*　　原田　智大
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSpawnerScript  : MonoBehaviour
{
    public GameObject decalPrefab; // デカールのプレハブ

    public void OnCollisionEnter(Collision collision)
    {
        // 衝突点と法線を取得
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;
        Vector3 normal = contact.normal;

        // デカールを生成して配置
        GameObject decal = Instantiate(decalPrefab, position, Quaternion.identity);
        decal.transform.forward = normal; // デカールを法線方向に向ける
    }
}

