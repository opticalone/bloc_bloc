using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

//public class AvPostProcessAffector : MonoBehaviour
//{
//    PostProcessVolume m_Volume;
//    Vignette m_Vignette;

//    public int band;

//    void Start()
//    {
//        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
//        m_Vignette.enabled.Override(true);
//        m_Vignette.intensity.Override(1f);

//        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);
//    }

//    void Update()
//    {
//        m_Vignette.intensity.value = AudioVis._audioBandBuffer[band] * 2;
//    }

//    void OnDestroy()
//    {
//        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
//    }
//}
public class AvPostProcessAffector : MonoBehaviour
{
    Camera cam;
    public Transform lookPoint;
    PostProcessVolume m_Volume;
   
    LensDistortion m_Lens;
    public int band;

    void Start()
    {
        cam = GetComponent<Camera>();

        m_Lens = ScriptableObject.CreateInstance<LensDistortion>();
        m_Lens.enabled.Override(true);
        m_Lens.scale.Override(1f);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Lens);
    }

    void Update()
    {
        transform.LookAt(lookPoint);
        m_Lens.scale.value = (AudioVis._audioBandBuffer[band] + 1)*2;
        cam.fieldOfView = 35 + (-AudioVis._audioBandBuffer[band] * 15);
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}
