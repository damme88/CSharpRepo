#pragma once
#include "PCam.h"

const double VALUE_AXIS = 10000.0;
// TViewCore command target

class __declspec(dllexport) TViewCore : public CObject
{
public:
	TViewCore();
	virtual ~TViewCore();
    void OnMouseMove(UINT nFlags, CPoint point);
    void OnRButtonDown(UINT nFlags, CPoint point);
    void OnRButtonUp(UINT nFlags, CPoint point);
public:
    BOOL InitializeOpenGL(HWND hwnd);
    void OnSize(const INT& nWidth, const INT& nHeight);
    void Renderscene();
    void SetViewFrustum();
    void SetColor(int red, int green, int blue);
    void MakeAxis();
private:
    HGLRC   m_hRC;
    HDC     m_hDC;

    CPoint ms_down_pt_;
    bool is_ldown_;
    bool is_rdown_;
    PCam p_cameral_;
    int cx_; // size of window
    int cy_;
    GLfloat rendering_rate_;
    float red_;
    float green_;
    float blue_;
};


