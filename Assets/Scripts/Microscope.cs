using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(MicroscopeRenderer), PostProcessEvent.AfterStack, "Custom/Microscope")]
public sealed class Microscope : PostProcessEffectSettings {
  [Range(0f, 1f), Tooltip("Microscope effect intensity.")]
  public FloatParameter blend = new FloatParameter { value = 0.5f };
  [Range(-10f, 10f), Tooltip("Microscope effect spread.")]
  public FloatParameter spread = new FloatParameter { value = 1.5f };
}

public sealed class MicroscopeRenderer : PostProcessEffectRenderer<Microscope> {
  public override void Render(PostProcessRenderContext context) {
    var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Microscope"));
    sheet.properties.SetFloat("_Blend", settings.blend);
    sheet.properties.SetFloat("_Spread", settings.spread);
    context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
  }
}
