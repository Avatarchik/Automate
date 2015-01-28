using System;

public sealed class ValidationException : ParseResponseException {

    private string FieldName;
    
    public ValidationException (string detailMessage, int serverCode, string sysName, string fieldName) : 
                                                    this (detailMessage, serverCode, sysName, fieldName, null) {
    }
    
    public ValidationException (string detailMessage, int serverCode, string sysName, string fieldName, Exception exception) : 
                                                                            base(detailMessage, serverCode, sysName, exception) {
        FieldName = fieldName;
    }
    
    public string GetFieldName () {
        return FieldName;
    }
    
    public override String ToString () {
        return string.Format ("Validation error for field [{0}]: name: [{1}], server code: [{2}], msg: [{3}]",
                                    FieldName, GetSysName (), GetServerCode (), Message);
    }
}
