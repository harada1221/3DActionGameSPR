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
    }
    private void Update()
    {
        //一定範囲内にいるかかつ1回目か
        if (Vector3.Distance(transform.position, _player.transform.position) < 1 && isChageRespwn == false)
        {
            isChageRespwn = true;
            _playerScript.ChangeReSpawnPosition(transform.position, _cameraPosition,_cameraRotation);
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}

