using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTraceMaster : MonoBehaviour {
    public ComputeShader RayTracingShader;
    private RenderTexture _target;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Render(destination);
    }
    private void Render(RenderTexture destination)
    {
        // Make sure we have a current render target
        InitRenderTexture();
        // Set the target and dispatch the compute shader
        RayTracingShader.SetTexture(0, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        // Blit the result texture to the screen
        Graphics.Blit(_target, destination);
    }
    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();
            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }
    private Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void SetShaderParameters()
    {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
    }
}
    //public ComputeShader RayTracingShader;

    //private RenderTexture _target;
    //Camera cam;

    //private void Awake()
    //{
    //    cam = GetComponent<Camera>();
    //}
    
    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    SetShaderParams();
    //    Render(destination);
    //}
    //private void SetShaderParams()
    //{
    //    RayTracingShader.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
    //    RayTracingShader.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
    //}

    //private void InitRenderTexture()
    //{
    //    if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
    //    {
    //        //release texture if we already haveone
    //        if (_target != null)
    //        {
    //            _target.Release();
    //            _target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
    //            _target.enableRandomWrite = true;
    //            _target.Create();
    //        }
    //    }
    //}
    //private void Render(RenderTexture destination)
    //{
    //    //validate current render target
    //    InitRenderTexture();
    //    //set target and dispatch to the compute shader

    //    RayTracingShader.SetTexture(0, "Result", _target);
    //    int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
    //    int threadGroupsY = Mathf.CeilToInt(Screen.width / 8.0f);
    //    RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

    //    //blit the result texture to screen
    //    Graphics.Blit(_target, destination);
    //}

    

