using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField, Header("初速度")]
    private float _initialSpeed = 10f;
    [SerializeField, Header("重力加速度")]
    private float _gravity = 9.81f;
    [SerializeField, Header("スピード")]
    private float _speed = 2f;
    [SerializeField, Header("塗りの色")]
    private Color _paintColor = Color.red;
    [SerializeField, Header("塗りの大きさ")]
    private float _size = 4f;
    //投擲速度
    private Vector3 _throwVelocity = default;
    //投擲開始時刻
    private float _throwStartTime = 0f;
    //爆弾制御スクリプト
    private BombControlScript _bombControl = default;
    private void Start()
    {
        _bombControl = GameObject.FindWithTag("Player").GetComponent<BombControlScript>();
    }

    private void Update()
    {
        if (_throwVelocity != Vector3.zero)
        {
            //経過時間を計算
            float timeElapsed = Time.time - _throwStartTime;
            //放物線運動のx座標を計算
            float x = _throwVelocity.x * _initialSpeed * timeElapsed;
            //放物線運動のy座標を計算
            float y = _throwVelocity.y * _initialSpeed * timeElapsed - 0.5f * _gravity * timeElapsed * timeElapsed;
            //放物線運動のz座標を計算
            float z = _throwVelocity.z * _initialSpeed * timeElapsed;
            //物体の位置を更新
            transform.position += new Vector3(x, y, z) * _speed * Time.deltaTime;
            //物体が一定の高さより下にある場合は非表示にする
            if (transform.position.y <= -1)
            {
                HideFromStage();
            }
        }
    }
    /// <summary>
    /// 飛ばす方向を決める
    /// </summary>
    /// <param name="throwDirection"></param>
    public void SetVelocity(Vector3 throwDirection)
    {
        //投げる方向ベクトルに初速度をかけて投擲速度を計算する
        _throwVelocity = throwDirection.normalized;
        _throwStartTime = Time.time;
    }
    /// <summary>
    /// 弾を回収する
    /// </summary>
    public void HideFromStage()
    {
        // オブジェクトプールのCollect関数を呼び出し自身を回収
        _bombControl.BombCollect(this);
    }
    private void OnCollisionStay(Collision collision)
    {
        // Debug.Log(collision.transform.position);
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