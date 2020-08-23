#include "stdafx.h"
#include "TAppCore.h"


TAppCore::TAppCore()
{
    m_pView = new TViewCore();
}


TAppCore::~TAppCore()
{
    if (m_pView != NULL)
    {
        delete m_pView;
        m_pView = NULL;
    }
}


BOOL TAppCore::InitOpenGL(HWND hwnd)
{
    if (m_pView != NULL)
    {
        return m_pView->InitializeOpenGL(hwnd);
    }
    return FALSE;
}

void TAppCore::Render()
{
    if (m_pView != NULL)
    {
        m_pView->Renderscene();
    }
}

void TAppCore::OnSize(INT& nWidth, INT& nHeight)
{
    if (m_pView != NULL)
    {
        m_pView->OnSize(nWidth, nHeight);
    }
}

void TAppCore::SetColorBackground(int red, int green, int blue)
{
    if (m_pView != NULL)
    {
        m_pView->SetColor(red, green, blue);
    }
}