using UnityEngine;

namespace SelectedEffectOutline
{
	[RequireComponent(typeof(Renderer))]
	public class Outline : MonoBehaviour
	{
		public enum Usage { Node_up = 0,Node_down, Viewing, Merge }; //描边的几个用处
		public Usage m_Usage = Usage.Node_up;
		public CursorOutlines col;
		public CursorOutlinesDown col_down;
		public ViewTowerCurSor vtc;
		public enum ETriggerMethod { MouseMove = 0, MouseRightPress, MouseLeftPress };
		[Header("Trigger Method")]
		public ETriggerMethod m_TriggerMethod = ETriggerMethod.MouseMove;
		public bool m_Persistent = false;
		[Header("Outline")]
		public Color m_OutlineColor = Color.green;
		[Range(1f, 10f)] public float m_OutlineWidth = 2f;
		[Range(0f, 1f)] public float m_OutlineFactor = 1f;
		public bool m_WriteZ = false;
		public bool m_BasedOnVertexColorR = false;
		public bool m_OutlineOnly = false;
		[Range(-16f, -1f)] public float m_DepthOffset = -8f;
		public int m_StencilRefValue = 1;
		[Header("Overlay Dash")]
		public bool m_Dash = false;
		public Vector2 m_DashSpeed = new Vector2(0f, 0f);
		public float m_DashFrequency = 0.5f;
		public float m_DashSpaceWidth = 0.2f;
		[Header("Overlay Flash")]
		public Color m_OverlayColor = Color.red;
		[Range(0f, 0.6f)] public float m_Overlay = 0f;
		public bool m_OverlayFlash = false;
		[Range(1f, 6f)] public float m_OverlayFlashSpeed = 3f;
		[Header("Shader")]
		public Shader m_SdrOutlineOnly;
		public Shader m_SdrOutlineStandard;
		Shader m_SdrOriginal;
		Renderer m_Rd;
		
		public bool m_IsMouseOn = false;
		public bool ol_on;
		public bool ol_on_withMouse = false;

		void Awake()
		{
			ol_on_withMouse = false;
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

			if (m_Usage == Usage.Node_down || m_Usage == Usage.Node_up)
			{
				if (!m_IsMouseOn && ol_on == true)
				{
					if (transform.parent.name.Substring(transform.parent.name.Length - 3, 3) == "red")
					{
						if (col_down._canDisappear == true)
						{
							OutlineDisable();
						}
					}
					else
					{
						if (col._canDisappear == true)
						{
							OutlineDisable();
						}
					}
				}

				if (col != null)
				{
					if (col.olWithoutMouse == true && ol_on == false )
					{
						ol_on_withMouse = true;
	                    OutlineEnable();
	                }
	                else if(col.olWithoutMouse == false && ol_on == true && ol_on_withMouse == true)
					{
						ol_on_withMouse = false;
	                    OutlineDisable();
	                }
				}
				else if (col_down != null)
				{
					if (col_down.olWithoutMouse == true && ol_on == false )
					{
						ol_on_withMouse = true;
						OutlineEnable();
					}
					else if(col_down.olWithoutMouse == false && ol_on == true && ol_on_withMouse == true)
					{
						ol_on_withMouse = false;
						OutlineDisable();
					}
				}

			}

			if (m_Usage == Usage.Viewing)
			{
				if (vtc._canDisappear == true && vtc.mouseEnter == false)
				{
					if (GlobalVar._instance.GetState() == GlobalVar.GameState.Viewing)
					{
						OutlineDisable();
					}
				}
				else if (vtc.mouseEnter == true && GlobalVar._instance.GetState() == GlobalVar.GameState.Viewing)
				{
					OutlineEnable();
				}
			}
			
			// material effect parameters
			if (m_OverlayFlash)
			{
				float curve = Mathf.Sin(Time.time * m_OverlayFlashSpeed) * 0.5f + 0.5f;
				m_Overlay = curve * 0.6f;
			}
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
			{
				if (m_Dash)
				{
					mats[i].EnableKeyword("OUTLINE_DASH");
					mats[i].SetVector("_OutlineDashParams", new Vector4(m_DashSpeed.x, m_DashSpeed.y, m_DashFrequency, m_DashSpaceWidth));
				}
				else
				{
					mats[i].DisableKeyword("OUTLINE_DASH");
				}
				mats[i].SetFloat("_OutlineWidth", m_OutlineWidth);
				mats[i].SetColor("_OutlineColor", m_OutlineColor);
				mats[i].SetFloat("_OutlineFactor", m_OutlineFactor);
				mats[i].SetColor("_OverlayColor", m_OverlayColor);
				mats[i].SetFloat("_OutlineWriteZ", m_WriteZ ? 1f : 0f);
				mats[i].SetFloat("_OutlineBasedVertexColorR", m_BasedOnVertexColorR ? 0f : 1f);
				mats[i].SetFloat("_Overlay", m_Overlay);
				mats[i].SetFloat("_DepthOffset", m_DepthOffset);
				mats[i].SetFloat("_RefValue", m_StencilRefValue);
			}
		}
		void OutlineEnable()
		{
			ol_on = true;
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
			{
				if (m_OutlineOnly)
					mats[i].shader = m_SdrOutlineOnly;
				else
					mats[i].shader = m_SdrOutlineStandard;
			}
		}
		void OutlineDisable()
		{
			ol_on = false;
			if (m_Persistent)
				return;
			Material[] mats = m_Rd.materials;
			for (int i = 0; i < mats.Length; i++)
				mats[i].shader = m_SdrOriginal;
		}
		void OnMouseEnter()
		{			
			if (m_Usage == Usage.Node_down || m_Usage == Usage.Node_up)
			{
				if (GlobalVar._instance.global_OL == true)
				{
					if (transform.parent.name.Substring(transform.parent.name.Length - 3, 3) == "red")
                    {
                    	col_down.mouseEnter = true;
                    	m_IsMouseOn = true;
                    	
                    	if (m_TriggerMethod == ETriggerMethod.MouseMove && col_down._canDisappear == true)
                    		OutlineEnable();
                    }
                    else
                    {
                    	col.mouseEnter = true;
                    	m_IsMouseOn = true;
                    				
                    	if (m_TriggerMethod == ETriggerMethod.MouseMove && col._canDisappear == true)
                    		OutlineEnable();
                    }
				}
			}

		}
		void OnMouseExit()
		{
			if (m_Usage == Usage.Node_down || m_Usage == Usage.Node_up)
			{
				if (transform.parent.name.Substring(transform.parent.name.Length - 3, 3) == "red")
				{
					col_down.mouseEnter = false;
					m_IsMouseOn = false;
					if (col_down._canDisappear == true)
					{
						OutlineDisable();
					}
				}
				else
				{
					col.mouseEnter = false;
					m_IsMouseOn = false;
					if (col._canDisappear == true)
					{
						OutlineDisable();
					}
				}
			}
		}
	}
}