using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToyProcessor : MonoBehaviour
{
    public MoneyTracker moneyTracker;
    public ApprovalTracker approvalTracker;

    [System.Serializable]
    public enum ResourceType {None, Money, Approval, Both }
    public ResourceType rewardResourceType;
    public ResourceType penaltyResourceType;

    [System.Serializable]
    public class ICEvent : UnityEvent<float> { }
    public ICEvent OnCorrect;
    public ICEvent OnIncorrect;
    
    public List<ToyType> correctToyTypes;

    public void ProcessToy(ToyController toy) {
        if(correctToyTypes.Contains(toy.toyType)) {
            if (rewardResourceType == ResourceType.Money || rewardResourceType == ResourceType.Both) {
                moneyTracker.GainMoney(toy.toyType.moneyValue);
                OnCorrect.Invoke(toy.toyType.moneyValue);
            }
            if (rewardResourceType == ResourceType.Approval || rewardResourceType == ResourceType.Both) {
                approvalTracker.GainApproval(toy.toyType.approvalValue);
            }
        } else {
            if (penaltyResourceType == ResourceType.Money || penaltyResourceType == ResourceType.Both) {
                moneyTracker.SpendMoney(toy.toyType.moneyValue * toy.toyType.penaltyMultiplier);
            }
            if (penaltyResourceType == ResourceType.Approval || penaltyResourceType == ResourceType.Both) {
                approvalTracker.LoseApproval(toy.toyType.approvalValue * toy.toyType.penaltyMultiplier);
            }
            OnIncorrect.Invoke(0.0f);
        }
    }
}
