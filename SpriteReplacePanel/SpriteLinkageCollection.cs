using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class SpriteLinkageCollection
{
	public IList<SpriteLinkage> linkages;

	public Component[] renderers;

	public SpriteLinkageCollection (Component[] from_renderers)
	{
		this.renderers = from_renderers;		

		this.linkages = new List<SpriteLinkage> ();
		foreach (SpriteRenderer ren in from_renderers) {
			if (linkages.Any (x => x.sprite.Equals (ren.sprite))) {
				linkages.Where (x => x.sprite.Equals (ren.sprite)).First ().renderers.Add (ren);
			} else {
				linkages.Add(new SpriteLinkage(ren)); 
			}
		}
	}

	public void ApplyChanges(){
		linkages.All((x)=>{
			x.ApplyChange();			
			return true;
		});
	}

	public void SaveChanges(){
		linkages.All((x)=>{
			x.SaveChange();			
			return true;
		});
	}


	public void RevertChanges(){
		linkages.All((x)=>{x.RevertChange(); return true;});
	}
}

