using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AppEventManager {
    public static UnityAction<Component, bool> PlayerOnPunch;
	public static UnityAction<Component, bool> EnemyOnPunch;
	public static UnityAction<int> Death;
	public static readonly PlayerEvents Player = new PlayerEvents();
	public static readonly EnemyEvents UI = new EnemyEvents();
	
	
	public class PlayerEvents {
		// This lets us pass a reference to the player component instance
		// and the health value to any listeners
		public UnityAction<Component, bool> OnPunch;
	}
	
	public class EnemyEvents {
		public UnityAction<int> OnButtonPress;
	}
}