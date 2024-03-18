/*
*　　説明　
*　　日付
*
*
*
*　　原田　智大
*/
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("プレイヤー最大HP")]
    private int _maxHp = 100;
    [SerializeField, Header("回復量")]
    private int _healValue = 10;
    [SerializeField, Header("通常時の回復速度")]
    private float _normalHealTime = 1f;
    [SerializeReference, Header("潜り状態の回復速度")]
    private float _diverHealTime = 0.125f;
    [SerializeField, Header("ダメージUI")]
    private CanvasGroup _damegeUi = default;
    //プレイヤースクリプト
    private PlayerScript _playerScript = default;
    //現在のHP
    private int _nowHp = default;
    //回復までのクールタイム
    private float _damegeCoolTimeCount = default;
    //クールタイムの比較よう
    private float _damegeCoolCheak = default;

    public int GetNowHp { get => _nowHp; }
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //HP初期化
        _nowHp = _maxHp;
        //プレイヤースクリプト
        _playerScript = GetComponent<PlayerScript>();
        //クールタイムの初期設定
        _damegeCoolCheak = _normalHealTime;
        //ダメージUIを透明にする
        _damegeUi.alpha = 0;
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //タイム加算
        _damegeCoolTimeCount += Time.deltaTime;
        if (_damegeCoolTimeCount >= _damegeCoolCheak)
        {
            //タイマー初期化
            _damegeCoolTimeCount = 0;
            //回復
            HealNowHp();
        }
        //回復速度を決める
        if (_playerScript.GetNowStatus == PlayerScript.PlayerStatus.Crouch || _playerScript.GetNowStatus == PlayerScript.PlayerStatus.Diver)
        {
            _damegeCoolCheak = _diverHealTime;
        }
        else
        {
            _damegeCoolCheak = _normalHealTime;
        }
        if (_playerScript.GetEnemyColor == true && _nowHp >= 40)
        {
            DownNowHp(1);
        }
    }
    /// <summary>
    /// HP減少
    /// </summary>
    /// <param name="damege">与えるダメージ</param>
    public void DownNowHp(int damege)
    {
        //HPが最小値か
        if (_nowHp <= 0)
        {
            _nowHp = 0;
            return;
        }
        //回復までの時間を初期化
        _damegeCoolTimeCount = 0;
        //HP減少
        _nowHp = _nowHp - damege;
        DamegeUiChage();
    }
    /// <summary>
    /// HP回復
    /// </summary>
    /// <param name="healValue">回復量</param>
    public void HealNowHp()
    {
        //HPが最大値か
        if (_nowHp >= _maxHp)
        {
            _nowHp = _maxHp;
            return;
        }
        //HP回復
        _nowHp += _healValue;
        DamegeUiChage();
    }
    /// <summary>
    /// HPを最大値まで回復
    /// </summary>
    public void HpMaxHeal()
    {
        _nowHp = _maxHp;
        DamegeUiChage();
    }
    /// <summary>
    /// 現在のHPによってUIのalpha値を変更
    /// </summary>
    private void DamegeUiChage()
    {
        //alpha値変更
        _damegeUi.alpha = 1f - (float)_nowHp / (float)_maxHp;
    }
}

