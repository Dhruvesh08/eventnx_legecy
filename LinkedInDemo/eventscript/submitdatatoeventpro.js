$(document).ready(function () {
    var baseUrl = "http://localhost:52315/";

    SaveUser();
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

   
    function SaveUser() {

        var userid = accessCookie("gepuserid");
        var allowPost = accessCookie("gepallowpost");
        if (allowPost == undefined || allowPost == '' || allowPost == null) {
            allowPost = true;
        }

        $.ajax({
            method: 'POST',
            url: baseUrl + '/Home/UpdateUserFromCRM',
            data: {
                UserId: accessCookie("gepuserid"),
                firstname: $('#submitdatatoeventpro').attr('data-firstname'),
                lastname: $('#submitdatatoeventpro').attr('data-lastname'),
                emailid: $('#submitdatatoeventpro').attr('data-emailid'),
                companyname: $('#submitdatatoeventpro').attr('data-companyname'),
                jobtitle: $('#submitdatatoeventpro').attr('data-jobtitle'),
                phone: $('#submitdatatoeventpro').attr('data-phone'),
                senioritylevel: $('#submitdatatoeventpro').attr('data-senioritylevel'),
                primaryjob: $('#submitdatatoeventpro').attr('data-primaryjob'),
                natureofbusiness: $('#submitdatatoeventpro').attr('data-natureofbusiness'),
                country: $('#submitdatatoeventpro').attr('data-country'),
                topicofinterest: $('#submitdatatoeventpro').attr('data-topicofinterest'),
                registeredforglobal: $('#submitdatatoeventpro').attr('data-registeredforglobal'),
                checkbox1: $('#submitdatatoeventpro').attr('data-checkbox1'),
                checkbox2: $('#submitdatatoeventpro').attr('data-checkbox2'),
                checkbox3: $('#submitdatatoeventpro').attr('data-checkbox3'),
                checkbox4: $('#submitdatatoeventpro').attr('data-checkbox4'),
                iszoomevent: $('#submitdatatoeventpro').attr('data-iszoomevent'),
                isbigmarkerevent: $('#submitdatatoeventpro').attr('data-isbigmarkerevent'),
                crmregid: $('#submitdatatoeventpro').attr('data-crmregid'),
                allowpost: allowPost
            },
            success: function (response) {
                
            },
            error: function (err) {
                console.log(err);
            }
        });

        $.ajax({
            method: 'POST',
            url: baseUrl + '/Home/GetRegisteredUserByUserId',
            data: {
                UserId: userid
            },
            success: function (response) {
                if (response.success) {
                    var shortUrl = response.ReferralLink;;
                    $("#divsuccessmessage").empty();
                    $("#divwhoisgoing").empty();
                    $("#divshareit").empty();
                    var event = response.RegisteredUser;
                    
                    var eventuser = "<div class='registered_person_page'>";
                    eventuser += " <div class='section-title grayheaer'><h3>See who else is coming!</h3> </div>" +
                        "<div class='serach_area grayheaer'> <table><tr> <td><input id='search-criteria' onkeyup='searchtext();' type='text' value='' placeholder='search keyword '></td> </tr></table></div>" +
                        "<div id='myTable' class='registered_person_area'><div id='content-1' class='content mCustomScrollbar'><ul>";
                    jQuery.each(event, function (i, val) {
                        eventuser += "<li> <div class='box_register tooltipbox'><div class='imagebox ' id = 'exampleModalCenter" + val.UserId + "'><img src=" + val.ProfileImage + " class='img-fluid'> </div><div class ='profilename'> " + val.CRM_CompanyName + "<br/>";
                        if (val.CRM_JobTitle != '') {
                            eventuser += "<span>";
                            eventuser += val.CRM_JobTitle + "</span><br/>";
                        }
                        eventuser += "</div><span class='tooltiptext'>" + val.FullName + "</span></div></li>";
                    });
                    eventuser += "</ul></div>";
                    eventuser += "</div>";
                   
                    $("#divsuccessmessage").show();
                    var divsuccessmessage = "<div class='section-title'><h3>Next step is to invite your colleagues</h3><small class='fnt16'> Copy the link below and forward it to your colleagues.</small></div><table><tr><td><div id='modelbody'>" + shortUrl + "</div></td><td><a id='btnCopy' onclick='CopyToClipboard();' href='#'> Copy URL</a></td></tr></table><div class='section-title'><hr/></div";

                    $("#divsuccessmessage").append(divsuccessmessage);
                    //for who is going
                    $("#divwhoisgoing").show();
                    $("#divwhoisgoing").append(eventuser);
					
                    //for Share It Message

                    //$("#divshareit").show();
                    //var divshareit = $("<div class='special1'><b>Email / Share It</b></div><table class='social_icon'><tr><td><a href='https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse'><img src='" + baseUrl + "/content/images/icon_facebook.png' alt='' /></a></td><td><img src='" + baseUrl + "/content/images/icon_twitter.png' alt='' /></td><td><img src='" + baseUrl + "/content/images/icon_linked.png' alt='' /></td><td><img src='" + baseUrl + "/content/images/icon_email.png' alt='' /></td></tr></table>");
                    //$("#divshareit").append(divshareit);
                }
            },
            error: function () {
                return false;
            }
        });

    }

});


function CopyToClipboard() {
    var elm = document.getElementById("modelbody");
    // for Internet Explorer

    if (document.body.createTextRange) {
        var range = document.body.createTextRange();
        range.moveToElementText(elm);
        range.select();
        document.execCommand("Copy");
      
    }
    else if (window.getSelection) {
        // other browsers

        var selection = window.getSelection();
        var range = document.createRange();
        range.selectNodeContents(elm);
        selection.removeAllRanges();
        selection.addRange(range);
        document.execCommand("Copy");
        alert("Url copied");
    }
}
function myModel() {

}
(function ($) {
    $(window).on("load", function () {
		//if($("#content-1")){
			//$("#content-1").mCustomScrollbar({
				//theme: "minimal"
			//});
		//}
    });
})(jQuery);

function searchtext() {
        var search = document.getElementById("search-criteria").value;
        var value = search.toLowerCase();
        $("#myTable li").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
}

