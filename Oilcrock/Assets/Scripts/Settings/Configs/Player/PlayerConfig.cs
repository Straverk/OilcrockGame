using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Setting/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Move Characteristics")]
        [SerializeField, Range(0, 50)] public float MoveSpeed = 10;
        [Space]
        [SerializeField, Range(0, 4)] public float SprintMoveModifier = 2f;
        [SerializeField, Range(0, 4)] public float CrouchMoveModifier = 0.5f;
        [Space]
        [SerializeField, Range(-10, 0)] public float SurfacePull = -1f;
        [SerializeField, Range(-10, 0)] public float GravityForce = -0.981f;
        [SerializeField, Range(0, 100)] public float JumpStrength = 10f;

        [Header("Interaction Characteristics")]
        [SerializeField, Range(0, 5)] public float InteractDistance = 2f;
        [SerializeField, Range(0, 5)] public float TakeTime = 0.2f;
        [SerializeField, Range(0, 500)] public float DropItemForce = 50f;


        [Header("Camera Characteristics")]
        [SerializeField, Range(-90, -45)] public float MaxAngleUp = -90;
        [SerializeField, Range(45, 90)] public float MinAngleDown = 70;
    }
}