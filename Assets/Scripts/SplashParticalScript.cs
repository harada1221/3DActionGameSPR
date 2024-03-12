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

public class SplashParticalScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField,Header("エフェクト")]
    private ParticleSystem _particleSystem = default;
    //エフェクトコントローラー
    private SplashControlScript _controlScript = default;
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //コントローラー取得
        _controlScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SplashControlScript>();
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //エフェクトの再生が終わったら回収する
        if (_particleSystem.isStopped)
        {
            EndSplash();
        }
    }
    /// <summary>
    /// エフェクトを再生する
    /// </summary>
    /// <param name="position">発生位置</param>
    /// <param name="rotation">発生向き</param>
    public void StrartSplash(Vector3 position, Vector3 rotation)
    {
        //エフェクトの向きと位置を設定
        this.transform.position = position;
        this.transform.rotation = Quaternion.LookRotation(rotation);
        //エフェクト開始
        _particleSystem.Play();
    }
    /// <summary>
    /// 回収
    /// </summary>
    private void EndSplash()
    {
        _controlScript.SplashCollect(this);
    }
}

