using UnityEngine;

namespace Layer.View
{
    /// <summary> ターゲット(主にPlayerを追従するカメラ) </summary>
    public class TargetFollowCamera : MonoBehaviour
    {
        [SerializeField, Header("ターゲットとの距離")]
        private Vector3 _targetDistance;

        [SerializeField, Header("ターゲットオブジェクト")]
        private GameObject _targetObj;

        [SerializeField, Header("追従速度")]
        private float _followSpeed;

        [SerializeField, Header("回転速度")]
        private float _rotationSpeed;

        private void Start()
        {
            // ターゲットオブジェクトが設定されている場合、初期位置を設定
            if (_targetObj != null)
                this.transform.position = _targetObj.transform.position + _targetDistance;
        }

        private void LateUpdate()
        {
            if (_targetObj != null)
            {
                // ターゲットオブジェクトに向かって位置を補間
                transform.position = Vector3.Lerp(this.transform.position, _targetObj.transform.position - _targetDistance, Time.deltaTime * _followSpeed);

                // ターゲットオブジェクトの方向を計算
                Vector3 direction = _targetObj.transform.position - transform.position;

                // ターゲットオブジェクトの方向を計算
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 回転を補間
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary> ターゲットオブジェクトを設定 </summary>
        public void SetTargetObj(GameObject playerObj)
        {
            _targetObj = playerObj;
            this.transform.position = _targetObj.transform.position + _targetDistance;
        }
    }
}