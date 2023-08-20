using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SelectedEffectOutlineURP
{
	public class OutlineFeature : ScriptableRendererFeature
	{
		public class CustomPass : ScriptableRenderPass
		{
			ShaderTagId k_TagID = new ShaderTagId("DepthOnly");

			public Material m_MatFlatColor;
			public Material m_MatFunc;
			public float m_BlurPixelOffset = 1.2f;
			public float m_GlowIntensity = 1f;
			public Color m_GlowColor;
			public RenderTargetIdentifier m_Source;
			//int m_RtPropID1 = 0;
			FilteringSettings m_FilterSettings;
			public LayerMask m_LayerMask;
			RenderTargetIdentifier m_RtA;
			RenderTargetIdentifier m_RtB;
			RenderTargetIdentifier m_RtC;
			RenderTargetIdentifier m_RtD;
			int m_RtAID = 0;
			int m_RtBID = 0;
			int m_RtCID = 0;
			int m_RtDID = 0;

			public CustomPass(LayerMask lm)
			{
				this.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
				m_FilterSettings = new FilteringSettings(RenderQueueRange.all, lm);
			}
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				int w = cameraTextureDescriptor.width;
				int h = cameraTextureDescriptor.height;

				m_RtAID = Shader.PropertyToID("rtA");
				cmd.GetTemporaryRT(m_RtAID, w, h, 16, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtA = new RenderTargetIdentifier(m_RtAID);

				m_RtBID = Shader.PropertyToID("rtB");
				cmd.GetTemporaryRT(m_RtBID, w, h, 16, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtB = new RenderTargetIdentifier(m_RtBID);

				m_RtCID = Shader.PropertyToID("rtC");
				cmd.GetTemporaryRT(m_RtCID, w, h, 16, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtC = new RenderTargetIdentifier(m_RtCID);

				m_RtDID = Shader.PropertyToID("rtD");
				cmd.GetTemporaryRT(m_RtDID, w, h, 16, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
				m_RtD = new RenderTargetIdentifier(m_RtDID);

				ConfigureTarget(m_RtA);
				ConfigureClear(ClearFlag.All, Color.black);
			}
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				CommandBuffer cmd = CommandBufferPool.Get("OutlineImageBased");

				// draw orignal to rt
				Blit(cmd, m_Source, m_RtD);

				// draw glow object to rt
				SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
				DrawingSettings drawSettings = CreateDrawingSettings(k_TagID, ref renderingData, sortingCriteria);
				drawSettings.overrideMaterial = m_MatFlatColor;
				context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilterSettings);

				// blur pass, must use global material parameters
				cmd.SetGlobalVector("_Global_Offsets", new Vector4(0f, m_BlurPixelOffset, 0f, 0f));
				Blit(cmd, m_RtA, m_RtB, m_MatFunc, 0);
				cmd.SetGlobalVector("_Global_Offsets", new Vector4(m_BlurPixelOffset, 0f, 0f, 0f));
				Blit(cmd, m_RtB, m_RtC, m_MatFunc, 0);

				cmd.SetGlobalVector("_Global_Offsets", new Vector4(0f, m_BlurPixelOffset, 0f, 0f));
				Blit(cmd, m_RtC, m_RtB, m_MatFunc, 0);
				cmd.SetGlobalVector("_Global_Offsets", new Vector4(m_BlurPixelOffset, 0f, 0f, 0f));
				Blit(cmd, m_RtB, m_RtC, m_MatFunc, 0);

				// final compose pass
				cmd.SetGlobalTexture("_ObjectTex", m_RtA);
				cmd.SetGlobalTexture("_GlowTex", m_RtC);
				cmd.SetGlobalTexture("_OrigTex", m_RtD);
				cmd.SetGlobalColor("_GlowColor", m_GlowColor);
				cmd.SetGlobalFloat("_GlowIntensity", m_GlowIntensity);
				Blit(cmd, m_RtC, m_Source, m_MatFunc, 1);

				context.ExecuteCommandBuffer(cmd);
				CommandBufferPool.Release(cmd);
			}
			public override void FrameCleanup(CommandBuffer cmd)
			{
				cmd.ReleaseTemporaryRT(m_RtAID);
				cmd.ReleaseTemporaryRT(m_RtBID);
				cmd.ReleaseTemporaryRT(m_RtCID);
				cmd.ReleaseTemporaryRT(m_RtDID);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public Material m_MatFlatColor;
		public Material m_MatFunc;
		public LayerMask m_LayerMask;
		public float m_BlurPixelOffset = 1.2f;
		[Range(0.1f, 3f)] public float m_GlowIntensity = 2f;
		public Color m_GlowColor = Color.white;
		CustomPass m_Pass;

		public override void Create()
		{
			m_Pass = new CustomPass(m_LayerMask);
		}
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			RenderTargetIdentifier src = renderer.cameraColorTarget;
			if (m_MatFlatColor == null || m_MatFunc == null)
			{
				Debug.LogWarningFormat("Missing materials. {0} pass will not execute.", GetType().Name);
				return;
			}
			m_Pass.m_Source = src;
			m_Pass.m_MatFlatColor = m_MatFlatColor;
			m_Pass.m_MatFunc = m_MatFunc;
			m_Pass.m_BlurPixelOffset = m_BlurPixelOffset;
			m_Pass.m_GlowIntensity = m_GlowIntensity;
			m_Pass.m_GlowColor = m_GlowColor;
			renderer.EnqueuePass(m_Pass);
		}
	}
}