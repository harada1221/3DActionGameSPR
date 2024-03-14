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

public class PlayerHpScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("プレイヤー最大HP")]
    private int _maxHp = 100;
    //現在のHP
    private int _nowHp = default;

    public int GetNowHp { get => _nowHp; }
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //HP初期化
        _nowHp = _maxHp;
    }
    /// <summary>
    /// HP減少
    /// </summary>
    /// <param name="damege">与えるダメージ</param>
    public void DownNowHp(int damege)
    {
        if (_nowHp <= 0)
        {
            _nowHp = 0;
            return;
        }
        //HP減少
        _nowHp = _nowHp - damege;
    }
    /// <summary>
    /// HP回復
    /// </summary>
    /// <param name="healValue">回復量</param>
    public void HealNowHp(int healValue)
    {
        if (_nowHp >= _maxHp)
        {
            _nowHp = _maxHp;
            return;
        }
        //HP回復
        _nowHp += healValue;
    }
    /// <summary>
    /// HPを最大値まで回復
    /// </summary>
    public void HpMaxHeal()
    {
        _nowHp = _maxHp;
    }
}

