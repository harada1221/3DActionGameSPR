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

public class ChangeRespwnScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("カメラポジション")]
    private Vector3 _cameraPosition = default;
    [SerializeField, Header("カメラローテンション")]
    private Vector3 _cameraRotation = default;
    [SerializeField, Header("消える時間")]
    private float _fadeOutTime = 3;
    //更新UI
    private CanvasGroup _updatePositionui = default;
    //プレイヤーオブジェクト
    private GameObject _player = default;
    //プレイヤースクリプト
    private PlayerScript _playerScript = default;
    //リスポーン変更判定
    private bool isChageRespwn = false;
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //プレイヤー取得
        _player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤースクリプト取得
        _playerScript = _player.GetComponent<PlayerScript>();
        //表示UI取得
        _updatePositionui = GameObject.FindGameObjectWithTag("RespwnUI").GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        //一定範囲内にいるかかつ1回目か
        if (Vector3.Distance(transform.position, _player.transform.position) < 1 && isChageRespwn == false)
        {
            isChageRespwn = true;
            _playerScript.ChangeReSpawnPosition(transform.position, _cameraPosition, _cameraRotation);
            GetComponent<Renderer>().material.color = Color.red;
            _updatePositionui.alpha = 1;
        }
        //少しづつ透明化
        if (isChageRespwn == true && _updatePositionui != null)
        {
            _fadeOutTime -= Time.deltaTime;
            _updatePositionui.alpha = _fadeOutTime;
            if(_updatePositionui.alpha == 0)
            {
                _updatePositionui = null;
            }
        }
    }
}

