using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T _instant = null;
    public static T Instant => _instant;

    private void Awake()
    {
        if (_instant == null)
        {
            _instant = GetComponent<T>();
            return;
        }

        if (_instant.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }
}
