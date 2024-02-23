using UnityEngine;

namespace Common.Game
{
    public class DontDestroy : MonoBehaviour
    {
        [SerializeField] private GameObject[] _objects;

        private void Awake()
        {
            foreach (GameObject obj in _objects) 
                DontDestroyOnLoad(obj);
        }
    }
}
