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

public class BombControlScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("発射位置")]
    private Transform _shootPosition = default;
    [SerializeField, Header("生成する数")]
    private int _maxCount = 5;
    [SerializeField, Header("メインカメラ")]
    private Camera _mainCamera = default;
    [SerializeField, Header("生成する爆弾")]
    private BombScript _bomScript = default;
    [SerializeField, Header("インクの減少量")]
    private float _tankdecrease = 70;
    //インクタンクのスクリプト
    private TankScript _tankScript = default;
    //投げるの方向
    private Vector3 _finalDestination = default;
    //プール用のQueue
    private Queue<BombScript> _bombQueue = default;
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        _tankScript = GetComponent<TankScript>();
        //プール生成
        _bombQueue = new Queue<BombScript>();
        //最大数作る
        for (int i = 0; i < _maxCount; i++)
        {
            //生成
            BombScript bomb = Instantiate(_bomScript, _shootPosition.transform.position, Quaternion.identity);
            //Queueに追加
            _bombQueue.Enqueue(bomb);
            //非表示
            bomb.gameObject.SetActive(false);
        }
    }
    public void Bombistic()
    {
        //消費量以上ならリターン
        if (_tankScript.GetNowCapacity < _tankdecrease)
        {
            return;
        }
        if (_bombQueue.Count <= 0)
        {
            //生成
            BombScript bomb = Instantiate(_bomScript, _shootPosition.transform.position, Quaternion.identity);
            //Queueに追加
            _bombQueue.Enqueue(bomb);
            //非表示
            bomb.gameObject.SetActive(false);
        }
        //弾を取り出す
        BombScript bombScript = _bombQueue.Dequeue();
        //弾を表示
        bombScript.gameObject.SetActive(true);
        //弾の方向を設定
        _finalDestination = default;
        _finalDestination = _mainCamera.transform.forward;
        //発射位置に移動
        bombScript.transform.position = _shootPosition.transform.position;
        Debug.Log(bombScript.transform.position);
        //方向を決定
        bombScript.SetVelocity(_finalDestination);
        //インクタンク減少
        _tankScript.Inkdecrease(_tankdecrease);
    }
    /// <summary>
    /// 弾を格納する
    /// </summary>
    /// <param name="ballScript">格納する弾</param>
    public void BombCollect(BombScript bombScript)
    {
        //弾のゲームオブジェクトを非表示
        bombScript.gameObject.SetActive(false);
        //Queueに格納
        _bombQueue.Enqueue(bombScript);
    }
}

