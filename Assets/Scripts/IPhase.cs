using System.Collections;

namespace CardGame
{
    public interface IPhase
    {
        IEnumerator Execute(GameContext ctx);
    }
}