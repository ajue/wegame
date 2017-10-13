namespace KBEngine{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System;
	using LitJson;

	public class DdzRoom : Entity {

		public DdzRoom(){}

		public override void onEnterWorld()
		{
			base.onEnterWorld();
		}

		public override void onLeaveWorld()
		{
			base.onLeaveWorld();
		}

		public override void onEnterSpace(){
			base.onEnterSpace ();

		}
		public override void onLeaveSpace(){
			base.onLeaveSpace ();

		}

		public void onMessage(int recode,int action,string json){
			
		}

		public override void onDestroy(){
			KBEngine.Event.deregisterIn (this);
		}
	}
}
