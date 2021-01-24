using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ComputeShaderRun : MonoBehaviour
{

    public ComputeShader _shader;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private int _multiplier = 4;
    private int lastFrameMultiplier;

    private int _textureSizeX = 640;
    private int _textureSizeY = 480;

    private RenderTexture _renderTexture;
    private RenderTexture _sourceTex;

    Renderer _renderer;
    int _lastScreenWidth = 0;
    int _lastScreenHeight = 0;

    private int _ditherKernelHandle;
    private int _lerpedKernelHandle;
    private int _noneKernelHandle;
    private int _currentKernelHandle;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScreenSize();
        _sourceTex.enableRandomWrite = true;


        _ditherKernelHandle = _shader.FindKernel("Dither");
        _lerpedKernelHandle = _shader.FindKernel("LerpedBlend");
        _noneKernelHandle = _shader.FindKernel("None");
        _currentKernelHandle = _ditherKernelHandle;

    }

    public void SetMultiplier(float multiplier)
    {
        _multiplier = Mathf.RoundToInt( multiplier);
    }

    private const int KERNALSIZE = 30;
    void UpdateScreenSize()
    {
        if (_renderTexture)
            Destroy(_renderTexture);
        if (_sourceTex)
            Destroy(_sourceTex);


        _textureSizeX = ((Screen.width / _multiplier) / KERNALSIZE+1) * KERNALSIZE;
        _textureSizeY = ((Screen.height / _multiplier) / KERNALSIZE+1) * KERNALSIZE;

        Debug.Log("Render size = " + _textureSizeX + " x" + _textureSizeY +" Y");

        _renderTexture = new RenderTexture(_textureSizeX, _textureSizeY, 24);
        _renderTexture.enableRandomWrite = true;
        _renderTexture.Create();

        _renderTexture.filterMode = FilterMode.Point;




        _sourceTex = new RenderTexture(_textureSizeX, _textureSizeY, 24);
        _sourceTex.enableRandomWrite = true;
        _sourceTex.Create();
        _sourceTex.filterMode = FilterMode.Point;
        _camera.targetTexture = _sourceTex;


        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;

        transform.localScale = new Vector3((float)_textureSizeX / _textureSizeY, 1.0f,1.0f);

    }


    // Update is called once per frame
    void Update()
    {

        if (_lastScreenWidth != Screen.width || _lastScreenHeight != Screen.height || lastFrameMultiplier != _multiplier)
        {
            lastFrameMultiplier = _multiplier;
            _multiplier = Mathf.Max(_multiplier, 1);
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            Debug.Log("Screensize changed");
            UpdateScreenSize();
        }



        _shader.SetTexture(_currentKernelHandle, "Input", _sourceTex);
        _shader.SetTexture(_currentKernelHandle, "Result",  _renderTexture);
        _shader.Dispatch(_currentKernelHandle, _textureSizeX/ KERNALSIZE, _textureSizeY/ KERNALSIZE, 1);
        
        _renderer.material.SetTexture("_MainTex", _renderTexture);
    }

    public void ChangeRenderMode(int ID)
    {
        switch (ID)
        {
            case 0:
                _currentKernelHandle = _ditherKernelHandle;
                break;
            case 1:
                _currentKernelHandle = _lerpedKernelHandle;
                break;
            case 2:
                _currentKernelHandle = _noneKernelHandle;
                break;
        }
    }

}
