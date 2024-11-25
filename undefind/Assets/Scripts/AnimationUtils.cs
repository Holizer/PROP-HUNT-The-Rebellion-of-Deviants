using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public static class AnimationUtils
{
    //public static async void WaitForAnimationToEndAsync(Animator animator, int layerIndex = 0, float timeout = 5f)
    //{
    //    if (animator == null)
    //    {
    //        Debug.LogWarning("Animator не найден. Завершение ожидания.");
    //        return;
    //    }

    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

    //    // Убедимся, что анимация активна
    //    if (!stateInfo.IsName(stateInfo.shortNameHash.ToString()))
    //    {
    //        Debug.LogWarning("Текущая анимация недоступна для ожидания.");
    //        return;
    //    }

    //    float elapsedTime = 0f;

    //    // Ожидание завершения анимации
    //    while (stateInfo.normalizedTime < 1f || stateInfo.loop)
    //    {
    //        if (elapsedTime > timeout)
    //        {
    //            Debug.LogWarning("Ожидание завершения анимации прервано из-за тайм-аута.");
    //            break;
    //        }


    //        await Task.Yield(16);
    //        elapsedTime += 0.016f;
    //        stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
    //    }
    //}
}
