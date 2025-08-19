using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct MonsterWalkerJob : IJobEntity
{
    public float DeltaTime;
    [ReadOnly] public FixedList128Bytes<float3> MovePoints;

    public void Execute(in MonsterData monster, ref MonsterWalkerData walker, ref LocalTransform xform)
    {
        if (MovePoints.Length == 0)
            return;

        // 현재 목표 포인트 가져오기
        var targetIndex = walker.CurrentTargetIndex % MovePoints.Length;
        var targetPosition = MovePoints[targetIndex];
        var currentPosition = xform.Position;

        // 목표 지점까지의 방향과 거리 계산
        var direction = targetPosition - currentPosition;
        var distance = math.length(direction);

        // 목표 지점에 도달했는지 확인
        if (distance <= walker.StoppingDistance)
        {
            // 다음 목표 포인트로 이동
            walker.CurrentTargetIndex = (walker.CurrentTargetIndex + 1) % MovePoints.Length;
        }
        else
        {
            // 목표 지점을 향해 이동 (MonsterData.Speed 사용)
            var normalizedDirection = math.normalize(direction);
            var moveDistance = monster.Speed * DeltaTime;

            // 목표 지점을 넘어가지 않도록 제한
            moveDistance = math.min(moveDistance, distance);

            xform.Position += normalizedDirection * moveDistance;

            // 이동 방향으로 회전
            //if (math.lengthsq(normalizedDirection) > 0.001f)
            //{
            //    xform.Rotation = quaternion.LookRotation(normalizedDirection, math.up());
            //}
        }
    }
}