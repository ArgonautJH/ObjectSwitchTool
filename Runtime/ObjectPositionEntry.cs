using UnityEngine;

namespace ArgonautJH.ObjectSwitchTool.Runtime
{
    /// <summary>
    /// 오브젝트 위치 저장 엔트리
    /// </summary>
    [System.Serializable]
    public class ObjectPositionEntry
    {
        public GameObject gameObject;
        public Vector3 positionA;
        public Vector3 positionB;
        public bool isAtA;
    }
}