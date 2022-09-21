using UnityEngine;

public class MagnetEffectRange : MonoBehaviour
{
    [SerializeField] GameObject tapEffect;              // �^�b�v�G�t�F�N�g

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            tapEffect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.5f);
        }
        else
        {
            tapEffect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0);
        }
    }
}
