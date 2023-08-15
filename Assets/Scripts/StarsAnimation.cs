using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StarsAnimation : MonoBehaviour
{
    Animator animator;
    public float moveToScoreAnimDuration = 3f;
    

    [SerializeField] GameObject starsObject;
    [SerializeField] GameObject starsAnchor;
    
    
    [SerializeField] GameObject centerStarObject;
    [SerializeField] GameObject rewardDiamondObject;
    [SerializeField] GameObject rewardDiamondAnchor;
    [SerializeField] GameObject rewardText;
    [SerializeField] Transform targetDiamonsObject;

    [SerializeField] Transform targetStarObject;
    [SerializeField] LvlIconPulseAnimation lvlPulseAnimation;
    [SerializeField] MoneyIconPulseAnimation moneyPulseAnimation;

    [SerializeField] MoneyManager moneyManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject nextLvlPanel;
    Animator nextLvlPanelAnimator;
    Vector3 startPosition;
    Vector3 targetPosition;

    Vector3 diamondStartPosition;


    Vector3 initialStarScale;
    Quaternion initialRotation;
    Quaternion targetRotation;

    Vector3 startPos;
    Vector3 endPos;
    Vector3 targetScale;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = starsObject.transform.position;
        targetPosition = targetStarObject.transform.position;
        targetScale = new Vector3(0.3f, 0.3f, 0);
        nextLvlPanelAnimator = nextLvlPanel.GetComponent<Animator>();
    }

    //Вызывается по кнопке
    public void StartAnim()
    {
        animator.SetBool("combine", true);
    }
    //Полёт и вращение звезды
    public void PlayFinalAnimation()
    {
        animator.SetBool("rotate", true);
       // float animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        
        StartCoroutine(MoveStar(starsObject, starsObject, targetStarObject, 0.5f));
        
    }
    
    //После анимации звезды -> анимация кристала (В анимации)
    public void DiamondAnimation()
    {
        StartCoroutine(MoveDiamond(rewardDiamondObject, rewardDiamondObject, targetDiamonsObject, 0.5f));
        
    }

    //Обнуление позиций
    public void Reset()
    {
        animator.SetBool("rotate", false);
        animator.SetBool("combine", false);
        starsObject.transform.localScale = new Vector3(1, 1, 1);
        starsObject.transform.position = starsAnchor.transform.position;
        rewardDiamondObject.transform.position = rewardDiamondAnchor.transform.position;
        rewardDiamondObject.transform.localScale = new Vector3(1, 1, 1);
        
        nextLvlPanelAnimator.SetBool("isEndAnim", false);
        nextLvlPanelAnimator.SetBool("isNextLvl", false);
        starsObject.SetActive(true);
        rewardDiamondObject.SetActive(true);
    }


    IEnumerator MoveStar(GameObject movingObject, GameObject anchorObject, Transform targetObject, float duration)
    {        
        startPos = anchorObject.transform.position;
        targetScale = new Vector3(0.3f, 0.3f, 0);
        var initStarScale = movingObject.transform.localScale;
        float timeElapsed = 0;
        while (timeElapsed <= duration)
        {
            endPos = targetObject.transform.position;
            movingObject.transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / duration);
            movingObject.transform.localScale = Vector3.Lerp(initStarScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        lvlPulseAnimation.StartPulse();
        levelManager.AddLevelCount();
        

    }

    IEnumerator MoveDiamond(GameObject movingObject, GameObject anchorObject, Transform targetObject, float duration)
    {
        startPos = anchorObject.transform.position;
        targetScale = new Vector3(0.3f, 0.3f, 0);
        var initStarScale = movingObject.transform.localScale;
        float timeElapsed = 0;
        while (timeElapsed <= duration)
        {
            endPos = targetObject.transform.position;
            movingObject.transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / duration);
            movingObject.transform.localScale = Vector3.Lerp(initStarScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        moneyPulseAnimation.StartPulse();

        moneyManager.AddMoneyForCompleteLvl();

        yield return new WaitForSeconds(1f);

        
        nextLvlPanelAnimator.SetBool("isEndAnim", true);
        starsObject.SetActive(false);
        rewardDiamondObject.SetActive(false);
    }


    
}
