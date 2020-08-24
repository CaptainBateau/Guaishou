using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
	public PlayerEvent _playerEvent;

	void TakeStep()
	{
		_playerEvent.PlayerStep(new PlayerEvent.PlayerStepEventArgs { indoor = false });
	}

}