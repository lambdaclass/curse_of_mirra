using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUtils
{
    public static  float Dot(Position firstPosition, Position secondPosition){
        return firstPosition.X * secondPosition.X + firstPosition.Y * secondPosition.Y;
    }

    public static Position SubPosition(Position firstPosition, Position secondPosition){
        return new Position{
            X = firstPosition.X - secondPosition.X,
            Y = firstPosition.Y - secondPosition.Y,
        };
    }

    public static Position AddPosition(Position firstPosition, Position secondPosition){
        return new Position{
            X = firstPosition.X + secondPosition.X,
            Y = firstPosition.Y + secondPosition.Y,
        };
    }

    public static Position NormalizedPosition(Position position){
        float length = (float)Math.Sqrt(Math.Pow(position.X, 2.0f) + Math.Pow(position.Y, 2.0f));
        return new Position{
            X = position.X / length,
            Y = position.Y / length
        };
    }

    public static float DistanceToPosition(Position firstPosition, Position secondPosition){
        Position resultPosition =  SubPosition(firstPosition, secondPosition);
        return (float)Math.Sqrt(Math.Pow(resultPosition.X, 2.0f) + Math.Pow(resultPosition.Y, 2.0f));
    }
}
