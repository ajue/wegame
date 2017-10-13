using UnityEngine;
using System.Collections;

public interface GameUIHandlerInterface : GameHandler {
	bool isSingle();
	bool isAllways();
	void Show();
	void UnShow();
}
