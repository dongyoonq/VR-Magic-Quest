using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLocomotion : MonoBehaviour
{
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
}
