namespace OpeningApplication.DTOs.Teams
{
    public class AssignTeamToBoxRequestDto
    {
            public int BoxId { get; set; }
            public IEnumerable<TeamMemberDto> TeamMembers { get; set; }
    }
}
