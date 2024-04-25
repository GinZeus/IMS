using IMS.Models;

namespace IMS.CoreServices
{
    public interface IPositionService
    {
        IEnumerable<Position> GetPositions();
        string? GetPositionName(int positionId);

        Position GetPositionById(int positionId);

	}
}
