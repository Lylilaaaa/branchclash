using UnityEngine;

namespace SelectedEffectOutline
{
	[RequireComponent(typeof(Renderer))]
	public class OcclusionOutline : MonoBehaviour
	{
		public enum ETriggerMethod { MouseMove = 0, MouseRightPress, MouseLeftPress };
		public enum EDrawMethod { OutlineInvisible = 0, OutlineAll, OutlineVisible, OutlineAndFill };
		[Header("Trigger")]
		public ETriggerMethod m_TriggerMethod = ETriggerMethod.MouseMove;
		public EDrawMethod m_DrawMethod = EDrawMethod.OutlineAll;
		public bool m_Persistent = false;
		[Header("Outline")]
		public Color m_OutlineColor = Color.green;
		[Range(1f, 10f)] public float m_OutlineWidth = 2f;
		[Range(0f, 1f)] public float m_OutlineFactor = 1f;
		public bool m_VertexColorR = false;
		[Header("Shader")]
		public Shader m_SdrOutline;
		Shader m_SdrOriginal;
		Renderer m_Rd;
		bool m_IsMouseOn = false;

		void Start()
		{
			m_Rd = GetComponent<Renderer>();
			m_SdrOriginal = m_Rd.material.shader;
		}
		void Update()
		{
			if (m_TriggerMethod == ETriggerMethod.MouseRightPress)
			{
				bool on = m_IsMouseOn && Input.GetMouseButton(1);
				if (on)
					OutlineEnable();
				else
					OutlineDisable();
			}
			else if (m_TriggerMethod == ETriggerMethod.MouseLeftPress)
			{
				bool on = m_IsMouseOn && Input.GetMouseButton(0);
				if (on)
					OutlineEnable();
				else
					OutlineDisable();
			}
			// material effect parameters
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
			{
				mats[i].SetFloat("_OutlineWidth", m_OutlineWidth);
				mats[i].SetColor("_OutlineColor", m_OutlineColor);
				mats[i].SetFloat("_OutlineFactor", m_OutlineFactor);
				mats[i].SetFloat("_OutlineBasedVertexColorR", m_VertexColorR ? 0f : 1f);
				if (m_DrawMethod == EDrawMethod.OutlineInvisible)
				{
					mats[i].SetInt("_ZTest2ndPass", (int)UnityEngine.Rendering.CompareFunction.Always);
					mats[i].SetInt("_ZTest3rdPass", (int)UnityEngine.Rendering.CompareFunction.Greater);
				}
				if (m_DrawMethod == EDrawMethod.OutlineAll)
				{
					mats[i].SetInt("_ZTest2ndPass", (int)UnityEngine.Rendering.CompareFunction.Always);
					mats[i].SetInt("_ZTest3rdPass", (int)UnityEngine.Rendering.CompareFunction.Always);
				}
				if (m_DrawMethod == EDrawMethod.OutlineVisible)
				{
					mats[i].SetInt("_ZTest2ndPass", (int)UnityEngine.Rendering.CompareFunction.Always);
					mats[i].SetInt("_ZTest3rdPass", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
				}
				if (m_DrawMethod == EDrawMethod.OutlineAndFill)
				{
					mats[i].SetInt("_ZTest2ndPass", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
					mats[i].SetInt("_ZTest3rdPass", (int)UnityEngine.Rendering.CompareFunction.Always);
				}
			}
		}
		void OutlineEnable()
		{
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
				mats[i].shader = m_SdrOutline;
		}
		void OutlineDisable()
		{
			if (m_Persistent)
				return;
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
				mats[i].shader = m_SdrOriginal;
		}
		void OnMouseEnter()
		{
			m_IsMouseOn = true;
			if (m_TriggerMethod == ETriggerMethod.MouseMove)
				OutlineEnable();
		}
		void OnMouseExit()
		{
			m_IsMouseOn = false;
			OutlineDisable();
		}
	}
}