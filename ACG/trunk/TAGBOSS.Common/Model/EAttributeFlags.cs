using System;

namespace TAGBOSS.Common.Model
{
  [Flags]
  public enum EAttributeFlags: uint
  {
    None = 0,
    IsDirty = 1,
    HasHistory = 2,
    IsInherited = 4,
    IsIncluded = 8,
    IsGenerated = 16,
    IsFunctionValue = 32,
    IsRefValue = 64,
    IsHistory = 128,
    IsLocal = 256,
    IsAnInclude = 512,
    FromLocalInclude = 1024,
    FromDefaultEntity = 2048,
    FromInclude = 4096,
    FromDefaultItem = 8192,
    IsTransCodeGenerator = 16384,
    IsTableHeader = 32768,
    IsFunctionEvaluated = 65536,
    IsAtAtEvaluated = 131072
  }
}
