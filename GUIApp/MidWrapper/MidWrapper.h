// MidWrapper.h

#pragma once

using namespace System;
#include "..\\NegativeCpp\MathCpp.h"

namespace MidWrapper {
	public ref class WrapperBasic
	{
    public:
        WrapperBasic();
        ~WrapperBasic();
        float CallSum(float a, float b);
    private:
        MathCpp* imp_math_;
	};
}
