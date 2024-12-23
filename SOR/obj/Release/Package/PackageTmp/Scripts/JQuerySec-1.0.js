
function setLogoBackrgroundValues(logoK, logoV) {
    window.localStorage.setItem(logoK, logoV);
}

function getLogoBackrgroundValues(logoK) {
    var res = window.localStorage.getItem(logoK);
    return res;
}

function ridLogoBackrgroundValues(logoK) {
    window.localStorage.removeItem(logoK);
}

function clearLogoBackrgroundValues() {
    window.localStorage.clear();
}

function getKeyLogoBackrgroundValues(logoK) {
    var res = window.localStorage.removeItem(logoK);
    return res;
}


function UserLoggedIn(LogoK, LogoV, LogoH) {
    if (LogoK != null && LogoK != "" && LogoV != null && LogoV != "" && LogoH != null && LogoH != "") {
        localStorage.setItem("LogoK", LogoK);
        var request;
        if (window.XMLHttpRequest) {
            //New browsers.
            request = new XMLHttpRequest();
        }
        else if (window.ActiveXObject) {
            //Old IE Browsers.
            request = new ActiveXObject("Microsoft.XMLHTTP");
        }
        var url = "WebMethods.aspx/SetLogo";
        request.open("POST", url, false);
        var params = "{LogoV: '" + LogoV + "'}";
        request.setRequestHeader("Content-Type", "application/json");
        request.onreadystatechange = function () {
            if (request.readyState == 4 && request.status == 200) {
                var responsedata = JSON.parse(request.responseText)
                responsedata = JSON.parse(JSON.stringify(responsedata.d))
                if (responsedata.ResponseCode != localStorage.getItem("LogoK")) {
                    localStorage.setItem("UserID", "");
                    localStorage.setItem("LogoK", "");
                    window.location = "/frmLogin.aspx";
                    return;
                }
                else {

                    localStorage.setItem("UserID", LogoV);
                    window.location = LogoH;
                    return;
                }
            }
        };
        request.send(params);
    }
    else {
        window.location = "/frmLogin.aspx";
    }
};

function IsUserLoggedIn() {
    if (localStorage.getItem("UserID") != null && localStorage.getItem("UserID") != "") {
        var request;
        if (window.XMLHttpRequest) {
            //New browsers.
            request = new XMLHttpRequest();
        }
        else if (window.ActiveXObject) {
            //Old IE Browsers.
            request = new ActiveXObject("Microsoft.XMLHTTP");
        }
        var url = "WebMethods.aspx/GetLogo";
        request.open("POST", url, false);
        var params = "{LogoV: '" + localStorage.getItem("UserID") + "'}";
        request.setRequestHeader("Content-Type", "application/json");
        request.onreadystatechange = function () {
            if (request.readyState == 4 && request.status == 200) {
                var responsedata = JSON.parse(request.responseText)
                responsedata = JSON.parse(JSON.stringify(responsedata.d))
                if (responsedata.ResponseCode != "00") {
                    window.localStorage.clear();
                    window.location = "/frmLogin.aspx";
                    return;
                }
            }
        };
        request.send(params);
    }
    else {
        window.localStorage.clear();
        window.location = "/frmLogin.aspx";
    }
};

function UserLoggedOut() {
   
        var request;
        if (window.XMLHttpRequest) {
            //New browsers.
            request = new XMLHttpRequest();
        }
        else if (window.ActiveXObject) {
            //Old IE Browsers.
            request = new ActiveXObject("Microsoft.XMLHTTP");
        }

        
        var url = "WebMethods.aspx/RidLogo";
        request.open("POST", url, false);
        var params = "{}";
        request.setRequestHeader("Content-Type", "application/json");
        request.onreadystatechange = function () {
            if (request.readyState == 4 && request.status == 200) {
                var responsedata = JSON.parse(request.responseText)
                responsedata = JSON.parse(JSON.stringify(responsedata.d))
                if (responsedata.ResponseCode != "00")
                {
                    localStorage.setItem("UserID", "");
                    localStorage.clear();
                    window.location = "/frmLogin.aspx";
                    return;
                }
                else
                {
                    localStorage.setItem("UserID", "");
                    window.localStorage.clear();
                    window.location = "/frmLogin.aspx";
                    return;
                }
            }
        };
        request.send(params);
};
