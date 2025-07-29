using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.AsyncProcessing;

public class LocksCollection : IDisposable
{
    private readonly List<ISyncLock> locksToRelease = new ();

    public LocksCollection Add(ISyncLock syncLock)
    {
        locksToRelease.Add(syncLock);

        return this;
    }

    public int ReleaseAll()
    {
        var count = locksToRelease.Count;
        for (int i = 0; i < count; i++)
        {
            locksToRelease[i].Release(true);
        }
        locksToRelease.Clear();
        return count;
    }

    public void Dispose()
    {
        ReleaseAll();
    }
}