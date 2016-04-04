using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Timer
{
    public static class LeaseBlobExtensions
    {
        public static async Task<string> TryAcquireLease(this CloudBlockBlob blob)
        {
            try
            {
                return await blob.AcquireLeaseAsync(TimeSpan.FromSeconds(60), null);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> TryRenewLease(this CloudBlockBlob blob, string leaseId)
        {
            try
            {
                await blob.RenewLeaseAsync(new AccessCondition
                {
                    LeaseId = leaseId
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}