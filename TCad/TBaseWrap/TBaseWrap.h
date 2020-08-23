// TBaseWrap.h

#pragma once

#define WIN32_LEAN_AND_MEAN
#include <afxcoll.h>
#include <windows.h>
#include "TAppCore.h"

using namespace System;
using namespace System::Windows;
using namespace System::Windows::Interop;
using namespace System::Windows::Input;
using namespace System::Windows::Media;
using namespace System::Runtime::InteropServices;

namespace TBaseWrap {

    LRESULT WINAPI WindMsgProc(HWND _hWnd, UINT _msg, WPARAM _wParam, LPARAM _lParam)
    {
        switch (_msg)
        {
        case WM_IME_SETCONTEXT:
            if (LOWORD(_wParam) > 0)
                SetFocus(_hWnd);

            return 0;

        default:
            return DefWindowProc(_hWnd, _msg, _wParam, _lParam);
        }
    }

    public ref class GlWrapperHwnd : public HwndHost
	{
    public:
        GlWrapperHwnd()
        {
            m_pAPPCore = TAppCore::GetInstance();
        }

        ~GlWrapperHwnd()
        {
            if (m_pAPPCore != NULL)
            {
                delete m_pAPPCore;
                m_pAPPCore = NULL;
            }
        }
    protected:
        HINSTANCE   m_hInstance;
        HWND        m_hWnd;
        LPCWSTR     m_WndName;
        LPCWSTR     m_ClassName;
        TAppCore*   m_pAPPCore;

        virtual void OnRenderSizeChanged(SizeChangedInfo^ szInfo) override
        {

            int iHeight = (int)(szInfo->NewSize.Height);
            int iWidth = (int)(szInfo->NewSize.Width);
            if (m_pAPPCore != NULL)
            {
                m_pAPPCore->OnSize(iWidth, iHeight);
            }

        }

        virtual void OnRender(System::Windows::Media::DrawingContext^ drawingContext) override
        {
            if (m_pAPPCore != NULL)
            {
                m_pAPPCore->Render();
            }
        }

        virtual void DestroyWindowCore(HandleRef hwnd) override
        {
            if (NULL != m_hWnd && m_hWnd == (HWND)hwnd.Handle.ToPointer())
            {
                ::DestroyWindow(m_hWnd);
                m_hWnd = NULL;
            }

            UnregisterClass(m_ClassName, m_hInstance);
        }

        bool RegisterWindowClass()
        {
            WNDCLASS wnd_class;
            if (GetClassInfo(m_hInstance, m_ClassName, &wnd_class))
            {
                return true;
            }

            wnd_class.style = CS_HREDRAW | CS_VREDRAW | CS_OWNDC;

            wnd_class.lpfnWndProc = (WNDPROC)WindMsgProc;
            wnd_class.cbClsExtra = 0;
            wnd_class.cbWndExtra = 0;
            wnd_class.hInstance = m_hInstance;
            wnd_class.hIcon = LoadIcon(NULL, IDI_WINLOGO);
            wnd_class.hCursor = LoadCursor(0, IDC_ARROW);
            wnd_class.hbrBackground = 0;
            wnd_class.lpszMenuName = 0;
            wnd_class.lpszClassName = m_ClassName;

            if (!RegisterClass(&wnd_class))
            {
                return false;
            }
            return true;
        }

        virtual HandleRef BuildWindowCore(HandleRef hwndParent) override
        {
            m_hInstance = (HINSTANCE)GetModuleHandle(NULL);
            m_WndName = L"OpenGL in HwndHost";
            m_ClassName = L"OGLClassHwnd";
            if (RegisterWindowClass())
            {
                // some default size
                int iWidth = 2;
                int iHeight = 2;

                DWORD dwStyle = WS_CHILD | WS_VISIBLE;

                // Get the parent (WPF) Hwnd. This is important: Windows won't let you create
                // the Hwnd otherwise.
                HWND parentHwnd = (HWND)hwndParent.Handle.ToPointer();

                m_hWnd = CreateWindowEx(0,
                    m_ClassName,
                    m_WndName,
                    dwStyle,
                    0,	// X Pos
                    0,	// Y Pos
                    iWidth,
                    iHeight,
                    parentHwnd, // Parent
                    0,	// Menu
                    m_hInstance,
                    0); // Param


                if (m_pAPPCore != NULL)
                {
                    m_pAPPCore->InitOpenGL(m_hWnd);
                }

                return HandleRef(this, IntPtr(m_hWnd));
            }
            return HandleRef(nullptr, System::IntPtr::Zero);
        }
	};

    public ref class GlWrapperData
    {
    public:
        GlWrapperData()
        {
            m_pAPPCore = TAppCore::GetInstance();
        }

        ~GlWrapperData()
        {
            if (m_pAPPCore != NULL)
            {
                delete m_pAPPCore;
                m_pAPPCore = NULL;
            }
        }

        void UpdateColorBkgn(int red, int green, int blue)
        {
            m_pAPPCore->SetColorBackground(red, green, blue);
        }
    private:
        TAppCore*   m_pAPPCore;
    };
}
