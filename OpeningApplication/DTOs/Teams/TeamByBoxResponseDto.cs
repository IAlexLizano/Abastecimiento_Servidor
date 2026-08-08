namespace OpeningApplication.DTOs.Teams
{
    public class TeamByBoxResponseDto
    {
            public int IdTeam { get; set; }
            public int IdBox { get; set; }
            public string BoxCode { get; set; }
            public IEnumerable<TeamMemberDto> Members { get; set; }
    }
}
