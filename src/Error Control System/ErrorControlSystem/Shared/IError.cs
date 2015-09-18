using System;
using System.Collections.Generic;
using System.Drawing;

namespace ErrorControlSystem.Shared
{
    public interface IError : IEquatable<IError>
    {
        int Id { set; get; }
        Boolean IsHandled { get; set; }
        DateTime ErrorDateTime { get; set; }
        DateTime ServerDateTime { get; set; }
        int HResult { get; set; }
        String AppName { get; set; }
        String ClrVersion { get; set; }
        String CurrentCulture { get; set; }
        String ErrorType { get; set; }
        String Host { get; set; }
        String IPv4Address { get; set; }
        String MacAddress { get; set; }
        String MemberType { get; set; }
        String Message { get; set; }
        String Method { get; set; }
        String ModuleName { get; set; }
        string OS { get; set; }
        string Processes { get; set; }
        String Source { get; set; }
        String StackTrace { get; set; }
        String User { get; set; }
        CodeScope LineColumn { get; set; }
        int Duplicate { get; set; }
        String Data { get; set; }
        System.Drawing.Image Snapshot { get; }
    }
}