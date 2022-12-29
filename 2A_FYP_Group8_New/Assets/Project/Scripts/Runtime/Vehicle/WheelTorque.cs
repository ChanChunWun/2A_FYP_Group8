using System;

public struct WheelTorque : IEquatable<WheelTorque>
{
    public float fL, fR, rL, rR;

    public bool Equals(WheelTorque other) =>
        fL == other.fL && fR == other.fR && rL == other.rL && rR == other.rR;
}
