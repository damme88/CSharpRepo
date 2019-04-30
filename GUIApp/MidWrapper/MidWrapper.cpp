// This is the main DLL file.

#include "stdafx.h"
#include "MidWrapper.h"

MidWrapper::WrapperBasic::WrapperBasic()
{
    imp_math_ = new MathCpp();
}

MidWrapper::WrapperBasic::~WrapperBasic()
{
    delete imp_math_;
}

float MidWrapper::WrapperBasic::CallSum(float a, float b)
{
    return imp_math_->CalSum(a, b);
}