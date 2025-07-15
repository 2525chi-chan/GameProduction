using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class MovePlayer : MonoBehaviour
{
    [Header("動かすオブジェクト")]
    [SerializeField] Transform target;
    [Header("移動スピードの倍率")]
    [SerializeField] float speed = 1f;
    [Header("回転スピードの倍率")]
    [SerializeField] float rotationSpeed = 1f;
    [Header("プレイヤーの質量")]
    [SerializeField] float mass = 1f;

    [Header("必要なコンポーネント")]
    [SerializeField] PlayerStatus status;
    [SerializeField] CameraDirectionCalculator cameraDirectionCalculator;
    [SerializeField] Dodge dodge;

    Vector3 move; //入力値取得用変数
    Vector3 prev_Move = Vector3.zero; //前フレームの移動値保存用変数
    Vector3 prev_Position; //前フレームの位置保存用変数
    Vector3 moveDirection; //移動方向用の変数

    float moveSpeedMultiplier = 1f; //移動量制限用の変数
    float rotateSpeedMultiplier = 1f; //回転量制限用の変数

    public Vector3 Move { get { return move; } }
    public Vector3 Prev_Move { get {  return prev_Move; } }
    public Vector3 Position { get { return target.position; } }
    public Vector3 Prev_Position { get { return prev_Position; } }

    public float MoveSpeedMultiplier { get { return moveSpeedMultiplier; } set { moveSpeedMultiplier = value; } }
    public float RotationSpeedMultiplier { get { return rotateSpeedMultiplier; } set {  rotateSpeedMultiplier = value; } }

    public void MoveProcess() //移動状態の移動処理
    {
        prev_Move = move; //移動の値を保存

        if (move.magnitude > 0.1f) //移動の入力があったら
        {
            CalculateMoveDirection();
            RotateTransform();
            MoveTransform();
        }

        else if (move == prev_Move)
        {
            PlayerActionEvents.IdleEvent();
        }

        prev_Position = target.position; //現在地点を保存
    }

    public void MoveProcess_AnyAttackState() //攻撃状態の移動処理
    {
        if (move.magnitude > 0.1f) //移動の入力があったら
        {
            CalculateMoveDirection();
            RotateTransform();
            MoveTransform();
        }
    }

    void CalculateMoveDirection() //移動方向の計算
    {
        moveDirection = cameraDirectionCalculator.CamForWard * move.z + cameraDirectionCalculator.CamRight * move.x; //移動方向の算出
    }

    void MoveTransform() //移動処理
    {    
        target.transform.position += moveDirection * speed * moveSpeedMultiplier * status.Agility * Time.deltaTime;
    }

    void RotateTransform() //回転処理
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        //回転速度に制限をかける
        target.transform.rotation = Quaternion.RotateTowards(target.transform.rotation, targetRotation, rotationSpeed * rotateSpeedMultiplier * status.Agility * Time.deltaTime);
    }

    public void GetMoveVector(Vector3 getVec)
    {
        move = new Vector3(getVec.x, 0, getVec.z); //入力値の取得
    }

    float AddGravity()
    {
        float gravity = -9.8f * mass * Time.deltaTime;

        return gravity;
    }
}
