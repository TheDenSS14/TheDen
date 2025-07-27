// SPDX-FileCopyrightText: 2025 Cam
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Runtime.CompilerServices;


public sealed class MathUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CubicBezier(float controlPoint1X, float controlPoint1Y, float controlPoint2X, float controlPoint2Y, float progress)
    {
        if (progress <= 0) return 0;
        if (progress >= 1) return 1;

        float parameter = progress;
        const int maxIterations = 10;
        const float epsilon = 0.000001f;

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            float bezierX = GetBezierCoordinate(parameter, controlPoint1X, controlPoint2X);
            float difference = bezierX - progress;
            float absoluteDifference = MathF.Abs(difference);

            if (absoluteDifference < epsilon) break;

            float derivativeX = GetBezierDerivative(parameter, controlPoint1X, controlPoint2X);
            float absoluteDerivative = MathF.Abs(derivativeX);

            if (absoluteDerivative < epsilon) break;

            parameter -= difference / derivativeX;
            parameter = Clamp(parameter, 0, 1);
        }

        return GetBezierCoordinate(parameter, controlPoint1Y, controlPoint2Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EaseCubic(float progress) => CubicBezier(0.25f, 0.1f, 0.25f, 1.0f, progress);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetBezierCoordinate(float parameter, float controlPoint1, float controlPoint2)
    {
        float oneMinusParameter = 1 - parameter;
        float oneMinusParameterSquared = oneMinusParameter * oneMinusParameter;
        float oneMinusParameterCubed = oneMinusParameterSquared * oneMinusParameter;
        float parameterSquared = parameter * parameter;
        float parameterCubed = parameterSquared * parameter;

        float term1 = 3 * oneMinusParameterSquared * parameter * controlPoint1;
        float term2 = 3 * oneMinusParameter * parameterSquared * controlPoint2;
        float term3 = parameterCubed;

        return term1 + term2 + term3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetBezierDerivative(float parameter, float controlPoint1, float controlPoint2)
    {
        float oneMinusParameter = 1 - parameter;
        float oneMinusParameterSquared = oneMinusParameter * oneMinusParameter;
        float parameterSquared = parameter * parameter;

        float term1 = 3 * oneMinusParameterSquared * controlPoint1;
        float term2 = 6 * oneMinusParameter * parameter * (controlPoint2 - controlPoint1);
        float term3 = 3 * parameterSquared * (1 - controlPoint2);

        return term1 + term2 + term3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max) => MathF.Max(min, MathF.Min(max, value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float from, float to, float progress) => from + (to - from) * Clamp(progress, 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InverseLerp(float from, float to, float value) => Clamp((value - from) / (to - from), 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float progress = InverseLerp(fromMin, fromMax, value);
        return Lerp(toMin, toMax, progress);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SmoothStep(float from, float to, float progress)
    {
        float clampedProgress = Clamp(progress, 0, 1);
        float smoothed = clampedProgress * clampedProgress * (3 - 2 * clampedProgress);
        return Lerp(from, to, smoothed);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(float value) => (value > 0f ? 1 : 0) - (value < 0f ? 1 : 0);
}
