using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValues : MonoBehaviour
{
    [Header("Default Action Texts")]

    [SerializeField] public string edibleSuccessText;
    [SerializeField] public string edibleFailText;
    [SerializeField] public string drinkableSuccessText;
    [SerializeField] public string drinkableFailText;
    [SerializeField] public string talkableSuccessText;
    [SerializeField] public string talkableFailText;
    [SerializeField] public string killableSuccessText;
    [SerializeField] public string killableFailText;
    [SerializeField] public string breakableSuccessText;
    [SerializeField] public string breakableFailText;
    [SerializeField] public string sittableSuccessText;
    [SerializeField] public string sittableFailText;
    [SerializeField] public string usableSuccessText;
    [SerializeField] public string usableFailText;
    [SerializeField] public string pickupableSuccessText;
    [SerializeField] public string pickupableFailText;
    [SerializeField] public string wearableSuccessText;
    [SerializeField] public string wearableFailText;
    [SerializeField] public string openableSuccessText;
    [SerializeField] public string openableFailText;

    [SerializeField] public string lookAtDefaultText;

    [Header("Other Action Texts")]

    [SerializeField] public string edibleNotFoundText;
    [SerializeField] public string drinkableNotFoundText;
    [SerializeField] public string talkableNotFoundText;
    [SerializeField] public string killableNotFoundText;
    [SerializeField] public string breakableNotFoundText;
    [SerializeField] public string sittableNotFoundText;
    [SerializeField] public string usableNotFoundText;
    [SerializeField] public string pickupableNotFoundText;
    [SerializeField] public string wearableNotFoundText;
    [SerializeField] public string openableNotFoundText;
    [SerializeField] public string lookAtNotFoundText;

    [Header("Misc Texts")]

    [SerializeField] public string winText;
    [SerializeField] public string deathText;
    [SerializeField] public string unknownCommand;
}
