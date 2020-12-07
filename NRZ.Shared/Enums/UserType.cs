using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Shared.Enums
{
    public enum UserType
    {
        CHSI,
        CHSIHelper,
        DSI,
        NAP,
        SYN,
        AUCPAR
    }

    public enum AuthType
    {
        ESIGN,
        EAUTH,
        USER,
        CHSI
    }
}
