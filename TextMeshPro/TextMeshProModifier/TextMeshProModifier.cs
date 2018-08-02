using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//[ExecuteInEditMode]
[RequireComponent(typeof(TextMeshPro))]
public class TextMeshProModifier : MonoBehaviour
{

    [System.Serializable]
    public class ModifierUnderlay
    {

        public int order;
        public Color color = Color.white;
        public float offsetX = 0;
        public float offsetY = 0;
        public float softness = 0;
        public float dilate = 0;

        public TextMeshProModifier modifier;
        
        private GameObject m_GameObject;
		private TextMeshPro m_TMP;

        public ModifierUnderlay(TextMeshProModifier modifier)
        {
            //Debug.Log("Init");
            this.modifier = modifier;
            //CreateObject();
        }

        public void CreateObject()
        {
            m_GameObject = new GameObject("Underlay" + (order + 1));

            var refTMP = modifier.GetComponent<TextMeshPro>();
            m_TMP = CopyComponent<TextMeshPro>(refTMP, m_GameObject);
			//hardcode to disable gradient for underlay
			m_TMP.enableVertexGradient = false;
			m_TMP.colorGradient = new VertexGradient(Color.white);
            //CopyComponent<RectTransform>(modifier.GetComponent<RectTransform>(), m_GameObject);

#if UNITY_EDITOR
            //m_GameObject.hideFlags = HideFlags.DontSave;
#endif

            //m_GameObject.hideFlags = HideFlags.NotEditable;
            //var tran = m_GameObject.transform;
            var refTran = modifier.GetComponent<RectTransform>();
            var selfTran = m_GameObject.GetComponent<RectTransform>();

            if (refTran.parent != null)
                selfTran.SetParent(refTran.parent);
            selfTran.anchorMin = refTran.anchorMin;
            selfTran.anchorMax = refTran.anchorMax;
            selfTran.sizeDelta = refTran.sizeDelta;
            selfTran.pivot = refTran.pivot;
            selfTran.position = new Vector3(refTran.position.x + offsetX, refTran.position.y + offsetY, refTran.position.z + (order + 1) * 0.1f);
            selfTran.rotation = refTran.rotation;
            selfTran.localScale = refTran.localScale;

			//copy layer
			m_GameObject.layer = modifier.gameObject.layer;

            //Debug.Log(m_TMP.material);

            m_TMP.fontSharedMaterial = modifier.CacheMaterial;
            m_TMP.color = color;

            var mat = m_GameObject.AddComponent<MaterialOverrideController>();
            mat.RefreshMode = MaterialOverrideRefreshMode.EveryFrame;
            mat.Properties.Add(new MaterialOverrideProperty() {
                name = "_OutlineSoftness",
                type = MaterialOverrideProperty.PropType.Float,
                floatValue = softness
            });
            mat.Properties.Add(new MaterialOverrideProperty()
            {
                name = "_FaceDilate",
                type = MaterialOverrideProperty.PropType.Float,
                floatValue = dilate
            });
        }

        public void DestroyObject()
        {
            DestroyImmediate(m_GameObject);
        }

		public void SetText(string text)
		{
			if (m_TMP == null)
				return;
			m_TMP.SetText(text);
		}

        T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst as T;
        }

    }

    private TextMeshPro m_Text;
    public Transform Transform { get; private set; }
    public Material CacheMaterial { get; private set; }

    public bool enableUnderlay;
    public ModifierUnderlay[] underlay;

    public bool enableCurve;

	private void Awake ()
    {
        m_Text = this.GetComponent<TextMeshPro>();
        CacheMaterial = m_Text.fontSharedMaterial;
        //Debug.Log(CacheMaterial);
        Transform = this.transform;

	}

    private void Start()
    {
        ApplyModifier();
    }

    void OnEnable()
    {
        RegisterEvent();
    }

    void OnDisable()
    {
        UnregisterEvent();
    }

    void RegisterEvent()
    {
        //Debug.Log("Register OnTextChanged");
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void UnregisterEvent()
    {
        //Debug.Log("Unregister OnTextChanged");
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    void OnTextChanged(Object obj)
    {
        if (obj != m_Text)
            return;

		//Debug.Log("Update Text!");

		//AdjustSize();
		UpdateUnderlayModifier();

	}

    public void ApplyModifier()
    {
        if (enableUnderlay)
        {
            ApplyUnderlayModifier();
        }

        m_Text.fontSharedMaterial = CacheMaterial;
    }

    private void ClearUnderlayModifier()
    {
        for (int a = 0; a < underlay.Length; a++)
        {
            underlay[a].DestroyObject();
        }
    }

    private void ApplyUnderlayModifier()
    {
        //Debug.Log("Number: " + underlay.Length);
        for (int a = 0; a < underlay.Length; a++)
        {
            //underlay[a] = new ModifierUnderlay(this, a);
            underlay[a].CreateObject();
        }

        //Underlay = new ModifierUnderlay[1];
        //Underlay[0] = new ModifierUnderlay();
    }

	private void UpdateUnderlayModifier()
	{
		for (int a = 0; a < underlay.Length; a++)
		{
			var item = underlay[a];
			item.SetText(m_Text.text);
		}
	}

#if UNITY_EDITOR

    public void BakeResult()
    {
        Awake();
        //Debug.Log("HI EDITOR");
        ClearUnderlayModifier();
        ApplyModifier();
    }

    public void RemoveCurrentBakeResult()
    {
        //ClearUnderlayModifier();
    }

#endif

    // Update is called once per frame
    /*void Update ()
	{
		
	}*/
}

