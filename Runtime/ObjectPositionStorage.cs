using System.Collections.Generic;
using UnityEngine;

namespace ArgonautJH.ObjectSwitchTool.Runtime
{
    /// <summary>
    /// 현재 기록된 오브젝트 위치를 저장하는 오브젝트
    /// </summary>
    public class ObjectPositionStorage : MonoBehaviour
    {
        public List<ObjectPositionEntry> objectPositions = new();

        /// <summary>
        /// 오브젝트 위치 저장
        /// </summary>
        /// <param name="obj">대상 오브젝트</param>
        /// <param name="posA">위치 A의 좌표</param>
        /// <param name="posB">위치 B의 좌표</param>
        /// <param name="isAtA">현 위치가 A인지 여부</param>
        public void SavePosition(GameObject obj, Vector3 posA, Vector3 posB, bool isAtA)
        {
            var entry = objectPositions.Find(e => e.gameObject == obj);
            if (entry != null)
            {
                entry.positionA = posA;
                entry.positionB = posB;
                entry.isAtA = isAtA;
            }
            else
            {
                objectPositions.Add(new ObjectPositionEntry()
                {
                    gameObject = obj,
                    positionA = posA,
                    positionB = posB,
                    isAtA = isAtA
                });
            }
        }

        /// <summary>
        /// 오브젝트 위치를 가져옴
        /// </summary>
        /// <param name="obj">대상 오브젝트</param>
        /// <returns>(A좌표, B좌표. A여부)</returns>
        public (Vector3 A, Vector3 B, bool isAtA)? GetPosition(GameObject obj)
        {
            var entry = objectPositions.Find(e => e.gameObject == obj);
            if (entry != null)
            {
                return (entry.positionA, entry.positionB, entry.isAtA);
            }

            return null;
        }
    }
}