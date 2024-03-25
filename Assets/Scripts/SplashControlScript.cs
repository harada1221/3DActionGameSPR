/*
*　　説明　エフェクトの生成を管理する
*　　原田　智大
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashControlScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("生成するエフェクト")]
    private SplashParticalScript _particalScript = default;
    [SerializeField, Header("生成する数")]
    private int _maxCount = 20;
    //プール用のQueue
    private Queue<SplashParticalScript> _splashQueue = default;
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //プール生成
        _splashQueue = new Queue<SplashParticalScript>();
        //最大数作る
        for (int i = 0; i < _maxCount; i++)
        {
            //生成
            SplashParticalScript Splash = Instantiate(_particalScript, this.transform.position, Quaternion.identity);
            //Queueに追加
            _splashQueue.Enqueue(Splash);
            //非表示
            Splash.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// エフェクトを発生させる
    /// </summary>
    /// <param name="position">発生位置</param>
    /// <param name="rotation">発生向き</param>
    public void StartEffects(Vector3 position, Vector3 rotation)
    {
        //Queueの中になかったら生成
        if (_splashQueue.Count <= 0)
        {
            //生成
            SplashParticalScript Splash = Instantiate(_particalScript, this.transform.position, Quaternion.identity);
            //Queueに追加
            _splashQueue.Enqueue(Splash);
            //非表示
            Splash.gameObject.SetActive(false);
        }
        //エフェクトを取り出す
        SplashParticalScript ballScript = _splashQueue.Dequeue();
        //エフェクトを表示
        ballScript.gameObject.SetActive(true);
        //エフェクト開始
        ballScript.StrartSplash(position, rotation);
    }
    /// <summary>
    /// オブジェクト回収
    /// </summary>
    /// <param name="splashPartical">エフェクト表示スクリプト</param>
    public void SplashCollect(SplashParticalScript splashPartical)
    {
        //弾のゲームオブジェクトを非表示
        splashPartical.gameObject.SetActive(false);
        //Queueに格納
        _splashQueue.Enqueue(splashPartical);
    }
}

