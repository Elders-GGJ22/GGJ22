using System;
using UnityEngine;

namespace Assets.Scripts.Utili
{
    public class RemoveColliderOnStart : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}