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

public class EnemyGunScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("威力")]
    private float _power = default;
    [SerializeField, Header("生成する数")]
    private int _maxCount = 100;
    [SerializeField, Header("生成する弾")]
    private EnemyBallScript _ballScript = default;
    [SerializeField, Header("射撃のクールタイム")]
    private float _shotCoolTime = 2f;
    [SerializeField, Header("与えるダメージ")]
    private int _damege = 20;
    //射撃のカウント
    private float _shotTime = default;
    //プール用のQueue
    private Queue<EnemyBallScript> _ballQueue = default;
    #endregion
    public float GetPower { get => _power; }
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //プール生成
        _ballQueue = new Queue<EnemyBallScript>();
        //最大数作る
        for (int i = 0; i < _maxCount; i++)
        {
            //生成
            EnemyBallScript ball = Instantiate(_ballScript, transform.position, Quaternion.identity);
            //Queueに追加
            _ballQueue.Enqueue(ball);
            //非表示
            ball.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 弾を表示
    /// </summary>
    /// <param name="playerPosition">プレイヤーの向き</param>
    /// <param name="enemyPositon">発射位置</param>
    public void Ballistic(Vector3 playerPosition, Vector3 enemyPositon)
    {
        //クールタイム加算
        _shotTime += Time.deltaTime;
        //クールタイムならリターン
        if (_shotTime < _shotCoolTime)
        {
            return;
        }
        //タイマー初期化
        _shotTime = 0;
        //Queueの中になかったら生成
        if (_ballQueue.Count <= 0)
        {
            //生成
            EnemyBallScript ball = Instantiate(_ballScript, transform.position, Quaternion.identity);
            //Queueに追加
            _ballQueue.Enqueue(ball);
            //非表示
            ball.gameObject.SetActive(false);
        }
        //弾を取り出す
        EnemyBallScript ballScript = _ballQueue.Dequeue();
        //弾を表示
        ballScript.gameObject.SetActive(true);
        //発射位置に移動
        ballScript.transform.position = enemyPositon;
        //方向を決定
        ballScript.SetVelocity(playerPosition,enemyPositon);
        ////ダメージ設定
        ballScript.SetShootDamege(_damege);
    }
    /// <summary>
    /// 弾を格納する
    /// </summary>
    /// <param name="ballScript">格納する弾</param>
    public void BallCollect(EnemyBallScript ballScript)
    {
        //弾のゲームオブジェクトを非表示
        ballScript.gameObject.SetActive(false);
        //Queueに格納
        _ballQueue.Enqueue(ballScript);
    }
}

