/*
*　　説明　
*　　日付
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
    [SerializeField, Header("移動方向")]
    private Vector3[] _movePosition = default;
    //敵要の銃スクリプト
    private EnemyGunScript _enemyGunScript = default;
    //ポジションの配列のインデックス
    private int _currentIndex = 0;
    //プレイヤーの向き
    private Vector3 _playerVelocity = default;
    //プレイヤーのポジション
    private Transform _playerPosition = default;
    //現在のステータス
    private EenemyStatus _nowStatus = EenemyStatus.Move;

    private enum EenemyStatus
    {
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
        //敵の銃取得
        _enemyGunScript = GameObject.FindWithTag("EnemyController").GetComponent<EnemyGunScript>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //プレイヤーを見つけたか
        if (CanSeePlayer())
        {
            //射撃のステータス変更
            _nowStatus = EenemyStatus.Shot;
        }
        else
        {
            _nowStatus = EenemyStatus.Move;
        }
        switch (_nowStatus)
        {
            case EenemyStatus.Shot:
                //射撃する
                _enemyGunScript.Ballistic(_playerVelocity + Vector3.up, transform.position);
                break;
            case EenemyStatus.Move:
                //移動先がなければリターン
                if (_movePosition.Length == 0)
                {
                    return;
                }
                //移動させる
                transform.position = Vector3.MoveTowards(transform.position, _movePosition[_currentIndex], _moveSpeed * Time.deltaTime);
                //移動が終わったか
                if (transform.position == _movePosition[_currentIndex])
                {
                    //インデックス増加
                    _currentIndex++;
                    //_movePositionの長さを超えたら最初に戻る
                    if (_currentIndex >= _movePosition.Length)
                    {
                        _currentIndex = 0;
                    }
                }
                break;
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
        Debug.DrawRay(transform.position + Vector3.up, _playerVelocity.normalized * _sightRange, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up, _playerVelocity.normalized, out hit, _sightRange))
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
    /// <summary>
    /// HPを減らす
    /// </summary>
    /// <param name="damege">減らす量</param>
    public void DownHp(int damege)
    {
        //HPを減らす
        _nowHp -= damege;
        //HPが0いかになったら消滅
        if (_nowHp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}