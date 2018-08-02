using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class AbstractComponentLinkage<TRen,T> where TRen : Component where T : UnityEngine.Object{
	public IList<TRen> renderers;

	public T obj;
	public T obj_backup;

	public AbstractComponentLinkage (TRen renderer)
	{
		this.renderers = new List<TRen>();
		this.renderers.Add (renderer);
	}


	public abstract void ApplyChange ();

	public void SaveChange(){
		ApplyChange ();
		this.obj_backup = this.obj;
	}

	public abstract void RevertChange ();

	public string GetDisplayNames(){
		StringBuilder str = new StringBuilder ();
		foreach (var r in renderers) {
			str.Append(_GetGameObjectPath(r.gameObject));
			str.AppendLine ();
		}
		return str.ToString();
	}

	protected string _GetGameObjectPath(GameObject obj)
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