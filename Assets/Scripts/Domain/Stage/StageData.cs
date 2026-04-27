using System;
using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    /// <summary> ステージのデータ </summary>
    public class StageData
    {
        /// <summary> ステージを形成する各セクションデータ </summary>
        [Header("ステージを形成する各セクションデータ")]
        [SerializeField] SectionData[,] _sectionDataArray;

        /// <summary> ステージ内部のセクションデータをまとめた配列 </summary>
        public SectionData[,] StageSectionsData => _sectionDataArray;

        /// <summary> ステージデータの初期化 </summary>
        public void InitStageData(int horizontalSectionRange, int verticalSectionRange)
        {
            _sectionDataArray = new SectionData[horizontalSectionRange, verticalSectionRange];
        }

        /// <summary> SectionDataの保存関数 </summary>
        public void SetSectionData(SectionData sectionData, int arrIndexX, int arrIndexY)
        {
            _sectionDataArray[arrIndexX, arrIndexY] = sectionData;
        }
    }

    /// <summary> ダンジョンを形成するセクションのデータ </summary>
    public class SectionData
    {
        /// <summary> Section内部の部屋の有無 </summary>
        private bool _isBuildRoom = false;

        /// <summary> このセクションがほかのセクションとつながっているかの判定 </summary>
        private bool _isConnect = false;

        /// <summary> Section内部のGridのデータ </summary>
        private GridData[] _gridDataArr;

        /// <summary> Sectionのどの方向に道が伸びているかの判定の保存先 </summary>
        private Dictionary<ConnectDirection, bool> _isConnectDirectionDict = new();

        /// <summary> Section内部のJointの保存先 </summary>
        private Dictionary<JointType, List<Joint>> _jointDataDict = new();

        /// <summary> GridDataの1列の長さ </summary>
        private int _gridWidth;

        public bool IsBuildRoom
        {
            get { return _isBuildRoom; }
            set { _isBuildRoom = value; }
        }

        public bool IsConnect
        {
            get { return _isConnect; }
            set { _isConnect = value; }
        }

        public Dictionary<ConnectDirection, bool> IsConnectDirectionDict => _isConnectDirectionDict;

        public Dictionary<JointType, List<Joint>> JointDataDict => _jointDataDict;

        public GridData[] GridDataArr => _gridDataArr;

        public int GridWidth => _gridWidth;

        /// <summary> SctionDataの初期化 </summary>
        public void InitSectionData(int arrIndex)
        {
            int totalIndexNum = arrIndex * arrIndex;
            _gridWidth = arrIndex;
            _gridDataArr = new GridData[totalIndexNum];

            for (int i = 0; i < Enum.GetValues(typeof(ConnectDirection)).Length; i++)
            {
                _isConnectDirectionDict.Add((ConnectDirection)i, false);
            }
        }

        /// <summary> GridDataの参照用関数 </summary>
        public GridData GetGridData(int x, int y)
        {
            int index = x + (_gridWidth * y);
            return _gridDataArr[index];
        }

        /// <summary> 生成したJointの保存 </summary>
        public void AddJoint(JointType jointType, Joint joint)
        {
            if (!_jointDataDict.ContainsKey(jointType))
                _jointDataDict.Add(jointType, new List<Joint>());

            _jointDataDict[jointType].Add(joint);
        }

        /// <summary> Sectionの道が伸びている方向の保存 </summary>
        public void AddConnectDirection(ConnectDirection connectDirection)
        {
            _isConnectDirectionDict[connectDirection] = true;
        }
    }

    /// <summary> Section内部のGridのデータ </summary>
    public struct GridData
    {
        //このGridがJointかどうかの判定
        private bool _isJoint;

        //このGridのタイルの種類
        private TileType _tileType;

        public bool IsJoint => _isJoint;

        public TileType TileType => _tileType;

        public void SetGridData(TileType tileType) => _tileType = tileType;
    }

    /// <summary> Section内部のJointのデータ </summary>
    public class Joint
    {
        private Vector2Int _jointPos;

        public Vector2Int JointPos
        {
            get { return _jointPos; }
            set { _jointPos = value; }
        }
    }

    /// <summary> Jointの種類 </summary>
    public enum JointType
    {
        Normal,
        TurningPoint,
        SectionExit
    }

    /// <summary> Sectionのどの方向に道が伸びているかの判定 </summary>
    public enum ConnectDirection
    {
        Top,
        Left,
        Right,
        Bottom
    }
}




