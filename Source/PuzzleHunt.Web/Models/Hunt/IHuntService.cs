using System.Collections.Generic;

namespace PuzzleHunt.Web.Models
{
    public interface IHuntService
    {
        IEnumerable<HuntSummary> GetHunts();
        CreateHuntStatus CreateHunt(int userID, string huntName);
        HuntModel GetHuntDetails(int huntId, int? currentUserId);
    }
}
