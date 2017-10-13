using UnityEngine;
using System.Collections.Generic;

public class ChipManager : SingletonMono<ChipManager> {
	List<GameObject> liveChips = new List<GameObject>();
	List<GameObject> dieChips = new List<GameObject>();
	int depth = 5;
	public override void initSingletonMono(){
//		this.addChip (300, 1, new Vector3 (500, -100, 0));
//		this.addChip (300, 1, new Vector3 (500, -100, 0));
//		Invoke ("Test", 5);
	}

	public void Test(){
		Debug.Log("ChipManager Test,,");
		this.reciveChip(new Vector3 (500, -100, 0));
	}
	public void addChip(float chip,float dizhu,Vector3 pos){
		int lev = (int)(chip / (dizhu*4))+1;
		if (lev > 5) {
			lev = 5;
		}
		if (dieChips.Count > 0) {
			GameObject go = dieChips[0];
			dieChips.RemoveAt(0);
			go.SetActive(true);
			go.GetComponent<UIPanel>().depth = depth;
			go.GetComponent<Chip>().setChip(chip,lev);
			go.GetComponent<Chip>().fromSend(pos);
			liveChips.Add(go);
		} else {
			GameObject go = Helper.primitiveLoad ("Prefabs/Chip", transform, pos);
			go.GetComponent<UIPanel>().depth = depth;
			go.GetComponent<Chip> ().setChip (chip,lev);
			go.GetComponent<Chip> ().fromSend (pos);
			liveChips.Add (go);
		}
		depth ++;
		AudioController.Instance.SoundPlay("g_addchip");
	}
	public void reciveChip(Vector3 pos){
		depth = 5;
		foreach (GameObject go in liveChips) {
			go.GetComponent<Chip>().reciveTo(pos);
			dieChips.Add(go);
		}
		AudioController.Instance.SoundPlay("win_money");
		liveChips.Clear ();
	}
	public void OnDestroy(){
		foreach (GameObject go in liveChips) {
			Destroy(go);
		}
		liveChips.Clear ();

		foreach (GameObject go in dieChips) {
			Destroy(go);
		}
		dieChips.Clear ();
	}
}
