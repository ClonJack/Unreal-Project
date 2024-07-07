using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnrealTeam.SB.Editor.StoneWindow
{
    public class StoneEditorWindow : EditorWindow
    {
        private string _prefabName = "DestroyedStone";
        private string _folderPath = "Assets/_Project/Bundles/Prefabs/Environment/Stone/DestroyedVariants/";
        private OverwriteMode _overwriteMode = OverwriteMode.Increment;

        private GameObject _rootTemplate;
        private GameObject _pieceTemplate;
        private PiecesCreationMode _piecesCreationMode = PiecesCreationMode.SingleSourceObject;
        private int _piecesCount;
        private GameObject[] _sourceObjects;
        private GameObject _singleSourceObject;

        private bool _randomizePosition = true;
        private bool _randomizeRotation = true;
        private bool _randomizeScale;
        private Vector3 _minPosition = Vector3.zero;
        private Vector3 _maxPosition = Vector3.zero;
        private Vector3 _minRotation = Vector3.zero;
        private Vector3 _maxRotation = Vector3.zero;
        private Vector3 _minScale = Vector3.one; 
        private Vector3 _maxScale = Vector3.one;


        [MenuItem("Unreal Editor/Stone Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<StoneEditorWindow>("Stone Editor");
            window._pieceTemplate ??=
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/_Project/Bundles/Prefabs/Environment/Stone/Common/PieceTemplate.prefab");
            window._rootTemplate ??=
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/_Project/Bundles/Prefabs/Environment/Stone/Common/RootTemplate.prefab");
        }

        private void OnGUI()
        {
            ShowPersistentFields();
            ShowPiecesModeFields();
            ShowRandomFields();
            ShowCreateButton();
        }

        private void CreateStone()
        {
            if (_pieceTemplate == null)
            {
                Debug.LogError("Template Object is not assigned.");
                return;
            }

            if (!TryGetPrefabPath(out var prefabPath))
                return;

            var createdPrefab = (GameObject)PrefabUtility.InstantiatePrefab(_rootTemplate);

            for (var i = 0; i < _piecesCount; i++)
            {
                var newPiece = (GameObject)PrefabUtility.InstantiatePrefab(_pieceTemplate, createdPrefab.transform);
                newPiece.name = $"Piece {i + 1}";
                CopyComponentsIfNeeded(i, newPiece);
                RandomizeTransformValuesIfNeeded(newPiece);
            }

            PrefabUtility.SaveAsPrefabAsset(createdPrefab, prefabPath);
            DestroyImmediate(createdPrefab);
        }

        private void ShowPersistentFields()
        {
            GUILayout.Label("Path", EditorStyles.boldLabel);
            _prefabName = EditorGUILayout.TextField("Prefab Name", _prefabName);
            _folderPath = EditorGUILayout.TextField("Folder Path", _folderPath);
            _overwriteMode = (OverwriteMode)EditorGUILayout.EnumPopup("Overwrite Mode", _overwriteMode);

            GUILayout.Space(5);
            GUILayout.Label("Components Templates", EditorStyles.boldLabel);
            _pieceTemplate =
                (GameObject)EditorGUILayout.ObjectField("Piece Template", _pieceTemplate, typeof(GameObject), false);
            _rootTemplate =
                (GameObject)EditorGUILayout.ObjectField("Root Template", _rootTemplate, typeof(GameObject), false);

            GUILayout.Space(5);
            GUILayout.Label("Pieces", EditorStyles.boldLabel);
            _piecesCreationMode =
                (PiecesCreationMode)EditorGUILayout.EnumPopup("Pieces Creation Mode", _piecesCreationMode);
            _piecesCount = Mathf.Max(0, EditorGUILayout.IntField("Number of Pieces", _piecesCount));
        }

        private void ShowPiecesModeFields()
        {
            switch (_piecesCreationMode)
            {
                case PiecesCreationMode.SourceObjects:
                {
                    if (_sourceObjects == null || _sourceObjects.Length != _piecesCount)
                        _sourceObjects = new GameObject[_piecesCount];

                    GUILayout.Space(5);
                    GUILayout.Label("Source Objects", EditorStyles.label);
                    for (var i = 0; i < _piecesCount; i++)
                        _sourceObjects[i] = (GameObject)EditorGUILayout.ObjectField($"Source Object {i + 1}",
                            _sourceObjects[i], typeof(GameObject), false);

                    break;
                }
                case PiecesCreationMode.SingleSourceObject:
                    _singleSourceObject = (GameObject)EditorGUILayout.ObjectField("Single Source Object",
                        _singleSourceObject, typeof(GameObject), false);
                    break;
                case PiecesCreationMode.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowRandomFields()
        {
            GUILayout.Space(10);
            _randomizePosition = EditorGUILayout.Toggle("Randomize Position", _randomizePosition);
            if (_randomizePosition)
            {
                _minPosition = EditorGUILayout.Vector3Field("Min Position", _minPosition);
                _maxPosition = EditorGUILayout.Vector3Field("Max Position", _maxPosition);
            }

            GUILayout.Space(10);
            _randomizeRotation = EditorGUILayout.Toggle("Randomize Rotation", _randomizeRotation);
            if (_randomizeRotation)
            {
                _minRotation = EditorGUILayout.Vector3Field("Min Rotation", _minRotation);
                _maxRotation = EditorGUILayout.Vector3Field("Max Rotation", _maxRotation);
            }

            GUILayout.Space(10);
            _randomizeScale = EditorGUILayout.Toggle("Randomize Scale", _randomizeScale);
            if (_randomizeScale)
            {
                _minScale = EditorGUILayout.Vector3Field("Min Scale", _minScale);
                _maxScale = EditorGUILayout.Vector3Field("Max Scale", _maxScale);
            }
        }

        private void ShowCreateButton()
        {
            if (GUILayout.Button("Create Prefab"))
                CreateStone();
        }

        private void CopyComponentsIfNeeded(int i, GameObject newPiece)
        {
            switch (_piecesCreationMode)
            {
                case PiecesCreationMode.SourceObjects when _sourceObjects[i] != null:
                    CopyRequiredComponents(_sourceObjects[i], newPiece);
                    break;
                case PiecesCreationMode.SingleSourceObject when _singleSourceObject != null:
                    CopyRequiredComponents(_singleSourceObject, newPiece);
                    break;
            }
        }

        private bool TryGetPrefabPath(out string path)
        {
            path = _folderPath + _prefabName + ".prefab";

            if (!AssetDatabase.IsValidFolder(_folderPath))
            {
                Debug.LogError("Specified folder path does not exist.");
                return false;
            }

            switch (_overwriteMode)
            {
                case OverwriteMode.Warning when System.IO.File.Exists(path):
                {
                    Debug.LogError(
                        "Prefab already exists. Enable 'Overwrite' mode to replace it or 'Increment' mode to create a new version.");
                    return false;
                }
                case OverwriteMode.Increment:
                {
                    var num = 1;
                    var newPath = path;
                    while (System.IO.File.Exists(newPath))
                    {
                        newPath = _folderPath + _prefabName + num + ".prefab";
                        num++;
                    }

                    path = newPath;
                    return true;
                }
            }

            return true;
        }

        private static void CopyRequiredComponents(GameObject source, GameObject target)
        {
            foreach (var component in source.GetComponents<Component>())
            {
                var componentType = component.GetType();
                if (!target.TryGetComponent(componentType, out _))
                    target.AddComponent(componentType);

                ComponentUtility.CopyComponent(component);
                ComponentUtility.PasteComponentValues(target.GetComponent(componentType));
            }
        }

        private void RandomizeTransformValuesIfNeeded(GameObject pieceObject)
        {
            var pieceTransform = pieceObject.transform;
            if (_randomizePosition)
            {
                pieceTransform.localPosition = new Vector3(
                    Random.Range(_minPosition.x, _maxPosition.x),
                    Random.Range(_minPosition.y, _maxPosition.y),
                    Random.Range(_minPosition.z, _maxPosition.z));
            }

            if (_randomizeRotation)
            {
                pieceTransform.localRotation = Quaternion.Euler(
                    Random.Range(_minRotation.x, _maxRotation.x),
                    Random.Range(_minRotation.y, _maxRotation.y),
                    Random.Range(_minRotation.z, _maxRotation.z));
            }

            if (_randomizeScale)
            {
                pieceTransform.localScale = new Vector3(
                    Random.Range(_minScale.x, _maxScale.x),
                    Random.Range(_minScale.y, _maxScale.y),
                    Random.Range(_minScale.z, _maxScale.z));
            }
        }
    }
}