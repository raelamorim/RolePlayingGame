using RolePlayingGame.Application.Dtos.Response;

namespace RolePlayingGame.Application.Mappers
{
    public static class PostBattleMapper
    {
        public static PostBattleResponse ToPostReponse(this string log)
        {
            return new PostBattleResponse
            {
                BattleLog = log
            };
        }
    }
}
