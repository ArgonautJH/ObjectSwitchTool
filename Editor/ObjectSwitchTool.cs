using System.Collections.Generic;
using ArgonautJH.ObjectSwitchTool.Runtime;
using UnityEditor;
using UnityEngine;

namespace ArgonautJH.ObjectSwitchTool.Editor
{
    public class ObjectSwitchTool : EditorWindow
    {
        // 편집기에서 관리할 오브젝트 리스트
        private readonly List<GameObject> objects = new();

        // 각 오브젝트의 A/B 위치 저장
        private readonly Dictionary<GameObject, (Vector3 A, Vector3 B)> positions = new();

        // 현재 위치가 A인지 B인지 저장
        private readonly Dictionary<GameObject, bool> positionSwitch = new();

        private bool foldout = true; // 오브젝트 리스트 펼치기/접기 상태 저장
        private ObjectPositionStorage positionStorage; // 위치 데이터를 저장할 오브젝트
        private GameObject saveTargetObject; // 위치 데이터를 저장할 게임 오브젝트
        private Vector2 scrollPosition; // 스크롤 위치 저장

        // 0. Unity 메뉴에서 "Tools/Object Switch Tool"을 선택하면 창을 열도록 설정
        [MenuItem("Tools/Object Switch Tool")]
        public static void ShowWindow()
        {
            GetWindow<ObjectSwitchTool>("Object Switch Tool");
        }

        private void OnEnable()
        {
            LoadStorage();
            LoadPositionData();
        }

        private void OnDisable() => SavePositionData();

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(0));
            DrawSaveObjectSettings();
            DrawObjectManagement();
            DrawActionsAllObject();
            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 위치 저장 대상 오브젝트를 선택하고 데이터를 저장/불러오는 UI
        /// </summary>
        private void DrawSaveObjectSettings()
        {
            GUILayout.Label("게임 세이브 오브젝트 설정", EditorStyles.boldLabel);
            saveTargetObject =
                (GameObject)EditorGUILayout.ObjectField("세이브 오브젝트", saveTargetObject, typeof(GameObject), true);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("세이브")) SavePositionData();
            if (GUILayout.Button("로드")) LoadPositionData();
            if (GUILayout.Button("세이브 오브젝트 설정")) SetStorageTarget();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 오브젝트 리스트 및 개별 오브젝트 관리 UI
        /// </summary>
        private void DrawObjectManagement()
        {
            GUILayout.BeginHorizontal(); // 가로 정렬 시작

            GUILayout.Label("오브젝트 관리", EditorStyles.boldLabel); // 레이블 표시 (글씨 Bold)
            if (GUILayout.Button("+ 오브젝트 추가")) objects.Add(null); // 버튼 표시 (초기 값 null인 오브젝트 추가)

            GUILayout.EndHorizontal(); // 가로 정렬 끝

            GUILayout.Space(10); // 10px 간격 추가
            foldout = EditorGUILayout.Foldout(foldout, $"오브젝트 리스트 ({objects.Count})"); // 접기/펼치기 버튼 표시
            if (!foldout) return;

            List<GameObject> toRemove = new(); // objects 리스트에서 삭제할 GameObject를 임시로 저장할 리스트

            // objects.Count만큼 반복하여 오브젝트 리스트 표시
            for (int i = 0; i < objects.Count; i++)
            {
                GUILayout.BeginVertical("box"); // 세로 정렬 시작 (박스 스타일)

                GUILayout.BeginHorizontal(); // 가로 정렬 시작
                objects[i] = (GameObject)EditorGUILayout.ObjectField( // 오브젝트 필드 표시
                    $"Element {i}" // 레이블 표시
                    , objects[i] // 표시할 오브젝트
                    , typeof(GameObject) // 오브젝트 타입
                    , true // Scene 뷰에 있는 오브젝트를 표시할지 여부
                );
                if (GUILayout.Button("삭제")) toRemove.Add(objects[i]); // 삭제 버튼 표시
                GUILayout.EndHorizontal(); // 가로 정렬 끝

                if (objects[i] != null) HandleObjectEntry(objects[i]);
                GUILayout.EndVertical(); // 세로 정렬 끝
            }

            // 삭제할 오브젝트를 objects 리스트에서 제거
            toRemove.ForEach(obj =>
            {
                objects.Remove(obj);
                positions.Remove(obj);
                positionSwitch.Remove(obj);
            });
        }

        /// <summary>
        /// 개별 오브젝트에 대한 UI 및 기능 처리
        /// </summary>
        private void HandleObjectEntry(GameObject obj)
        {
            if (!positions.ContainsKey(obj)) InitializeObjectPosition(obj); // 오브젝트 위치 초기화
            DisplayPositionInfo(obj); // 오브젝트 위치 정보 표시
            DrawPositionControls(obj); // 오브젝트 위치 제어 버튼 표시
        }

