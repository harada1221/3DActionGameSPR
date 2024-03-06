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

public class CalcUVScript : MonoBehaviour
{
    public Vector2 HitObj(RaycastHit hitinfo)
    {
        Vector2 uv = default;
        MeshFilter meshRenderer = hitinfo.transform.GetComponent<MeshFilter>();
        Mesh mesh = meshRenderer.sharedMesh;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            #region 1.ある点pが与えられた3点において平面上に存在するか
            //3点を持ってくる
            int indexFirst = i + 0;
            int indexSecond = i + 1;
            int indexThird = i + 2;

            //三角形の座標を持ってくる
            Vector3 triangleVerticesFirst = mesh.vertices[mesh.triangles[indexFirst]];
            Vector3 triangleVerticesSecound = mesh.vertices[mesh.triangles[indexSecond]];
            Vector3 triangleVerticesThird = mesh.vertices[mesh.triangles[indexThird]];
            Vector3 localHitPoint = hitinfo.transform.InverseTransformPoint(hitinfo.point);
            //最初の座標から次の座標の位置
            Vector3 edgeVectorFirst = triangleVerticesSecound - triangleVerticesFirst;
            Vector3 edgeVectorSecond = triangleVerticesThird - triangleVerticesFirst;
            Vector3 edgeVectorThird = localHitPoint - triangleVerticesFirst;
            //2つのベクトルの外積
            Vector3 crossProduct = Vector3.Cross(edgeVectorFirst, edgeVectorSecond);
            float val = Vector3.Dot(crossProduct, edgeVectorThird);

            //小さい少数値で誤差をカバー
            bool suc = -0.000001f < val && val < 0.000001f;

            #endregion 1.ある点pが与えられた3点において平面上に存在するか

            #region 2.同一平面上に存在する点pが三角形内部に存在するか

            if (!suc)
            {
                continue;
            }
            else
            {
                //外積の計算

                Vector3 crossProductFirst = Vector3.Cross(triangleVerticesFirst - triangleVerticesThird, localHitPoint - triangleVerticesFirst).normalized;
                Vector3 crossProductSecound = Vector3.Cross(triangleVerticesSecound - triangleVerticesFirst, localHitPoint - triangleVerticesSecound).normalized;
                Vector3 crossProductThird = Vector3.Cross(triangleVerticesThird - triangleVerticesSecound, localHitPoint - triangleVerticesThird).normalized;
                //正規化
                float dotProductAB = Vector3.Dot(crossProductFirst, crossProductSecound);
                float dotProductBC = Vector3.Dot(crossProductSecound, crossProductThird);
                //小さい少数値で誤差をカバー
                suc = 0.999f < dotProductAB && 0.999f < dotProductBC;
            }

            #endregion 2.同一平面上に存在する点pが三角形内部に存在するか

            #region 3.点pのUV座標を求める

            if (!suc)
            {
                continue;
            }
            else
            {
                //三角形のUV座標
                Vector2 uvFirst = mesh.uv[mesh.triangles[indexFirst]];
                Vector2 uvSecound = mesh.uv[mesh.triangles[indexSecond]];
                Vector2 uvThird = mesh.uv[mesh.triangles[indexThird]];

                //モデルビュープロジェクション行列
                Matrix4x4 mvp = Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix * hitinfo.transform.localToWorldMatrix;
                //各点をProjectionSpaceへの変換
                Vector4 pFirst = mvp * new Vector4(triangleVerticesFirst.x, triangleVerticesFirst.y, triangleVerticesFirst.z, 1);
                Vector4 pSecond = mvp * new Vector4(triangleVerticesSecound.x, triangleVerticesSecound.y, triangleVerticesSecound.z, 1);
                Vector4 pThird = mvp * new Vector4(triangleVerticesThird.x, triangleVerticesThird.y, triangleVerticesThird.z, 1);
                Vector4 pProject = mvp * new Vector4(localHitPoint.x, localHitPoint.y, localHitPoint.z, 1);
                //通常座標への変換(ProjectionSpace)
                Vector2 normalizedPFirst = new Vector2(pFirst.x, pFirst.y) / pFirst.w;
                Vector2 normalizedPSecond = new Vector2(pSecond.x, pSecond.y) / pSecond.w;
                Vector2 normalizedPThird = new Vector2(pThird.x, pThird.y) / pThird.w;
                //プロジェクションされたポイントを正規化した座標
                Vector2 normalizedProjectPoint = new Vector2(pProject.x, pProject.y) / pProject.w;
                //頂点のなす三角形を点pにより3分割し、必要になる面積を計算
                float triangleArea = 0.5f * ((normalizedPSecond.x - normalizedPFirst.x) * (normalizedPThird.y - normalizedPFirst.y) - (normalizedPSecond.y - normalizedPFirst.y) * (normalizedPThird.x - normalizedPFirst.x));
                float areaFirst = 0.5f * ((normalizedPThird.x - normalizedProjectPoint.x) * (normalizedPFirst.y - normalizedProjectPoint.y) - (normalizedPThird.y - normalizedProjectPoint.y) * (normalizedPFirst.x - normalizedProjectPoint.x));
                float areaSecond = 0.5f * ((normalizedPFirst.x - normalizedProjectPoint.x) * (normalizedPSecond.y - normalizedProjectPoint.y) - (normalizedPFirst.y - normalizedProjectPoint.y) * (normalizedPSecond.x - normalizedProjectPoint.x));
                //面積比からuvを補間
                float u = areaFirst / triangleArea;
                float v = areaSecond / triangleArea;
                float w = 1 / ((1 - u - v) * 1 / pFirst.w + u * 1 / pSecond.w + v * 1 / pThird.w);
                uv = w * ((1 - u - v) * uvFirst / pFirst.w + u * uvSecound / pSecond.w + v * uvThird / pThird.w);

                //uvが求まったよ!!!!
                Debug.Log(uv + ":" + hitinfo.textureCoord);
                return uv;
            }

            #endregion 3.点pのUV座標を求める
        }
        return Vector2.zero;
    }
}


