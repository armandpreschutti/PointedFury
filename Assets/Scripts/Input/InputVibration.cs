using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputVibration : MonoBehaviour
{
    private Gamepad gamepad;
    [SerializeField] StateMachine _stateMachine;

    public float LightAttackRumbleTime;
    public float LightAttackLowIntensity;
    public float LightAttackHighIntensity;

    public float HeavyAttackRumbleTime;
    public float HeavyAttackLowIntensity;
    public float HeavyAttackHighIntensity;

    public float BlockRumbleTime;
    public float BlockLowIntesitiy;
    public float BlockHighIntesitiy;

    public float DeflectRumbleTime;
    public float DeflectLowIntesitiy;
    public float DeflectHighIntesitiy;

    public float ParryRumbleTime;
    public float ParryLowIntesitiy;
    public float ParryHighIntesitiy;

    public float ParriedRumbleTime;
    public float ParriedLowIntesitiy;
    public float ParriedHighIntesitiy;

    public float HurtRumbleTime;
    public float HurtLowIntesitiy;
    public float HurtHighIntesitiy;

    public float ErrorRumbleTime;
    public float ErrorLowIntesitiy;
    public float ErrorHighIntesitiy;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnLightAttackGiven += LightAttackRumble;
        _stateMachine.OnHeavyAttackGiven += HeavyAttackRumble;
        _stateMachine.OnBlockSuccessful += BlockRumble;
        _stateMachine.OnDeflectSuccessful += DeflectRumble;
        _stateMachine.OnParrySuccessful += ParryRumble;
        _stateMachine.OnParryRecieved += ParriedRumble;
        _stateMachine.OnHurt += HurtRumble;
        UserInput.OnInputError += ErrorRumble;
        PauseMenuController.OnGamePaused += StopAllRumble;
        
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttackGiven -= LightAttackRumble;
        _stateMachine.OnHeavyAttackGiven -= HeavyAttackRumble;
        _stateMachine.OnBlockSuccessful -= BlockRumble;
        _stateMachine.OnDeflectSuccessful -= DeflectRumble;
        _stateMachine.OnParrySuccessful -= ParryRumble;
        _stateMachine.OnParryRecieved -= ParriedRumble;
        _stateMachine.OnHurt -= HurtRumble;
        UserInput.OnInputError -= ErrorRumble;
        PauseMenuController.OnGamePaused -= StopAllRumble;
    }

    void Start()
    {
        gamepad = Gamepad.current;
        if (gamepad == null)
        {
            this.enabled = false;
        }
    }

    public void LightAttackRumble()
    {
        gamepad.SetMotorSpeeds(LightAttackLowIntensity, LightAttackHighIntensity);
        StartCoroutine(StopRumble(LightAttackRumbleTime));
    }

    public void HeavyAttackRumble()
    {
        gamepad.SetMotorSpeeds(HeavyAttackLowIntensity, HeavyAttackHighIntensity);
        StartCoroutine(StopRumble(HeavyAttackRumbleTime));
    }

    public void BlockRumble(float value, string type)
    {
        gamepad.SetMotorSpeeds(BlockLowIntesitiy, BlockHighIntesitiy);
        StartCoroutine(StopRumble(BlockRumbleTime));
    }

    public void DeflectRumble()
    {
        gamepad.SetMotorSpeeds(DeflectLowIntesitiy, DeflectHighIntesitiy);
        StartCoroutine(StopRumble(DeflectRumbleTime));
    }

    public void ParryRumble()
    {
        gamepad.SetMotorSpeeds(ParryLowIntesitiy, ParryHighIntesitiy);
        StartCoroutine(StopRumble(ParryRumbleTime));
    }

    public void ParriedRumble(float value, string type)
    {
        gamepad.SetMotorSpeeds(ParriedLowIntesitiy, ParriedHighIntesitiy);
        StartCoroutine(StopRumble(ParriedRumbleTime));
    }

    public void HurtRumble()
    {
        gamepad.SetMotorSpeeds(HurtLowIntesitiy, HurtHighIntesitiy);
        StartCoroutine(StopRumble(HurtRumbleTime));
    }

    public void StopAllRumble(bool value)
    {
        gamepad.SetMotorSpeeds(0,0);
    }

    public void ErrorRumble()
    {
        gamepad.SetMotorSpeeds(ErrorLowIntesitiy, ErrorHighIntesitiy);
        StartCoroutine(StopRumble(ErrorRumbleTime));
    }

    private IEnumerator StopRumble(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f); 
        }
    }

}
