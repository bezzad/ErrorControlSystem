﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArchitectModeling
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="UsersManagements")]
	public partial class DatabaseModelDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertErrorLog(ErrorLog instance);
    partial void UpdateErrorLog(ErrorLog instance);
    partial void DeleteErrorLog(ErrorLog instance);
    partial void InsertSnapshot(Snapshot instance);
    partial void UpdateSnapshot(Snapshot instance);
    partial void DeleteSnapshot(Snapshot instance);
    #endregion
		
		public DatabaseModelDataContext() : 
				base(global::ArchitectModeling.Properties.Settings.Default.UsersManagementsConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseModelDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseModelDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseModelDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseModelDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ErrorLog> ErrorLogs
		{
			get
			{
				return this.GetTable<ErrorLog>();
			}
		}
		
		public System.Data.Linq.Table<Snapshot> Snapshots
		{
			get
			{
				return this.GetTable<Snapshot>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.sp_InsertErrorLog")]
		public int sp_InsertErrorLog(
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ServerDateTime", DbType="DateTime")] System.Nullable<System.DateTime> serverDateTime, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Host", DbType="VarChar(200)")] string host, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="User", DbType="VarChar(200)")] string user, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsHandled", DbType="Bit")] System.Nullable<bool> isHandled, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Type", DbType="VarChar(200)")] string type, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="AppName", DbType="VarChar(200)")] string appName, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ScreenCapture", DbType="Image")] System.Data.Linq.Binary screenCapture, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="CurrentCulture", DbType="NVarChar(100)")] string currentCulture, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="CLRVersion", DbType="VarChar(100)")] string cLRVersion, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Message", DbType="NVarChar(MAX)")] string message, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Source", DbType="NVarChar(MAX)")] string source, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="StackTrace", DbType="NVarChar(MAX)")] string stackTrace, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ModuleName", DbType="VarChar(200)")] string moduleName, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="MemberType", DbType="VarChar(200)")] string memberType, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Method", DbType="VarChar(500)")] string method, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Processes", DbType="VarChar(MAX)")] string processes, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorDateTime", DbType="DateTime")] System.Nullable<System.DateTime> errorDateTime, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="OS", DbType="VarChar(1000)")] string oS, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="IPv4Address", DbType="VarChar(50)")] string iPv4Address, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="MACAddress", DbType="VarChar(50)")] string mACAddress, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="HResult", DbType="Int")] System.Nullable<int> hResult, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="LineCol", DbType="VarChar(50)")] string lineCol, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Duplicate", DbType="Int")] System.Nullable<int> duplicate, 
					[global::System.Data.Linq.Mapping.ParameterAttribute(Name="Data", DbType="Xml")] System.Xml.Linq.XElement data)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), serverDateTime, host, user, isHandled, type, appName, screenCapture, currentCulture, cLRVersion, message, source, stackTrace, moduleName, memberType, method, processes, errorDateTime, oS, iPv4Address, mACAddress, hResult, lineCol, duplicate, data);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetErrorHandlingOptions", IsComposable=true)]
		public System.Nullable<int> GetErrorHandlingOptions()
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod()))).ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ErrorLog")]
	public partial class ErrorLog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ErrorId;
		
		private System.Nullable<System.DateTime> _ServerDateTime;
		
		private string _Host;
		
		private string _User;
		
		private System.Nullable<bool> _IsHandled;
		
		private string _Type;
		
		private string _AppName;
		
		private System.Xml.Linq.XElement _Data;
		
		private string _CurrentCulture;
		
		private string _CLRVersion;
		
		private string _Message;
		
		private string _Source;
		
		private string _StackTrace;
		
		private string _ModuleName;
		
		private string _MemberType;
		
		private string _Method;
		
		private string _Processes;
		
		private System.Nullable<System.DateTime> _ErrorDateTime;
		
		private string _OS;
		
		private string _IPv4Address;
		
		private string _MACAddress;
		
		private System.Nullable<int> _HResult;
		
		private string _LineColumn;
		
		private System.Nullable<int> _DuplicateNo;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnErrorIdChanging(long value);
    partial void OnErrorIdChanged();
    partial void OnServerDateTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnServerDateTimeChanged();
    partial void OnHostChanging(string value);
    partial void OnHostChanged();
    partial void OnUserChanging(string value);
    partial void OnUserChanged();
    partial void OnIsHandledChanging(System.Nullable<bool> value);
    partial void OnIsHandledChanged();
    partial void OnTypeChanging(string value);
    partial void OnTypeChanged();
    partial void OnAppNameChanging(string value);
    partial void OnAppNameChanged();
    partial void OnDataChanging(System.Xml.Linq.XElement value);
    partial void OnDataChanged();
    partial void OnCurrentCultureChanging(string value);
    partial void OnCurrentCultureChanged();
    partial void OnCLRVersionChanging(string value);
    partial void OnCLRVersionChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnSourceChanging(string value);
    partial void OnSourceChanged();
    partial void OnStackTraceChanging(string value);
    partial void OnStackTraceChanged();
    partial void OnModuleNameChanging(string value);
    partial void OnModuleNameChanged();
    partial void OnMemberTypeChanging(string value);
    partial void OnMemberTypeChanged();
    partial void OnMethodChanging(string value);
    partial void OnMethodChanged();
    partial void OnProcessesChanging(string value);
    partial void OnProcessesChanged();
    partial void OnErrorDateTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnErrorDateTimeChanged();
    partial void OnOSChanging(string value);
    partial void OnOSChanged();
    partial void OnIPv4AddressChanging(string value);
    partial void OnIPv4AddressChanged();
    partial void OnMACAddressChanging(string value);
    partial void OnMACAddressChanged();
    partial void OnHResultChanging(System.Nullable<int> value);
    partial void OnHResultChanged();
    partial void OnLineColumnChanging(string value);
    partial void OnLineColumnChanged();
    partial void OnDuplicateNoChanging(System.Nullable<int> value);
    partial void OnDuplicateNoChanged();
    #endregion
		
		public ErrorLog()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorId", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long ErrorId
		{
			get
			{
				return this._ErrorId;
			}
			set
			{
				if ((this._ErrorId != value))
				{
					this.OnErrorIdChanging(value);
					this.SendPropertyChanging();
					this._ErrorId = value;
					this.SendPropertyChanged("ErrorId");
					this.OnErrorIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServerDateTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> ServerDateTime
		{
			get
			{
				return this._ServerDateTime;
			}
			set
			{
				if ((this._ServerDateTime != value))
				{
					this.OnServerDateTimeChanging(value);
					this.SendPropertyChanging();
					this._ServerDateTime = value;
					this.SendPropertyChanged("ServerDateTime");
					this.OnServerDateTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Host", DbType="VarChar(200)")]
		public string Host
		{
			get
			{
				return this._Host;
			}
			set
			{
				if ((this._Host != value))
				{
					this.OnHostChanging(value);
					this.SendPropertyChanging();
					this._Host = value;
					this.SendPropertyChanged("Host");
					this.OnHostChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[User]", Storage="_User", DbType="VarChar(200)")]
		public string User
		{
			get
			{
				return this._User;
			}
			set
			{
				if ((this._User != value))
				{
					this.OnUserChanging(value);
					this.SendPropertyChanging();
					this._User = value;
					this.SendPropertyChanged("User");
					this.OnUserChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsHandled", DbType="Bit")]
		public System.Nullable<bool> IsHandled
		{
			get
			{
				return this._IsHandled;
			}
			set
			{
				if ((this._IsHandled != value))
				{
					this.OnIsHandledChanging(value);
					this.SendPropertyChanging();
					this._IsHandled = value;
					this.SendPropertyChanged("IsHandled");
					this.OnIsHandledChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="VarChar(200)")]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AppName", DbType="VarChar(200)")]
		public string AppName
		{
			get
			{
				return this._AppName;
			}
			set
			{
				if ((this._AppName != value))
				{
					this.OnAppNameChanging(value);
					this.SendPropertyChanging();
					this._AppName = value;
					this.SendPropertyChanged("AppName");
					this.OnAppNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Data", DbType="Xml", CanBeNull=true, UpdateCheck=UpdateCheck.Never)]
		public System.Xml.Linq.XElement Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				if ((this._Data != value))
				{
					this.OnDataChanging(value);
					this.SendPropertyChanging();
					this._Data = value;
					this.SendPropertyChanged("Data");
					this.OnDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CurrentCulture", DbType="NVarChar(200)")]
		public string CurrentCulture
		{
			get
			{
				return this._CurrentCulture;
			}
			set
			{
				if ((this._CurrentCulture != value))
				{
					this.OnCurrentCultureChanging(value);
					this.SendPropertyChanging();
					this._CurrentCulture = value;
					this.SendPropertyChanged("CurrentCulture");
					this.OnCurrentCultureChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CLRVersion", DbType="VarChar(100)")]
		public string CLRVersion
		{
			get
			{
				return this._CLRVersion;
			}
			set
			{
				if ((this._CLRVersion != value))
				{
					this.OnCLRVersionChanging(value);
					this.SendPropertyChanging();
					this._CLRVersion = value;
					this.SendPropertyChanged("CLRVersion");
					this.OnCLRVersionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="NVarChar(MAX)")]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Source", DbType="NVarChar(MAX)")]
		public string Source
		{
			get
			{
				return this._Source;
			}
			set
			{
				if ((this._Source != value))
				{
					this.OnSourceChanging(value);
					this.SendPropertyChanging();
					this._Source = value;
					this.SendPropertyChanged("Source");
					this.OnSourceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StackTrace", DbType="NVarChar(MAX)")]
		public string StackTrace
		{
			get
			{
				return this._StackTrace;
			}
			set
			{
				if ((this._StackTrace != value))
				{
					this.OnStackTraceChanging(value);
					this.SendPropertyChanging();
					this._StackTrace = value;
					this.SendPropertyChanged("StackTrace");
					this.OnStackTraceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ModuleName", DbType="VarChar(200)")]
		public string ModuleName
		{
			get
			{
				return this._ModuleName;
			}
			set
			{
				if ((this._ModuleName != value))
				{
					this.OnModuleNameChanging(value);
					this.SendPropertyChanging();
					this._ModuleName = value;
					this.SendPropertyChanged("ModuleName");
					this.OnModuleNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MemberType", DbType="VarChar(200)")]
		public string MemberType
		{
			get
			{
				return this._MemberType;
			}
			set
			{
				if ((this._MemberType != value))
				{
					this.OnMemberTypeChanging(value);
					this.SendPropertyChanging();
					this._MemberType = value;
					this.SendPropertyChanged("MemberType");
					this.OnMemberTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Method", DbType="VarChar(500)")]
		public string Method
		{
			get
			{
				return this._Method;
			}
			set
			{
				if ((this._Method != value))
				{
					this.OnMethodChanging(value);
					this.SendPropertyChanging();
					this._Method = value;
					this.SendPropertyChanged("Method");
					this.OnMethodChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Processes", DbType="VarChar(MAX)")]
		public string Processes
		{
			get
			{
				return this._Processes;
			}
			set
			{
				if ((this._Processes != value))
				{
					this.OnProcessesChanging(value);
					this.SendPropertyChanging();
					this._Processes = value;
					this.SendPropertyChanged("Processes");
					this.OnProcessesChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorDateTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> ErrorDateTime
		{
			get
			{
				return this._ErrorDateTime;
			}
			set
			{
				if ((this._ErrorDateTime != value))
				{
					this.OnErrorDateTimeChanging(value);
					this.SendPropertyChanging();
					this._ErrorDateTime = value;
					this.SendPropertyChanged("ErrorDateTime");
					this.OnErrorDateTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OS", DbType="VarChar(1000)")]
		public string OS
		{
			get
			{
				return this._OS;
			}
			set
			{
				if ((this._OS != value))
				{
					this.OnOSChanging(value);
					this.SendPropertyChanging();
					this._OS = value;
					this.SendPropertyChanged("OS");
					this.OnOSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IPv4Address", DbType="VarChar(50)")]
		public string IPv4Address
		{
			get
			{
				return this._IPv4Address;
			}
			set
			{
				if ((this._IPv4Address != value))
				{
					this.OnIPv4AddressChanging(value);
					this.SendPropertyChanging();
					this._IPv4Address = value;
					this.SendPropertyChanged("IPv4Address");
					this.OnIPv4AddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MACAddress", DbType="VarChar(50)")]
		public string MACAddress
		{
			get
			{
				return this._MACAddress;
			}
			set
			{
				if ((this._MACAddress != value))
				{
					this.OnMACAddressChanging(value);
					this.SendPropertyChanging();
					this._MACAddress = value;
					this.SendPropertyChanged("MACAddress");
					this.OnMACAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HResult", DbType="Int")]
		public System.Nullable<int> HResult
		{
			get
			{
				return this._HResult;
			}
			set
			{
				if ((this._HResult != value))
				{
					this.OnHResultChanging(value);
					this.SendPropertyChanging();
					this._HResult = value;
					this.SendPropertyChanged("HResult");
					this.OnHResultChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LineColumn", DbType="VarChar(50)")]
		public string LineColumn
		{
			get
			{
				return this._LineColumn;
			}
			set
			{
				if ((this._LineColumn != value))
				{
					this.OnLineColumnChanging(value);
					this.SendPropertyChanging();
					this._LineColumn = value;
					this.SendPropertyChanged("LineColumn");
					this.OnLineColumnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DuplicateNo", DbType="Int")]
		public System.Nullable<int> DuplicateNo
		{
			get
			{
				return this._DuplicateNo;
			}
			set
			{
				if ((this._DuplicateNo != value))
				{
					this.OnDuplicateNoChanging(value);
					this.SendPropertyChanging();
					this._DuplicateNo = value;
					this.SendPropertyChanged("DuplicateNo");
					this.OnDuplicateNoChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Snapshots")]
	public partial class Snapshot : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ErrorLogID;
		
		private System.Data.Linq.Binary _ScreenCapture;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnErrorLogIDChanging(int value);
    partial void OnErrorLogIDChanged();
    partial void OnScreenCaptureChanging(System.Data.Linq.Binary value);
    partial void OnScreenCaptureChanged();
    #endregion
		
		public Snapshot()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorLogID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int ErrorLogID
		{
			get
			{
				return this._ErrorLogID;
			}
			set
			{
				if ((this._ErrorLogID != value))
				{
					this.OnErrorLogIDChanging(value);
					this.SendPropertyChanging();
					this._ErrorLogID = value;
					this.SendPropertyChanged("ErrorLogID");
					this.OnErrorLogIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScreenCapture", DbType="Image NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary ScreenCapture
		{
			get
			{
				return this._ScreenCapture;
			}
			set
			{
				if ((this._ScreenCapture != value))
				{
					this.OnScreenCaptureChanging(value);
					this.SendPropertyChanging();
					this._ScreenCapture = value;
					this.SendPropertyChanged("ScreenCapture");
					this.OnScreenCaptureChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
