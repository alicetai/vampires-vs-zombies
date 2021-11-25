using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    private SortedList<(float, int), System.Action> toExecute = new SortedList<(float, int), System.Action>();
    private int keyComposite = 0;
    public void ExecuteAfter(System.Action func, float time) {
        toExecute.Add((Time.fixedTime + time, keyComposite), func);
        keyComposite++;
        
    }
    // Start is called before the first frame update
    void Awake()
    {
        Mutate mutate = this.gameObject.AddComponent<Mutate>();

        
        int next = 0;

        void queueTrans() {
            mutate.MutateInto(CharacterType.Villager, 0);
            ExecuteAfter(() => {
                mutate.MutateInto((CharacterType)(next + 1), 3f);
                next = (next + 1) % 2;
            }, 2f);
            ExecuteAfter(queueTrans, 10f);
        }

        queueTrans();

        //ExecuteAfter(() => mutate.MutateInto(CharacterType.Vampire, 3f), 3f);
        
        
    }


    // Update is called once per frame
    void Update()
    {
        while (toExecute.Count > 0 && Time.fixedTime > toExecute.Keys[0].Item1) {
            toExecute.Values[0]();
            toExecute.RemoveAt(0);

        }
        
    }
}
