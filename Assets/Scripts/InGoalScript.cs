/*
*　　説明　プレイヤーがゴールしたか
*　　原田　智大
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGoalScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("ロードシーン名")]
    private string _loadSceneName = default;
    //プレイヤー
    private GameObject _player = default;
    #endregion
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //プレイヤー取得
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //一定範囲内にいるか
        if (Vector3.Distance(transform.position, _player.transform.position) < 1)
        {
            SceneManager.LoadScene(_loadSceneName);
        }
    }
}

