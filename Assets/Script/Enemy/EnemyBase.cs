using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [BoxGroup("Required Component"), SerializeField, Required]
    private DOTweenAnimation myAnim;



    [ButtonGroup("메서드 디버깅")]
    public void TakeDamage(float damage)
    {
        PlayHurtAnim();
    }

    [ButtonGroup("메서드 디버깅")]
    public void PlayHurtAnim()
    {
        myAnim.DORestart();
    }
}
