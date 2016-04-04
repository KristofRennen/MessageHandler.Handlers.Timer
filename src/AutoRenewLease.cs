using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Timer
{
    public class AutoRenewLease : IDisposable
    {
        public bool HasLease { get { return _leaseId != null; } }

        private readonly CloudBlockBlob _blob;
        private string _leaseId;
        private Thread _renewalThread;
        private bool _disposed;

        public AutoRenewLease(CloudBlockBlob blob)
        {
            this._blob = blob;
            blob.Container.CreateIfNotExists();

            _renewalThread = new Thread(() =>
            {
                try
                {
                    if (HasLease)
                    {
                        _blob.RenewLease(new AccessCondition()
                        {
                            LeaseId = _leaseId
                        });
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(40));
                }
                catch (ThreadAbortException)
                {
                }
            });
            _renewalThread.Start();
        }

        public async Task<AutoRenewLease> TryAcquire()
        {
            if (!HasLease)
            {
                _leaseId = await _blob.TryAcquireLease();
            }
            return this;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            if (_renewalThread != null)
            {
                _renewalThread.Abort();
                _blob.ReleaseLease(new AccessCondition()
                                  {
                                      LeaseId = _leaseId
                                  });
                _renewalThread = null;
            }
            _disposed = true;
        }
    }
}