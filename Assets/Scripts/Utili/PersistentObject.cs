using UnityEngine;

namespace Assets.Scripts.Utili
{
    public class PersistentObject : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}