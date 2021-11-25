using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutate : MonoBehaviour
{
    public float infectionDuration;
    private Renderer[] modelChildren; // villager's bodyparts

    // all the material colors we want to change

    // Define the colours
    private Dictionary<string, Dictionary<CharacterType, Color>> colours = new Dictionary<string, Dictionary<CharacterType, Color>> {
        {"_SKINCOLOR", new Dictionary<CharacterType, Color> {
            {CharacterType.Villager, new Color (0.7264151f, 0.6476059f, 0.4899876f, 1)},
            {CharacterType.Zombie, new Color (0.564f, 0.768f, 0.309f, 1)},
            {CharacterType.Vampire, new Color (0.458f, 0.941f, 0.905f)}
        }},

        {"_EYESCOLOR", new Dictionary<CharacterType, Color> {
            {CharacterType.Zombie, Color.white},
            {CharacterType.Vampire, Color.red}
        }},

        {"_HAIRCOLOR", new Dictionary<CharacterType, Color> {
            {CharacterType.Vampire, Color.black}
        }},

        {"_CLOTH3COLOR", new Dictionary<CharacterType, Color> {
            {CharacterType.Vampire, Color.white}
        }},

        {"_LIPSCOLOR", new Dictionary<CharacterType, Color> {
            {CharacterType.Vampire, new Color(0.65f, 0.5f, 0.5f, 1)}
        }},
        
    };
    

    //private float transitionLength = 3;

    public void Awake()
    {
        infectionDuration = 5.0f;
        modelChildren = GetComponentsInChildren<Renderer>();

        float[] seeds = new float[900];
        for (int i=0; i<900; i++) {
            seeds[i] = Random.value * 2 * (float)System.Math.PI;
        }
        foreach (Renderer modelChild in modelChildren){
            modelChild.material.SetFloatArray("_SPARKLESEEDS", seeds);
        }

        // Get the model default colours for clothes etc as the villager ones
        foreach (var part in colours) {
            if (!part.Value.ContainsKey(CharacterType.Villager)) {
                part.Value.Add(CharacterType.Villager, modelChildren[0].material.GetColor(part.Key));
            }
        }
    }
    

    // Mutates villager into the character that attacked it
    public void MutateInto(CharacterType type, float length)
    {

        foreach (Renderer modelChild in modelChildren){
            modelChild.material.SetFloat("_TRANSITIONSTART", Time.timeSinceLevelLoad);
            modelChild.material.SetFloat("_TRANSITIONLENGTH", length + 0.001f);
            modelChild.material.SetInt("_SKINSTATE", (int) type);

            foreach (var part in colours) {
                
                // Set the previous transition colour to the new base colour
                modelChild.material.SetColor(part.Key, modelChild.material.GetColor(part.Key + "TRANS"));
                
                Color newColour;
                if (part.Value.TryGetValue(type, out newColour)) {
                    // set the new transition colour
                    modelChild.material.SetColor(part.Key + "TRANS", newColour);
                }
            }
        
        }
    }


    
}
