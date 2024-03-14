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

public class CameraScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("プレイヤー")]
    private GameObject _player = default;
    [SerializeField, Header("移動スピード")]
    private float _speed = default;
    //プレイヤーのスクリプト
    private PlayerScript _playerScript = default;
    //入力の名前
    private const string _horizontal = "Horizontal2";
    private const string _vertical = "Vertical2";
    #endregion
    private void Start()
    {
        _playerScript = _player.GetComponent<PlayerScript>();
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //スティックのX,Y軸がどれほど移動したか
        float X_Rotation = Input.GetAxisRaw(_horizontal);
        float Y_Rotation = Input.GetAxisRaw(_vertical);
        //X方向に一定量移動していれば横回転
        if (Mathf.Abs(X_Rotation) > 0.001f&&_playerScript.GetShoot == false)
        {
            //回転軸はワールド座標のY軸
            transform.RotateAround(_player.transform.position, Vector3.up, X_Rotation * Time.deltaTime * _speed);
        }
        //射撃中の移動
        else if(_playerScript.GetShoot == true)
        {
            //回転軸はワールド座標のY軸
            transform.RotateAround(_player.transform.position, Vector3.up, X_Rotation * Time.deltaTime * _speed);
            _player.transform.RotateAround(_player.transform.position, Vector3.up, X_Rotation * Time.deltaTime * _speed);
        }
        //Y方向に一定量移動していれば縦回転
        if (Mathf.Abs(Y_Rotation) > 0.001f)
        {
            //回転軸はカメラ自身のX軸
            transform.RotateAround(_player.transform.position, transform.right, Y_Rotation * Time.deltaTime * _speed);
        }
    }
   
}


