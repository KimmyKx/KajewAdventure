using System.Collections;
using System.Collections.Generic;

public static class Utility
{
    public static float NumToPercent(float _current, float _max, float _min) =>
        ((_current - _min) / (_max - _min)) * 100;
}
