
public class PlayerStatus
{
    public string Name;
    // userPlayer HP
    public int CurHP;
    // Player 이동범위 한정
    public int MoveRange;
    // Player 공격범위 한정
    public int AtkRange;
    // Player 이동속도
    public float MoveSpeed;
    // 생성자
    public PlayerStatus()
    {
        Name = "test";
        CurHP = 50;
        MoveRange = 3;
        AtkRange = 1;
        MoveSpeed = 5f;
    }
}