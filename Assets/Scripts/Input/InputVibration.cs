using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnLightAttackGiven += LightAttackRumble;
        _stateMachine.OnHeavyAttackGiven += HeavyAttackRumble;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttackGiven -= LightAttackRumble;
        _stateMachine.OnHeavyAttackGiven -= HeavyAttackRumble;
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

    public void BlockRumble()
    {
        gamepad.SetMotorSpeeds(BlockLowIntesitiy, BlockHighIntesitiy);
        StartCoroutine(StopRumble(BlockRumbleTime));
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
