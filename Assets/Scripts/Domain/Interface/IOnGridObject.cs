using UnityEngine;

namespace Domain
{
    /// <summary> Tileの上に置かれるオブジェクトのインターフェース </summary>
    public interface IOnGridObject 
    {
        /// <summary> 
        /// このオブジェクトが占有しているGridのサイズ
        /// GridSize = 2 ならば、2x2のGridを占有していることになる
        /// </summary>
        public int GridSize { get; }

        /// <summary> 
        /// このオブジェクトのGrid上の最も座標が低い位置 
        /// x = 4, z = 4 の場合、もしこのObjectのGridSizeが2であれば、
        /// 占有しているGridは(4, 0, 4), (5, 0, 4), (4, 0, 5), (5, 0, 5)の4つとなり、
        /// Data上でのこのオブジェクトの座標は(4, 0, 4)となる
        /// </summary>
        public Vector3 GridPosition { get; }

        /// <summary> Grid上の座標を設定する </summary>
        /// <param name="position"> 設定する座標 </param>
        public void SetPosition(Vector3 position);
    }
}
