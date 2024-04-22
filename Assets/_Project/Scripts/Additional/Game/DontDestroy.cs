using UnityEngine;

namespace UnrealTeam.SB.Additional.Game
{
    public class DontDestroy : MonoBehaviour
    {
        [SerializeField] private GameObject[] _objects;

        private void Awake()
        {
            foreach (var obj in _objects) 
                DontDestroyOnLoad(obj);
        }
    }
}
