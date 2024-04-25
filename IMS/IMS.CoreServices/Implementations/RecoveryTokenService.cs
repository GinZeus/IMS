using IMS.Data;
using IMS.Models;

namespace IMS.CoreServices.Implementations
{
    public class RecoveryTokenService : IRecoveryToken
    {
        private readonly ApplicationDbContext _dbContext;

        public RecoveryTokenService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRecoveryToken(RecoveryToken recoveryToken)
        {
            try
            {
                _dbContext.RecoveryTokens.Add(recoveryToken);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public RecoveryToken GetRecoveryTokenByCode(string code)
        {
            try
            {
                var recoveryToken = _dbContext.RecoveryTokens
                    .SingleOrDefault(rt => rt.Code == code);
                return recoveryToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public RecoveryToken GetRecoveryTokenByUid(string uid)
        {
            try
            {
                var recoveryToken = _dbContext.RecoveryTokens
                    .SingleOrDefault(rt => rt.UserID == uid);
                return recoveryToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void RemoveRecoveryToken(RecoveryToken recoveryToken)
        {
            try
            {
                _dbContext.RecoveryTokens.Remove(recoveryToken);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
