using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Text;
public class SpriteLinkage : AbstractComponentLinkage<SpriteRenderer, Sprite>
{	

	public SpriteLinkage (SpriteRenderer renderer) : base(renderer)
	{
		this.obj = renderer.sprite;
		this.obj_backup = renderer.sprite;
	}

	public override void ApplyChange(){
		foreach (var r in renderers) {
			if (r.sprite != this.obj) {
				r.sprite = this.obj;
				EditorGUIUtility.PingObject(r.gameObject);
			}
		}
	}

	public override void RevertChange(){
		foreach (var r in renderers) {
			r.sprite = this.obj_backup;
		}
	}
}

