using System.Linq;

namespace Covide.Web.Services
{
    public class ColorNameService : IColorNameService
    {
        private readonly CovideDataContext _db;

        public ColorNameService(CovideDataContext db)
        {
            _db = db;
        }

        public string GetName(string hexTriplet)
        {
            return _db.ColorCodes
                .FirstOrDefault(cc => cc.HexTriplet == hexTriplet.ToUpper())
                ?.Name;
        }
    }
}
