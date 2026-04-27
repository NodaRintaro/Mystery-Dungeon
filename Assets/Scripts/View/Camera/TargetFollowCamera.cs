using Application;
using Domain;
using UnityEngine;

namespace View
{
    /// <summary> プレイヤーを追従するMainCamera </summary>
    public class TargetFollowCamera : MonoBehaviour
    {
        [SerializeField, Header("カメラとプレイヤーの距離")]
        private Vector3 _targetDistance;

        [SerializeField, Header("追従するプレイヤーオブジェクト")]
        private GameObject _targetObj;

        [SerializeField, Header("追従するスピード")]
        private float _followSpeed;

        [SerializeField, Header("回転するスピード")]
        private float _rotationSpeed;

        private void Start()
        {
            //プレイヤーオブジェクトが存在する場合、カメラの位置をプレイヤーの位置から指定距離分だけ離す
            if (_targetObj != null)
                this.transform.position = _targetObj.transform.position + _targetDistance;
        }

        private void LateUpdate()
        {
            if (_targetObj != null)
            {
                //線形補間関数によるカメラの移動
                transform.position = Vector3.Lerp(this.transform.position, _targetObj.transform.position - _targetDistance, Time.deltaTime * _followSpeed);

                // ターゲットへの方向ベクトルを計算
                Vector3 direction = _targetObj.transform.position - transform.position;

                // その方向を向くための回転値を取得
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 現在の回転から目標の回転まで滑らかに補間
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary> プレイヤーオブジェクトを設定 </summary>
        public void SetTargetObj(GameObject playerObj)
        {
            _targetObj = playerObj;
            this.transform.position = _targetObj.transform.position + _targetDistance;
        }
    }
}






