// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FileUtil
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Helpers
{
  public static class FileUtil
  {
    private const int RmRebootReasonNone = 0;
    private const int CCH_RM_MAX_APP_NAME = 255;
    private const int CCH_RM_MAX_SVC_NAME = 63;

    [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
    private static extern int RmRegisterResources(
      uint pSessionHandle,
      uint nFiles,
      string[] rgsFilenames,
      uint nApplications,
      [In] FileUtil.RM_UNIQUE_PROCESS[] rgApplications,
      uint nServices,
      string[] rgsServiceNames);

    [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
    private static extern int RmStartSession(
      out uint pSessionHandle,
      int dwSessionFlags,
      string strSessionKey);

    [DllImport("rstrtmgr.dll")]
    private static extern int RmEndSession(uint pSessionHandle);

    [DllImport("rstrtmgr.dll")]
    private static extern int RmGetList(
      uint dwSessionHandle,
      out uint pnProcInfoNeeded,
      ref uint pnProcInfo,
      [In, Out] FileUtil.RM_PROCESS_INFO[] rgAffectedApps,
      ref uint lpdwRebootReasons);

    public static List<Process> WhoIsLocking(string path)
    {
      string strSessionKey = Guid.NewGuid().ToString();
      List<Process> processList = new List<Process>();
      uint pSessionHandle;
      if (FileUtil.RmStartSession(out pSessionHandle, 0, strSessionKey) != 0)
        throw new Exception("Could not begin restart session.  Unable to determine file locker.");
      try
      {
        uint pnProcInfoNeeded = 0;
        uint pnProcInfo1 = 0;
        uint lpdwRebootReasons = 0;
        string[] rgsFilenames = new string[1]{ path };
        if (FileUtil.RmRegisterResources(pSessionHandle, (uint) rgsFilenames.Length, rgsFilenames, 0U, (FileUtil.RM_UNIQUE_PROCESS[]) null, 0U, (string[]) null) != 0)
          throw new Exception("Could not register resource.");
        switch (FileUtil.RmGetList(pSessionHandle, out pnProcInfoNeeded, ref pnProcInfo1, (FileUtil.RM_PROCESS_INFO[]) null, ref lpdwRebootReasons))
        {
          case 0:
            break;
          case 234:
            FileUtil.RM_PROCESS_INFO[] rgAffectedApps = new FileUtil.RM_PROCESS_INFO[(int) pnProcInfoNeeded];
            uint pnProcInfo2 = pnProcInfoNeeded;
            if (FileUtil.RmGetList(pSessionHandle, out pnProcInfoNeeded, ref pnProcInfo2, rgAffectedApps, ref lpdwRebootReasons) != 0)
              throw new Exception("Could not list processes locking resource.");
            processList = new List<Process>((int) pnProcInfo2);
            for (int index = 0; (long) index < (long) pnProcInfo2; ++index)
            {
              try
              {
                processList.Add(Process.GetProcessById(rgAffectedApps[index].Process.dwProcessId));
              }
              catch (ArgumentException ex)
              {
              }
            }
            break;
          default:
            throw new Exception("Could not list processes locking resource. Failed to get size of result.");
        }
      }
      finally
      {
        FileUtil.RmEndSession(pSessionHandle);
      }
      return processList;
    }

    private struct RM_UNIQUE_PROCESS
    {
      public int dwProcessId;
      public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
    }

    private enum RM_APP_TYPE
    {
      RmUnknownApp = 0,
      RmMainWindow = 1,
      RmOtherWindow = 2,
      RmService = 3,
      RmExplorer = 4,
      RmConsole = 5,
      RmCritical = 1000, // 0x000003E8
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct RM_PROCESS_INFO
    {
      public FileUtil.RM_UNIQUE_PROCESS Process;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string strAppName;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
      public string strServiceShortName;
      public FileUtil.RM_APP_TYPE ApplicationType;
      public uint AppStatus;
      public uint TSSessionId;
      [MarshalAs(UnmanagedType.Bool)]
      public bool bRestartable;
    }
  }
}
