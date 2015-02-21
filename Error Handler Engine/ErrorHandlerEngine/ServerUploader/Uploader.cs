
using System.Threading.Tasks;
using ConnectionsManager;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    public static class Uploader
    {
        public static async Task<bool> SentOneErrorToDbAsync(LazyError error)
        {
            if (UploadController.CanToSent)
            {
                try
                {
                    await DynamicStoredProcedures.InsertErrorStoredProcedureAsync(error);
                }
                catch
                {
                    UploadController.CanToSent = false;
                }
            }
            return UploadController.CanToSent;
        }

        public static bool SentOneErrorToDb(LazyError error)
        {
            if (UploadController.CanToSent)
            {
                try
                {
                    DynamicStoredProcedures.InsertErrorStoredProcedure(error);
                }
                catch
                {
                    UploadController.CanToSent = false;
                }
            }
            return UploadController.CanToSent;
        }
    }
}
