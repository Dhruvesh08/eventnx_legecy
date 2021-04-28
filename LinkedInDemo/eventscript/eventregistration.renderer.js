$(document).ready(function () {
    var baseUrl = "https://www.eventnx.com/";
    var crmdata = {};
    var eventid;
    if ($('.linkedinlogin').length > 0)
        eventid = $('.linkedinlogin').attr("id").split('_')[1];
    else
        eventid = 0;
    
    var userid = getUrlVars()["userid"];
    var accesstoken = getUrlVars()["accesstoken"];
    var profileid = getUrlVars()["profileid"];
    var eid = getUrlVars()["eid"];
    var referralid = getUrlVars()["r"];
    var exists = getUrlVars()["exists"];
    var allowpost = getUrlVars()["allowpost"];
    var cancelmsg = getUrlVars()["error"];
    var state = getUrlVars()["state"];
    if (referralid === "" || referralid === null || referralid === undefined) {
        referralid = 0;
    }
    checkCookie();
    if (cancelmsg === "user_cancelled_login") {
        $.ajax({
            method: 'POST',
            url: baseUrl + '/Home/GetEventDetails',
            data: {
                "EventKey": eid
            },
            success: function (response) {
                window.location.href = response.domainname;
            }
        });
    }
    //if user has successfully authenticated with linkedIn, Its time to render form
    if ($('.eventregistration').length > 0 && userid !== undefined)
    {
		createCookie("gepuserid", userid, 1);
        $('#eventregistration').append("<input type='hidden' id='hidEventProUserId' value='" + userid + "' />"
            + "<input type='hidden' id='hidAccesstoken' value='" + accesstoken + "' />"
            + "<input type='hidden' id='hidEventKey' name='hidEventKey' style='display:none;'>"
            + "<input type='hidden' id='hidProfileId' name='hidProfileId' style='display:none;'>"
            + "<input type='hidden' id='hidProfileImage' name='hidProfileImage' style='display:none;'>"
            + "<input type='hidden' id='hidFirstName' name='hidFirstName' style='display:none;'>"
            + "<input type='hidden' id='hidLastName' name='hidLastName' style='display:none;'>"
            + "<input type='hidden' id='hidEmail' name='hidEmail' style='display:none;'>"
            + "<input type='hidden' id='hidAllowPost' name='hidEmail' value='" + allowpost + "' style='display:none;'>"
        );

        GetRegisteredUser();
    }
    else if ($('.linkedinlogin').length > 0)
    {
        $('.linkedinlogin').html(
            "<div>Activating Social Login...</div><div class='lds-ellipsis'><div></div><div></div><div></div><div></div></div><br/>"
        );

        $.ajax({
            method: 'POST',
            url: baseUrl + '/Home/GetEventDetails',
            data: {
                "EventKey": eventid,
                "referralid": referralid
            },
            success: function (response) {
                $('.linkedinlogin').html(
                    "<a id='btnLinkedIn' target='blank' href='https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=81wyrvvax51otg&scope=r_emailaddress%20r_liteprofile%20w_member_social&redirect_uri=https%3A%2F%2Fwww.eventnx.com%2F%3Feid%3D" + eventid + "%26r%3D" + referralid + "'>"
                    + "<img src='" + response.buttonUrl + "'></a>" +
                    "<a  style='display:none' id='btnFacebook' target='blank' href='https://www.facebook.com/v5.0/dialog/oauth?client_id=482419392462772&redirect_uri=https://www.eventnx.com/&auth_type=rerequest&scope=email&state={\"\eid\"\:" + response.eventid + ",\"\r\"\:" + referralid + "}'>"
                    + "<img src='" + response.fbButtonUrl + "'></a>"
                );
                $('#divallowpost').show();
                createCookie("gepallowpost", $('#AllowPost').prop("checked"), 1);
            }
        });
    }

    function updateQueryStringParameter(uri, key, value) {
        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.match(re)) {
            return uri.replace(re, '$1' + key + "=" + value + '$2');
        }
        else {
            return uri + separator + key + "=" + value;
        }
    }

    $('#AllowPost').change(function () {
        createCookie("gepallowpost", $('#AllowPost').prop("checked"), 1);
    });

    function GetRegisteredUser() {
    	$.ajax({
    		method: 'POST',
    		url: baseUrl + '/Home/GetRegisteredUser',
    		data: {
    			UserId: userid
    		},
    		beforeSend: function () { },
            success: function (response) {
               
                if (response.success) {
                    $('.linkedinlogin').hide();
                    $('.eventregistration').show();
                    $('#hidEventKey').val(eid);
                    $('#hidProfileId').val(response.user.Email);
                    $('#hidProfileImage').val(response.user.ProfileImage);
                    $('#hidFirstName').val(response.user.FirstName);
                    $('#hidLastName').val(response.user.LastName);
                    $('#hidEmail').val(response.user.Email);
    			}
    		}
    	});
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

    function createCookie(cookieName, cookieValue, daysToExpire) {
        var date = new Date();
        date.setTime(date.getTime() + (daysToExpire * 24 * 60 * 60 * 1000));
        document.cookie = cookieName + "=" + cookieValue + "; expires=" + date.toGMTString();
    }

    function accessCookie(cookieName) {
        var name = cookieName + "=";
        var allCookieArray = document.cookie.split(';');
        for (var i = 0; i < allCookieArray.length; i++) {
            var temp = allCookieArray[i].trim();
            if (temp.indexOf(name) == 0)
                return temp.substring(name.length, temp.length);
        }
        return "";
    }

    function checkCookie() {
        if (referralid === "" || referralid === null || referralid === undefined) {
            var user = accessCookie("gepreferral");
            if (user != "") {
                referralid = user;
            }
            else {
                if (referralid === "" || referralid === null || referralid === undefined) {
                    referralid = 0;
                }
                else {
                    createCookie("gepreferral", referralid, 30);
                }
            }
        }
        else {
            createCookie("gepreferral", referralid, 30);
        }
        
    }
});