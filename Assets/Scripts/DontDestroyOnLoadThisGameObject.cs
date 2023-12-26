using UnityEngine;

public class DontDestroyOnLoadThisGameObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
