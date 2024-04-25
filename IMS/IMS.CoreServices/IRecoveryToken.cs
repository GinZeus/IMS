using IMS.Models;

namespace IMS.CoreServices
{
    public interface IRecoveryToken
    {
        // Store code and uid into RecoveryToken
        public void AddRecoveryToken(RecoveryToken recoveryToken);
        // Get RecoveryToken by code
        public RecoveryToken GetRecoveryTokenByCode(string code);

        public RecoveryToken GetRecoveryTokenByUid(string uid);
        public void RemoveRecoveryToken(RecoveryToken recoveryToken);
    }
}
