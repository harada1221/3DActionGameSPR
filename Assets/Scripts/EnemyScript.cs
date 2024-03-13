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

public class EnemyScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("視界の範囲")]
    private float _sightRange = 10f;
    [SerializeField, Header("移動スピード")]
    private float _moveSpeed = 5f;
    [SerializeField, Header("HP")]
    private int _nowHp = 100;
    //
    private Vector3 _playerVelocity = default;
    //プレイヤーのポジション
    private Transform _playerPosition = default;

    private enum EenemyStatus
    {
        Idle,
        Shot,
        Move
    }
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //プレイヤーの位置を取得
        _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //プレイヤーが視界内にいるかをチェック
        if (CanSeePlayer())
        {
            //プレイヤーを見つけた場合の処理
            Debug.Log("Player spotted!");
        }
    }
    /// <summary>
    /// 視界の範囲内にプレイヤーがいるかどうか
    /// </summary>
    /// <returns>視界にいるか</returns>
    private bool CanSeePlayer()
    {
        //プレイヤーとの間にレイキャストを飛ばし、障害物がなければ true を返す
        RaycastHit hit;
        _playerVelocity = _playerPosition.position - transform.position;
        if (Physics.Raycast(transform.position, _playerVelocity, out hit, _sightRange))
        {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
            //プレイヤー以外の障害物に当たった場合
            else
            {
                return false;
            }
        }
        //レイが何にも当たらなかった場合
        return false;
    }
}

