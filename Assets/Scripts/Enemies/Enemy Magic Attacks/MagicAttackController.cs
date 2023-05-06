using UnityEngine;

public class MagicAttackController : MonoBehaviour
{
    [SerializeField] private float animationDuration = 1.0f;

    private void Start()
    {
        Invoke("DestroyMagicAttack", animationDuration);
    }

    private void DestroyMagicAttack()
    {
        Destroy(gameObject);
    }
}
