$(document).ready(function () {

    var baseUrl = "https://www.eventnx.com/";
    var authcode = getUrlVars()["code"];
    var error = getUrlVars()["error"];
    var errordesc = getUrlVars()["error_description"];
    var eventkey = getUrlVars()["eid"];
    var referralid = getUrlVars()["r"];
    var exists = getUrlVars()["exists"];
    var fbstate = getUrlVars()["state"];
    var fbStateObj = {};
    if (fbstate) {
        fbstate = fbstate.replace('#_', '');
        fbstate = decodeURIComponent(fbstate);
        console.log(fbstate.toString());
        fbStateObj = JSON.parse(fbstate);
        console.log(fbStateObj);
    }
    if (referralid === "" || referralid === null || referralid === undefined) {
        referralid = 0;
    }
    
    var token = "";
    //If user has logged in into linked In and we have the authtoken
    if (eventkey !== undefined) {
        if (eventkey.length === 36) {
            //If we have the authcode then only try to generate access token
            if (jQuery.type(authcode) !== "undefined") {
                $.ajax({
                    url: baseUrl + "/Home/GenerateToken",
                    data: {
                        "code": authcode, "eventkey": eventkey, "referralid": referralid
                    },
                    success: function (response) {
                        //Once access token is genereated, fetch user profile details and store into RegisteredUser table 
                        $.ajax({
                            url: baseUrl + "/Home/Authenticate",
                            data: { "accesstoken": JSON.parse(response).access_token, "eventkey": eventkey, "referralid": referralid },
                            success: function (response) {
                                window.location.href = response.eventurl;
                            }
                        });

                    }
                });
            }
        }
    }
    if (fbstate) {
        $.ajax({
            url: baseUrl + "/Home/FBGenerateToken",
            data: {
                "code": authcode, "eventid": fbStateObj.eid, "referralid": fbStateObj.referralid
            },
            success: function (response) {
                console.log(response);
                //Once access token is genereated, fetch user profile details and store into RegisteredUser table 
                $.ajax({
                    url: baseUrl + "/Home/GetFBProfile",
                    data: { "accesstoken": JSON.parse(response).access_token, "eventid": fbStateObj.eid, "referralid": referralid },
                    success: function (response) {
                        window.location.href = response.eventurl;
                    }
                });

            }
        });
    }
    if (error !== "user_cancelled_login") {
        $('#lblErrorMessage').text(errordesc);
    }
    if (error !== "user_cancelled_authorize") {
        $('#lblErrorMessage').text(errordesc);
    }

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
    
});