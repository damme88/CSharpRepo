#pragma once

// TViewCore command target

class __declspec(dllexport) TViewCore : public CObject
{
public:
	TViewCore();
	virtual ~TViewCore();

public:
    BOOL InitializeOpenGL(HWND hwnd);
    void OnSize(const INT& nWidth, const INT& nHeight);
    void Renderscene();
    void SetColor(int red, int green, int blue);
private:
    HGLRC   m_hRC;
    HDC     m_hDC;

    float red_;
    float green_;
    float blue_;
};


