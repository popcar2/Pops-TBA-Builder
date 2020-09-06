using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValues : MonoBehaviour
{
    public GenericActionTexts genericActionTexts = new GenericActionTexts();

    public class GenericActionTexts
    {
        public string edibleFailText;
        public string edibleSuccessText;
        public string talkableFailText;
        public string talkableSuccessText;
        public string killableFailText;
        public string killableSuccessText;
        public string sittableFailText;
        public string sittableSuccessText;
        public string usableFailText;
        public string usableSuccessText;
        public string pickupableFailText;
        public string pickupableSuccessText;
        public string wearableFailText;
        public string wearableSuccessText;
        public string openableFailText;
        public string openableSuccessText;
    }
}
