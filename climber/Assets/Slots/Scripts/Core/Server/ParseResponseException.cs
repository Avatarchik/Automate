using System;

public class ParseResponseException : Exception {

    private int ServerCode;
    private string SysName;
    
    public ParseResponseException (string detailMessage, int serverCode, string sysName) : 
                                                    this(detailMessage, serverCode, sysName, null) {
    }
    
    public ParseResponseException (string detailMessage, int serverCode, string sysName, Exception exception) : base(detailMessage, exception) {
        ServerCode = serverCode;
        SysName = sysName;
    }
    
    public int GetServerCode () {
        return ServerCode;
    }
    
    public String GetSysName () {
        return SysName;
    }
    
    public override string ToString () {
        return string.Format ("{0}, server code: {1}", base.ToString (), ServerCode);
    }
}
