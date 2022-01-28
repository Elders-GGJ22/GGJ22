using UnityEngine;

namespace Assets.Scripts.Utili
{
    public class RemoveOnStart : MonoBehaviour
    {
        public void Start()
        {
            Destroy(this.gameObject);
        }
    }
}