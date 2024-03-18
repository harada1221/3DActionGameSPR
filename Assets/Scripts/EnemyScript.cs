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
    [SerializeField, Header("発射位置")]
    private Transform _shotPosition = default;
    [SerializeField, Header("プレイヤーのレイヤー")]
    private LayerMask _targetLayer = default;
    [SerializeField, Header("視野角")]
    private float _sightAngle = 30;
    //敵要の銃スクリプト
    private EnemyGunScript _enemyGunScript = default;
    //敵のアニメータ
    private Animator _animator = default;
    //ポジションの配列のインデックス
    private int _currentIndex = 0;
    //プレイヤーの向き
    private Vector3 _playerVelocity = default;
    //プレイヤーのポジション
    private Transform _playerPosition = default;
    //現在のステータス
    private EenemyStatus _nowStatus = EenemyStatus.Idle;

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
        //敵の銃取得
        _enemyGunScript = GameObject.FindWithTag("EnemyController").GetComponent<EnemyGunScript>();
        //アニメーター取得
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //プレイヤーを見つけたか
        if (CanSeePlayer() && IsVisible())
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
            case EenemyStatus.Idle:
                _animator.SetBool("Idle", true);
                _animator.SetBool("Shot", false);
                _animator.SetBool("Walk", false);
                break;
            case EenemyStatus.Shot:
                //射撃する
                _enemyGunScript.Ballistic(_playerVelocity - Vector3.up * 1.5f, _shotPosition.position);
                //ベクトルがゼロでないことを確認
                if (_playerVelocity != Vector3.zero)
                {
                    Quaternion newRotation = Quaternion.LookRotation(_playerVelocity, Vector3.up);
                    newRotation.x = 0;
                    newRotation.z = 0;
                    transform.rotation = newRotation;
                }
                _animator.SetBool("Shot", true);
                _animator.SetBool("Idle", false);
                _animator.SetBool("Walk", false);
                break;
            case EenemyStatus.Move:
                //移動先がなければリターン
                if (_movePosition.Length == 0)
                {
                    _nowStatus = EenemyStatus.Idle;
                    _animator.SetBool("Idle", true);
                    _animator.SetBool("Walk", false);
                    _animator.SetBool("Shot", false);
                    return;
                }
                _animator.SetBool("Idle", false);
                _animator.SetBool("Shot", false);
                _animator.SetBool("Walk", true);
                //移動させる
                transform.position = Vector3.MoveTowards(transform.position, _movePosition[_currentIndex], _moveSpeed * Time.deltaTime);
                Vector3 direction = (_movePosition[_currentIndex] - transform.position).normalized;
                if (direction != Vector3.zero) // ベクトルがゼロでないことを確認
                {
                    Quaternion newRotation = Quaternion.LookRotation(direction, Vector3.up);
                    newRotation.x = 0;
                    newRotation.z = 0;
                    transform.rotation = newRotation;
                }
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
        Debug.DrawRay(transform.position + Vector3.up / 2, _playerVelocity.normalized * _sightRange, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up / 2, _playerVelocity.normalized, out hit, _sightRange, _targetLayer))
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

        return false;

    }
    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public bool IsVisible()
    {
        //自身の向き（正規化されたベクトル）
        Vector3 selfDir = transform.forward;

        //ターゲットまでの向きと距離計算
        Vector3 targetDir = _playerPosition.position - transform.position;
        float targetDistance = targetDir.magnitude;

        //視界角度の半分に対応するcos値を計算
        float cosHalfSightAngle = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        //自身とターゲットへの向きの内積計算
        float innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        //視界判定
        return innerProduct > cosHalfSightAngle && targetDistance < _sightRange;
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