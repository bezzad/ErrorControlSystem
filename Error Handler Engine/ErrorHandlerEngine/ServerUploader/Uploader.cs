
using System.Threading.Tasks;
using ConnectionsManager;
using ErrorHandlerEngine.ModelObjecting;

namespace ErrorHandlerEngine.ServerUploader
{
    public static class Uploader
    {
        public static async Task<bool> SentOneErrorToDbAsync(ConnectionManager conn, LazyError error)
        {
            if (UploadController.CanToSent)
            {
                try
                {
                    await DynamicStoredProcedures.InsertErrorStoredProcedureAsync(conn, error);
                }
                catch
                {
                    UploadController.CanToSent = false;
                }
            }
            return UploadController.CanToSent;
        }

        public static bool SentOneErrorToDb(ConnectionManager conn, LazyError error)
        {
            if (UploadController.CanToSent)
            {
                try
                {
                    DynamicStoredProcedures.InsertErrorStoredProcedure(conn, error);
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
