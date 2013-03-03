using UnityEngine;
using System.Collections;

public class TheInputMode : MonoBehaviour {

	public enum InputModes{
		IntelPerC,
		LeapMo,
		BitGym
	}
	public static InputModes im = InputModes.IntelPerC;
}
