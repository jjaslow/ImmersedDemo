using UnityEngine;
using UnityEngine.UI;
using VRUiKits.Utils;

public class ExampleCardItemAction : MonoBehaviour
{
    public Text title;
    public Text description;

    void Start() {
        // Subscribing to OnCardClick method
        GetComponent<CardItem>().OnCardClicked += ShowDescription;
    }

    void ShowDescription(Card card) {
        title.text = card.title;
        description.text = card.description;
    }
}
