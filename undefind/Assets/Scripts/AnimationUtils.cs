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
    //        Debug.LogWarning("Animator �� ������. ���������� ��������.");
    //        return;
    //    }

    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

    //    // ��������, ��� �������� �������
    //    if (!stateInfo.IsName(stateInfo.shortNameHash.ToString()))
    //    {
    //        Debug.LogWarning("������� �������� ���������� ��� ��������.");
    //        return;
    //    }

    //    float elapsedTime = 0f;

    //    // �������� ���������� ��������
    //    while (stateInfo.normalizedTime < 1f || stateInfo.loop)
    //    {
    //        if (elapsedTime > timeout)
    //        {
    //            Debug.LogWarning("�������� ���������� �������� �������� ��-�� ����-����.");
    //            break;
    //        }


    //        await Task.Yield(16);
    //        elapsedTime += 0.016f;
    //        stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
    //    }
    //}
}
