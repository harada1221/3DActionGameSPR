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
using UnityEngine.SceneManagement;

public class SceneLoad  : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("ロードシーン名")]
    private string _loadSceneName = default;
    private const string _jump = "Jump2";
    #endregion
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if (Input.GetButton(_jump) || Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(_loadSceneName);
        }
    }
}

