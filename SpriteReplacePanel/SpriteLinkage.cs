using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Text;
public class SpriteLinkage
{
	public Sprite sprite;
	public Sprite sprite_backup;

	public IList<SpriteRenderer> renderers;

	public SpriteLinkage (SpriteRenderer renderer)
	{
		this.sprite = renderer.sprite;
		this.sprite_backup = renderer.sprite;
		this.renderers = new List<SpriteRenderer>();
		this.renderers.Add (renderer);
	}

	public void ApplyChange(){
		foreach (var r in renderers) {
			if (r.sprite != this.sprite) {
				r.sprite = this.sprite;
				EditorGUIUtility.PingObject(r.gameObject);
			}
		}
	}

	public void SaveChange(){
		ApplyChange ();
		this.sprite_backup = this.sprite;
	}

	public void RevertChange(){
		foreach (var r in renderers) {
			r.sprite = this.sprite_backup;
		}
	}

	public string GetDisplayNames(){
		StringBuilder str = new StringBuilder ();
		foreach (var r in renderers) {
			str.Append(_GetGameObjectPath(r.gameObject));
			str.AppendLine ();
		}
		return str.ToString();
	}

	private string _GetGameObjectPath(GameObject obj)
	{
		string path = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			path = obj.name + "/" + path;
		}
		return path;
	}



}

