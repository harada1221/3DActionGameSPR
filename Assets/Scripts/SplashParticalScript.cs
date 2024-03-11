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

public class SplashParticalScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField]
    private ParticleSystem _particleSystem = default;
    private SplashControlScript _controlScript = default;
    #endregion
    private void Start()
    {
        _controlScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SplashControlScript>();
    }
    private void Update()
    {
        if (_particleSystem.isStopped)
        {
            EndSplash();
        }
    }
    public void StrartSplash(Vector3 position, Vector3 rotation)
    {
        Debug.Log("派生");
        this.transform.position = position;
        this.transform.rotation = Quaternion.LookRotation(rotation);
        _particleSystem.Play();
    }
    /// <summary>
    /// 回収
    /// </summary>
    private void EndSplash()
    {
        _controlScript.SplashCollect(this);
    }
}

