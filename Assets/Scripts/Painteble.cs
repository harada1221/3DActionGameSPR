/*
*　　説明　Mesh用デカール累積テクスチャを生成・設定する
*　　原田　智大
*/
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Painteble : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("貼り付けるテクスチャ")]
    private Texture _texture = default;
    //貼り付けるために必要なMesh系
    private MeshRenderer _meshRenderer = default;
    private MeshFilter _meshFilter = default;
    //Decalを貼り付けるスクリプト
    private DecalPainter _decalPainter = default;
    //マテリアル
    private Material _material = default;
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        //マテリアルをインスタンス化
        _material = _meshRenderer.material;
        if (_material == null)
        {
            _material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        //このMesh用デカール累積テクスチャを生成・設定
        int textureSize = _material.mainTexture != null
            ? _material.mainTexture.width
            : 1024;
        _decalPainter = new DecalPainter(_meshFilter, textureSize);
        _decalPainter.BakeBaseTexture(_material.mainTexture);
        _material.mainTexture = _decalPainter.texture;

        //ペイントテクスチャを設定
        _decalPainter.SetDecalTexture(_texture);
    }

    /// <summary>
    /// デカール用の座標と傾き大きさ色を渡す
    /// </summary>
    public void Paint(
        Vector3 worldPosition,
        Vector3 normal,
        Vector3 tangent,
        float decalSize,
        Color color)
    {
        Vector3 positionOS = transform.InverseTransformPoint(worldPosition);
        Vector3 normalOS = transform.InverseTransformDirection(normal);
        Vector3 tangentOS = transform.InverseTransformDirection(tangent);
        _decalPainter.SetPointer(
             positionOS,
             normalOS,
             tangentOS,
             decalSize,
             color,
             transform.lossyScale
            );
        //更新座標にデカールの貼り付け
        _decalPainter.Paint();
    }
}