        /// <summary>
        /// 오브젝트의 위치를 초기화하고 저장된 위치를 불러옴
        /// </summary>
        private void InitializeObjectPosition(GameObject obj)
        {
            var savedPos = positionStorage.GetPosition(obj); // 오브젝트의 위치 데이터 불러오기
            positions[obj] = savedPos.HasValue
                ? (savedPos.Value.A, savedPos.Value.B)
                : (obj.transform.position, obj.transform.position);
            positionSwitch[obj] = savedPos?.isAtA ?? true;
        }

        /// <summary>
        /// 현재 오브젝트의 위치 정보를 표시
        /// </summary>
        private void DisplayPositionInfo(GameObject obj)
        {
            string positionState
                = positions.ContainsKey(obj) ? ($"A: {positions[obj].A}, B: {positions[obj].B}") : "위치 미저장";
            EditorGUILayout.LabelField("저장된 위치:", positionState);
            EditorGUILayout.LabelField("현재 위치:", positionSwitch[obj] ? "A 위치" : "B 위치");
        }

        /// <summary>
        /// 오브젝트의 위치를 저장, 이동, 토글하는 UI 버튼 추가
        /// </summary>
        private void DrawPositionControls(GameObject obj)
        {
            GUILayout.BeginHorizontal(); // 가로 정렬 시작
            if (GUILayout.Button("A 위치 저장")) SavePosition(obj, true);
            if (GUILayout.Button("B 위치 저장")) SavePosition(obj, false);
            GUILayout.EndHorizontal(); // 가로 정렬 끝

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("A 위치 이동")) MoveObject(obj, true);
            if (GUILayout.Button("B 위치 이동")) MoveObject(obj, false);
            if (GUILayout.Button("위치 토글")) ToggleObjectPosition(obj);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 현재 오브젝트의 위치 정보를 표시
        /// </summary>
        private void DrawActionsAllObject()
        {
            GUILayout.Space(10);
            GUILayout.Label("전체 오브젝트 위치 변경", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("A 위치로 이동")) MoveAllObjects(true);
            if (GUILayout.Button("B 위치로 이동")) MoveAllObjects(false);
            if (GUILayout.Button("위치 토글")) ToggleAllObjectsPosition();
            GUILayout.EndHorizontal();
        }

        private void MoveAllObjects(bool toA)
        {
            foreach (var obj in objects) MoveObject(obj, toA);
        }

        private void ToggleAllObjectsPosition()
        {
            foreach (var obj in objects) ToggleObjectPosition(obj);
        }

        /// <summary>
        /// 오브젝트의 위치를 저장
        /// </summary>
        /// <param name="obj">오브젝트</param>
        /// <param name="isA">현재 위치가 A인지 여부</param>
        private void SavePosition(GameObject obj, bool isA)
        {
            if (obj != null)
                positions[obj] =
                    isA ? (obj.transform.position, positions[obj].B) : (positions[obj].A, obj.transform.position);
        }

        /// <summary>
        /// 오브젝트를 이동
        /// </summary>
        /// <param name="obj">대상 오브젝트</param>
        /// <param name="toA">A로 이동시킬지 여부</param>
        private void MoveObject(GameObject obj, bool toA)
        {
            if (obj != null && positions.ContainsKey(obj))
            {
                obj.transform.position = toA ? positions[obj].A : positions[obj].B;
                positionSwitch[obj] = toA;
            }
        }

        private void ToggleObjectPosition(GameObject obj)
        {
            if (obj != null && positions.ContainsKey(obj)) MoveObject(obj, !positionSwitch[obj]);
        }

        private void SetStorageTarget()
        {
            saveTargetObject ??= new GameObject("ObjectPositionStorage") { transform = { position = Vector3.zero } };
            positionStorage = saveTargetObject.GetComponent<ObjectPositionStorage>() ??
                              saveTargetObject.AddComponent<ObjectPositionStorage>();
            Undo.RegisterCreatedObjectUndo(saveTargetObject, "Create ObjectPositionStorage");
        }

        private void LoadStorage()
        {
            saveTargetObject = GameObject.Find("ObjectPositionStorage") ?? new GameObject("ObjectPositionStorage")
                { transform = { position = Vector3.zero } };
            positionStorage = saveTargetObject.GetComponent<ObjectPositionStorage>() ??
                              saveTargetObject.AddComponent<ObjectPositionStorage>();
        }

        /// <summary>
        /// 위치 데이터를 저장
        /// </summary>
        private void SavePositionData()
        {
            foreach (var obj in objects)
                positionStorage.SavePosition(obj, positions[obj].A, positions[obj].B, positionSwitch[obj]);
            EditorUtility.SetDirty(positionStorage);
        }

        /// <summary>
        /// 저장된 위치 데이터를 불러옴
        /// </summary>
        private void LoadPositionData()
        {
            objects.Clear();
            foreach (var entry in positionStorage.objectPositions)
            {
                if (entry.gameObject == null) continue;
                objects.Add(entry.gameObject);
                positions[entry.gameObject] = (entry.positionA, entry.positionB);
                positionSwitch[entry.gameObject] = entry.isAtA;
            }
        }
    }
}

