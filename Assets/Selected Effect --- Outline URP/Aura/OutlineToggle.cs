using UnityEngine;

namespace SelectedEffectOutline
{
	[RequireComponent(typeof(Renderer))]
	public class OutlineToggle : MonoBehaviour
	{
		public enum ETriggerMethod { MouseMove = 0, MouseRightPress, MouseLeftPress };
		[Header("Trigger Method")]
		public ETriggerMethod m_TriggerMethod = ETriggerMethod.MouseMove;
		public bool m_Persistent = false;
		Renderer m_Rd;
		bool m_IsMouseOn = false;

		void Start()
		{
			m_Rd = GetComponent<Renderer>();
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
				mats[i].SetShaderPassEnabled("LightweightForward", false);
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
		}
		void OutlineEnable()
		{
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
				mats[i].SetShaderPassEnabled("LightweightForward", true);
		}
		void OutlineDisable()
		{
			if (m_Persistent)
				return;
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
				mats[i].SetShaderPassEnabled("LightweightForward", false);
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
