using IMS.Data;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreServices.Implementations
{
    public class PositionService : IPositionService
    {
        private readonly ApplicationDbContext _context;

        public PositionService(ApplicationDbContext context)
        {
            _context = context;
        }

		public string? GetPositionName(int positionId)
		{
            Position? position = _context.Positions.Where(p => p.PositionId == positionId).FirstOrDefault();
			if (position == null)
            {
                return null;
            }
            return position.PositionName;
		}

		public IEnumerable<Position> GetPositions() 
        {
            return _context.Positions;
        }

        public Position GetPositionById(int positionId) {
			Position position = _context.Positions.Where(p => p.PositionId == positionId).FirstOrDefault();
            return position;
		}
    }
}
