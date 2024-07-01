using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Test : MonoBehaviour
{
    public float rotationDuration = 5f; // Th?i gian xoay
    public Vector3 targetRotation = new Vector3(0, 90, 0); // G�c xoay mong mu?n

    void Start()
    {
        // Xoay ??i t??ng quanh tr?c Y trong kh�ng gian world
        transform.DORotate(targetRotation, rotationDuration, RotateMode.WorldAxisAdd);
    }
}
