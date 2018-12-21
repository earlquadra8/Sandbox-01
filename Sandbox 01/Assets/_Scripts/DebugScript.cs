using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestEnumA
{
    A,
    B,
    C
}

public class DebugScript : MonoBehaviour
{
    [System.Serializable]
    public class TestClassA
    {
        public int numA;
        public string stringA;
        public TestEnumA enumA;

        public TestClassA (int num, string text, TestEnumA enm)
        {
            this.numA = num;
            this.stringA= text;
            this.enumA = enm;
        }            
    }

    public TestClassA[] testArr;

	// Use this for initialization
	void Start ()
    {
        testArr = new TestClassA[16];
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnGUI()
    {

        if (GUI.Button(new Rect(10, 10, 200, 200), "Debug"))
        {
#if UNITY_EDITOR
            print("unity editor");
#else
            print("NOT unity editor");
#endif
            foreach (var testClass in testArr)
            {
                print("int: " + testClass.numA + " string: " + testClass.stringA + " enum: " + testClass.numA);
            }
        }
    }
}
