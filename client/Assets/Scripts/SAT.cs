using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using UnityEngine;

/*
    Collision detection using the [SAT theorem](https://dyn4j.org/2010/01/sat/)
    To determine if a pair of shapes are colliding we'll try to draw a line from an axis where the entities
    are not overlaped, if we found at least one axis that meet this we can ensure that the entities are not
    overlaping

    DISCLAIMER: this algorithm works for collisions with polygons that are CONVEX ONLY which means
    that ALL of his internal angles has less than 180° degrees, if we would have a CONCAVE polygon this
    should be built differently. The usage of various convex polygons can be a solution.

*/

// Handle the intesection between a circle and a polygon, the return value is a tuple of 3 elements:
// 1: bool = true if the entities are colliding
// 2: Position = nomalized line of collision
// 3: f32 = the minimum amount of overlap between the shapes that would solve the collision
public class SAT
{
    
    public static (bool, Position, float) IntersectCirclePolygon(
        Entity circle,
        Entity polygon
    ){


        List<Entity> obstacles = GameServerConnectionManager.Instance.obstacles;
        List<Position> vertexList = new List<Position>(polygon.Vertices);
        // The normal will be the vector in which the polygons should move to stop colliding
        Position normal = new Position{X = 0f, Y = 0f};
        float resultDepth = float.MaxValue;

        Position axis;

        float minPolygonCastPoint; 
        float maxPolygonCastPoint;
        float minCircleCastPoint; 
        float maxCircleCastPoint;
        float circleOverlapDepth;
        float polygonOverlapDepth;
        float minDepth;

        for (int i = 0; i < polygon.Vertices.Count; i++)
        {
            int nextVertexIndex = i + 1;
            if(nextVertexIndex == polygon.Vertices.Count){
                nextVertexIndex = 0;
            }
            Position currentVertex = polygon.Vertices[i];
            Position nextVertex = polygon.Vertices[nextVertexIndex];

             Position currentLine = PositionUtils.SubPosition(currentVertex, nextVertex);
            // the axis will be the perpendicular line drawn from the current line of the polygon
            axis = PositionUtils.NormalizedPosition(new Position {
                X = -currentLine.Y,
                Y = currentLine.X
            });

            (minPolygonCastPoint, maxPolygonCastPoint) = ProjectVertices(vertexList, axis);
            (minCircleCastPoint, maxCircleCastPoint) = ProjectCircle(circle, axis);
            if (minPolygonCastPoint >= maxCircleCastPoint
                || minCircleCastPoint >= maxPolygonCastPoint)
            {
                return (false, normal, resultDepth);
            }

            circleOverlapDepth = maxCircleCastPoint - minPolygonCastPoint;

            polygonOverlapDepth = maxPolygonCastPoint - minCircleCastPoint;

            minDepth = Math.Min(circleOverlapDepth, polygonOverlapDepth);
            
            if (minDepth < resultDepth) {
                // If we hit the polygon from the right or top we need to turn around the direction
                if(polygonOverlapDepth > circleOverlapDepth) {
                    normal = new Position {
                        X = -axis.X,
                        Y = -axis.Y
                    };
                } else {
                    normal = axis;
                }
                resultDepth = minDepth;
            }
        }

    // Check normal and depth for center
    Position closestVertex = FindClosestVertex(circle.Position, vertexList);
    axis = PositionUtils.NormalizedPosition(PositionUtils.SubPosition(closestVertex, circle.Position));

    (minPolygonCastPoint, maxPolygonCastPoint) = ProjectVertices(vertexList, axis);
    (minCircleCastPoint, maxCircleCastPoint) = ProjectCircle(circle, axis);

    // If there's a gap between the polygon it means they do not collide and we can safely return false
    if (minPolygonCastPoint >= maxCircleCastPoint
        || minCircleCastPoint >= maxPolygonCastPoint)
    {
        return (false, normal, resultDepth);
    }

    circleOverlapDepth = maxCircleCastPoint - minPolygonCastPoint;

    polygonOverlapDepth = maxPolygonCastPoint - minCircleCastPoint;

    minDepth = Math.Min(circleOverlapDepth, polygonOverlapDepth);
    
    if (minDepth < resultDepth) {
        // If we hit the polygon from the right or top we need to turn around the direction
        if(polygonOverlapDepth > circleOverlapDepth) {
            normal = new Position {
                X = -axis.X,
                Y = -axis.Y
            };
        } else {
            normal = axis;
        }
        resultDepth = minDepth;
    }

    return (true, normal, resultDepth);
    }

    public static (float, float) ProjectVertices(List<Position> vertices, Position axis){
        float min = float.MaxValue;
        float max = float.MinValue;
        foreach (Position position in vertices)
        {
            float projection = PositionUtils.Dot(position, axis);

            if(projection < min){
                min = projection;
            }
            if(projection > max){
                max = projection;
            }
        }

        return (min, max);
    }

    public static (float, float) ProjectCircle(Entity circle, Position axis){
        float min = float.MaxValue;
        float max = float.MinValue;

        Position directionRadius = new Position{
            X = axis.X * circle.Radius,
            Y = axis.Y * circle.Radius
        };

        Position positionPlusRadius = PositionUtils.AddPosition(circle.Position, directionRadius);
        Position positionSubRadius = PositionUtils.SubPosition(circle.Position, directionRadius);

        min = PositionUtils.Dot(positionPlusRadius, axis);
        max = PositionUtils.Dot(positionSubRadius, axis);

        if(min > max){
            var temp = min;
            min = max;
            max = temp;
        }

        return (min, max);
    }

    public static Position FindClosestVertex(Position center, List<Position> vertices){
        Position result = new Position { X = 0f, Y = 0f };
        float minDistance = float.MaxValue;

        foreach (Position currentPosition in vertices){
            float distance = PositionUtils.DistanceToPosition(center, currentPosition);

            if(distance < minDistance){
                minDistance = distance;
                result = currentPosition;
            }
        }
        return result;
    }
}
