using System;
using UnityEngine;

namespace RegionKit.Modules.ShaderTools {
	public static class ShaderBuffers {

		/// <summary>
		/// The amount of bits needed to activate the stencil buffer.
		/// </summary>
		private const int DEPTH_AND_STENCIL_BUFFER_BITS = 24;

		/// <summary>
		/// Requests that the stencil buffer is enabled, setting the depth bit count of the main screen's render texture to 24 bits.
    /// Once at least one mod calls this, it will be permanently enabled and 
		/// </summary>
		public static void RequestStencilBuffer() {
			if (Futile.screen != null) {
				RenderTexture rt = Futile.screen.renderTexture;
				if (rt.depth < DEPTH_AND_STENCIL_BUFFER_BITS) {
					rt.depth = DEPTH_AND_STENCIL_BUFFER_BITS;
				}
			}
		}

		internal static void Initialize() {
			On.FScreen.ctor += OnConstructingFScreen;
			On.FScreen.ReinitRenderTexture += OnReinitializeRT;
		}

		private static void OnReinitializeRT(On.FScreen.orig_ReinitRenderTexture originalMethod, FScreen @this, int displayWidth) {
			originalMethod(@this, displayWidth);
			@this.renderTexture.depth = _highestRequestedBits;
		}

		private static void OnConstructingFScreen(On.FScreen.orig_ctor originalCtor, FScreen @this, FutileParams futileParams) {
			originalCtor(@this, futileParams);
			@this.renderTexture.depth = _highestRequestedBits;
		}

	}
}