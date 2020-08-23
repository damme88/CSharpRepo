#pragma once
#include "TViewCore.h"

class __declspec(dllexport) TAppCore
{
public:

    TAppCore();
    ~TAppCore();
    BOOL InitOpenGL(HWND hwnd);
    void Render();
    void OnSize(INT& nWidth, INT& nHeight);
    static TAppCore* GetInstance()
    {
        if (instance_ == NULL)
        {
            instance_ = new TAppCore();
        }
        return instance_;
    }
    void SetColorBackground(int red, int green, int blue);
protected:
    TViewCore* m_pView;
    static TAppCore* instance_;
};

TAppCore* TAppCore::instance_ = NULL;