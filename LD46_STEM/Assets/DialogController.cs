using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogController : MonoBehaviour
{
    public UnityEvent OnFinish;

    private List<string> texts = new List<string> {
        "Hi there. You are a stem cell.",
        "You have one goal:",
        "To EVOLVE.",
        "<color=orange>Nucleotides</color> will help you evolve, but they are scarce in this world. You must seek them out.",
        "You need <color=#00E0FF>oxygen</color> to move around. So if you're not careful, you might run out.",
        "Other cells can help you stay oxygenated. But looks like there are none around...",
        "Luckily, stem cells can make other cells. All it takes is a little bit of <color=red>protein</color>...",
        "If you're confused, <color=green>move your mouse on top of things to learn about them.</color>",
        "But enough talk. Go out and explore!"
    };

    private int current;

    public TMPro.TextMeshProUGUI textField;

    private void Awake() {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(current < texts.Count) {
            textField.text = texts[current];
        }
    }

    public void Advance() {
        current += 1;
        if(current >= texts.Count) {
            OnFinish.Invoke();
        }
    }
}
