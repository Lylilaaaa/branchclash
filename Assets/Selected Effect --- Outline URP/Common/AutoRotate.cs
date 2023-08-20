using UnityEngine;

namespace SelectedEffectOutlineURP
{
	public class AutoRotate : MonoBehaviour
	{
		public float m_Speed = 30f;
		public bool m_AroundAxisY = true;

		void Update()
		{
			if (m_AroundAxisY)
			{
				transform.RotateAround(transform.position, Vector3.up, m_Speed * Time.deltaTime);
			}
			else
			{
				float angle = Time.deltaTime * m_Speed;
				transform.Rotate(angle, angle, 0f);
			}
		}
	}
}