/*
*　　説明　
*　　日付
*　　原田　智大
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("スピード")]
    private float _speed = default;
    [SerializeField, Header("落下スピード")]
    private float _foolSpeed = default;
    [SerializeField, Header("塗りの色")]
    private Color _paintColor = Color.red;
    [SerializeField, Header("塗りの大きさ")]
    private float _size = 1f;
    //射撃の向き
    private Vector3 _shootVelocity = default;
    //射撃位置
    private Vector3 _nowShotPosition = default;
    //銃のスクリプト
    private GunScript _gunScript = default;
    //射程の最高地点に到達したか
    private bool isAngle = default;
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //銃のスクリプト
        _gunScript = GameObject.FindWithTag("Player").GetComponent<GunScript>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //射程範囲内か
        if (_gunScript.GetPower >= Vector3.Distance(transform.position, _nowShotPosition) && isAngle == false)
        {
            //毎フレーム弾を移動させる
            transform.position += _shootVelocity * _speed * Time.deltaTime;
        }
        //射程距離範囲外
        else
        {
            //弾を落下
            FoolMove();
        }
        //0以下だと回収
        if (transform.position.y < -1)
        {
            HideFromStage();
        }
    }
    /// <summary>
    /// 弾の方向を設定
    /// </summary>
    /// <param name="shotDirections">飛ばす方向</param>
    /// <param name="shotPosition">発射位置</param>
    public void SetVelocity(Vector3 shotDirections, Vector3 shotPosition)
    {
        //射程範囲内の初期化
        isAngle = false;
        //向き設定
        _shootVelocity = shotDirections.normalized;
        //発射位置
        _nowShotPosition = shotPosition;
    }
    /// <summary>
    /// 弾を回収する
    /// </summary>
    private void HideFromStage()
    {
        //オブジェクトプールのCollect関数を呼び出し自身を回収
        _gunScript.BallCollect(this);
    }
    /// <summary>
    /// 落下させる
    /// </summary>
    private void FoolMove()
    {
        isAngle = true;
        //下に落とす
        _shootVelocity = Vector3.down;
        transform.position += _shootVelocity * Time.deltaTime * _foolSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            //当たった場所を取得
            Painteble paintable = collision.gameObject.GetComponent<Painteble>();
            ContactPoint contact = collision.GetContact(0);
            Vector3 normal = contact.normal;
            Vector3 hitPosition = contact.point;
            Vector3 tangent = Vector3.Cross(normal, Vector3.right).normalized;
            if (tangent.sqrMagnitude < 0.01f)
            {
                tangent = Vector3.Cross(normal, Vector3.forward).normalized;
            }
            //テクスチャを更新
            paintable.Paint
                (
                hitPosition,
                normal,
                tangent,
                _size,
                 _paintColor
                );
            //弾回収
            HideFromStage();
        }

    }
}

